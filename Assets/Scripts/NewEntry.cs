using System.Collections.Generic;
using System;

public class Item
{
    public string[] attributes;
    public string dataGroupID = "";
    public DataGroup.DataGroups Type { protected set; get; }
    public string Date {
        get; protected set;
    }
    public string id = "";
    public bool filler = false;
    
    public int day, month, year;

    public void SetDate(string s)
    {
        Date = s;
        string[] temp = Date.Split('.');
        int.TryParse(temp[0], out day);
        int.TryParse(temp[1], out month);
        int.TryParse(temp[2], out year);
    }
    
    public override string ToString()
    {
        string value = "";
        if(attributes != null && attributes.Length > 0)
        {
            value = attributes[0];
            for (int i = 1; i < attributes.Length; i++)
                value += "," + attributes[i];
        }
        return value;
    }

    public virtual void XMLToObject(string text)
    {
        attributes = text.Split(',');
    }

    public virtual string ObjectToXML()
    {
        return ToString();
    }
    
}

public class Event : Item
{
    public string color = "";

    public string startTime = "", endTime = "";

    public Event()
    {
        Type = DataGroup.DataGroups.Event;
        dataGroupID = SettingsManager.GetDataGroup((int)Type)[0].Name;
        color = SettingsManager.GetColorGroups()[0].Name;
        filler = true;
    }

    public Event(string day, string dGName, string cGName, List<string> attList)
    {
        Type = DataGroup.DataGroups.Event;
        SetDate(day);
        dataGroupID = dGName;
        color = cGName;
        startTime = attList[0];
        endTime = attList[1];
        attributes = attList.GetRange(2, attList.Count-2).ToArray();
        filler = false;
    }

    public Event(string day, string dGName, string details)
    {
        Type = DataGroup.DataGroups.Event;
        SetDate(day);
        dataGroupID = dGName;
        XMLToObject(details);
        filler = false;
    }

    public int GetStartTime()
    {
        return TimeConversions.StringTimeToInt(startTime, 60);
    }

    public int GetEndTime()
    {
        return TimeConversions.StringTimeToInt(endTime, 60);
    }

    public override string ObjectToXML()
    {
        return color + "," + startTime + "," + endTime + "," + ToString();
    }

    public override void XMLToObject(string text)
    {
        string[] temp = text.Split(',');
        color = temp[0];
        startTime = temp[1];
        endTime = temp[2];
        int size = temp.Length - 3;
        attributes = new string[size];
        for(int i = 0; i < attributes.Length; i++)
        {
            attributes[i] = temp[i + 3];
        }
    }

}

public class Alarm : Item
{
    public bool report = false;

    public Alarm()
    {
        Type = DataGroup.DataGroups.Alarm;
        dataGroupID = SettingsManager.GetDataGroup((int)Type)[0].Name;
        filler = true;
    }

    public Alarm(string d, string dGName, string details)
    {
        Type = DataGroup.DataGroups.Alarm;
        dataGroupID = dGName;
        SetDate(d);
        XMLToObject(details);
        filler = false;
    }

    public Alarm(string d, string dGName, List<string> attList)
    {
        Type = DataGroup.DataGroups.Alarm;
        dataGroupID = dGName;
        SetDate(d);
        attributes = attList.ToArray();
        filler = false;
    }

}

public class NewEntryComparer : IComparer<Event>
{
    
    public int Compare(Event x, Event y)
    {
        int time_x = x.GetStartTime();
        int time_y = y.GetStartTime();

        if (time_x < time_y)
            return -1;
        else if (time_x > time_y)
            return 1;
        else
        {
            time_x = x.GetEndTime();
            time_y = y.GetEndTime();
            if (time_x < time_y)
                return -1;
            else if (time_x > time_y)
                return 1;
            else
            {
                return 0;
            }
        }
    }
}

public class NewEntryList
{
    private List<Event> list; 
    private NewEntryComparer comparer;

    public NewEntryList()
    {
        list = new List<Event>();
        comparer = new NewEntryComparer();
    }

    public void Remove(Event e)
    {
        list.Remove(e);
    }

    public void Add(Event e)
    {
        if (list == null || list.Count == 0)
            list = new List<Event> { e };
        else
        {
            int index = list.BinarySearch(e, comparer);
            if (index < 0)
                index = ~index;

            list.Insert(index, e);
        }
    }

    public void Clear()
    {
        if (list != null && list.Count > 0)
            list.Clear();
    }

    public int Count()
    {
        if (list != null)
            return list.Count;
        else
            return 0;
    }
    
    public bool TryGet(int i, out Event n)
    {
        if(i < list.Count)
        {
            n = list[i];
            return true;
        }
        else
        {
            n = null;
            return false;
        }
    }
}

public class TimeConversions
{

    public static string IntTimeToString(int time, int mod)
    {
        string minutes = (time % mod).ToString();
        string hours = (time / mod).ToString();
        while (hours.Length < 2)
        {
            hours = "0" + hours;
        }
        while (minutes.Length < 2)
        {
            minutes = "0" + minutes;
        }
        return hours + ":" + minutes;
    }

    public static int[] SplitString(string s, char c)
    {
        string[] time_s = s.Split(c);
        int[] time_int = new int[time_s.Length];
        for (int i = 0; i < time_s.Length; i++)
            int.TryParse(time_s[i], out time_int[i]);
        return time_int;
    }

    public static int StringTimeToInt(string s, int mod)
    {
        int[] c_time = SplitString(s, ':');
        if(c_time.Length == 2)
        {
            int time_i = c_time[0] * mod + c_time[1];
            return time_i;
        }
        else return 0;
    }

    public static bool IntInRange(int i, int min, int max)
    {
        return i >= min && i <= max;
    }

    public static string DateTimeToString(DateTime d)
    {
        return d.Day.ToString() + "." + d.Month.ToString() + "." + d.Year.ToString();
    }
}

public class DAY
{
    public NewEntryList Events { private set; get; }
    public List<Alarm> Alarms { private set; get; }
    public string Officers { private set; get; }
    public string TourGuides { private set; get; }
    public string id;

    public DAY()
    {
        Events = new NewEntryList();
        Alarms = new List<Alarm>();
        Officers = "";
        TourGuides = "";
        id = "";
    }

    public void AddEvent(Event n)
    {
        if(!n.filler)
            if(n.id == "")
                n.id = "_guide." + DataManager.GenerateNewIdFor("Guide");
        Events.Add(n);
    }
    
    public void AddAlarm(Alarm n)
    {
        if (n.report)
            foreach (Alarm a in Alarms)
                if (a.report)
                    return;

        if (n.report)
            Alarms.Insert(0, n);
        else if (n.id == "")
        {
            n.id = "_alarm." + DataManager.GenerateNewIdFor("Event");
            Alarms.Add(n);
        }
    }

    public void SetOfficer(string s)
    {
        Officers = s;
    }

    public string GetOfficer()
    {
        return Officers;
    }

    public void SetTourGuides(string s)
    {
        TourGuides = s;
    }

    public string GetTourGuides()
    {
        return TourGuides;
    }
    
}