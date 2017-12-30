using System.Collections.Generic;
using System;

public class Item
{
    public string[] labels;
    public string[] attributes;
    public string Date {
        get; protected set;
    }
    public string id = "";
    public bool filler = false;
    public string tag;

    public int day, month, year;

    public void SetDate(string s)
    {
        Date = s;
        string[] temp = Date.Split('.');
        int.TryParse(temp[0], out day);
        int.TryParse(temp[1], out month);
        int.TryParse(temp[2], out year);
    }
}

[Serializable]
public class NewEntry : Item
{

    public NewEntry()
    {
        tag = DataStrings.NewEntry;
        labels = new string[]{ DataStrings.StartTime, DataStrings.EndTime, DataStrings.NameOfTeam, DataStrings.NumberOfPeople, DataStrings.PersonInCharge, DataStrings.Telephone, DataStrings.ConfirmationDate, DataStrings.Guide, DataStrings.Notes, DataStrings.Colour };
        attributes = new string[] { "", "", "", "", "", "", "", "", "", "255.255.255" };
        filler = true;
    }

    public NewEntry(string[] list, string d)
    {
        tag = DataStrings.NewEntry;
        labels = new string[] { DataStrings.StartTime, DataStrings.EndTime, DataStrings.NameOfTeam, DataStrings.NumberOfPeople, DataStrings.PersonInCharge, DataStrings.Telephone, DataStrings.ConfirmationDate, DataStrings.Guide, DataStrings.Notes, DataStrings.Colour };
        attributes = list;
        SetDate(d);
        filler = false;
    }

    public int GetStartTime()
    {
        return TimeConversions.StringTimeToInt(attributes[0], 60);
    }

    public int GetEndTime()
    {
        return TimeConversions.StringTimeToInt(attributes[1], 60);
    }

    public int[] GetColor()
    {
        return TimeConversions.SplitString(attributes[attributes.Length - 1], '.');
    }
}

[Serializable]
public class Alarm : Item
{
    public bool report = false;
    public Alarm()
    {
        tag = DataStrings.Event;
        labels = new string[] { DataStrings.Notes/*, Strings.R_Days, Strings.R_Months, Strings.R_Years*/};
        attributes = new string[] { ""/*, "", "", ""*/ };
        filler = true;
    }

    public Alarm(string d, string n)
    {
        tag = DataStrings.Event;
        SetDate(d);
        labels = new string[] { DataStrings.Notes/*, Strings.R_Days, Strings.R_Months, Strings.R_Years*/ };
        attributes = new string[] { n/*, "0", "0", "0"*/ };
        filler = false;
    }
    
    /*
    public Alarm(string d, string n, int repeat, int by)
    {
        tag = Strings.Event;
        SetDate(d);
        labels = new string[] { Strings.Notes, Strings.R_Days, Strings.R_Months, Strings.R_Years };
        attributes = new string[] { n, "0", "0", "0" };
        switch (by)
        {
            case 0:
                attributes[1] = repeat.ToString();
                break;
            case 1:
                attributes[2] = repeat.ToString();
                break;
            case 2:
                attributes[3] = repeat.ToString();
                break;
            default:
                break;
        }
    }
    */
}


public class NewEntryComparer : IComparer<NewEntry>
{
    
    public int Compare(NewEntry x, NewEntry y)
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
    private List<NewEntry> list; 
    private NewEntryComparer comparer;

    public NewEntryList()
    {
        list = new List<NewEntry>();
        comparer = new NewEntryComparer();
    }

    public void Remove(NewEntry e)
    {
        list.Remove(e);
    }

    public void Add(NewEntry e)
    {
        if (list == null || list.Count == 0)
            list = new List<NewEntry> { e };
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
    
    public bool TryGet(int i, out NewEntry n)
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
        int time_i = c_time[0] * mod + c_time[1];
        return time_i;
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
    public NewEntryList Guides { private set; get; }
    public List<Alarm> Events { private set; get; }
    public List<string> Officers { private set; get; }
    public string id;

    public DAY()
    {
        Guides = new NewEntryList();
        Events = new List<Alarm>();
        Officers = new List<String>() { "" };
        id = "";
    }

    public void AddGuide(NewEntry n)
    {
        if(!n.filler)
            if(n.id == "")
                n.id = "_guide." + DataManager.GenerateNewIdFor("Guide");
        Guides.Add(n);
    }
    
    public void AddEvent(Alarm n)
    {
        if (n.report)
            foreach (Alarm a in Events)
                if (a.report)
                    return;

        if (n.report)
            Events.Insert(0, n);
        else if (n.id == "")
        {
            n.id = "_event." + DataManager.GenerateNewIdFor("Event");
            Events.Add(n);
        }
    }

    public void SetOfficer(string s)
    {
        Officers[0] = s;
    }

    public string GetOfficer()
    {
        return Officers[0];
    }

    public void AddOfficer(string s)
    {
        Officers.Add(s);
    }
}