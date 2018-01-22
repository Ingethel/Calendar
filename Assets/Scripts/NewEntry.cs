﻿using System.Collections.Generic;
using System;

public class Item
{
    public string[] attributes;
    public DataGroup dataGroupID;
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
    
    public override string ToString()
    {
        string value = "";
        if(attributes != null && attributes.Length > 0)
        {
            foreach (string att in attributes)
                value += att + " ";
        }
        return value;
    }
}

[Serializable]
public class Event : Item
{
    ColorGroup color;

    string startTime, endTime;

    public Event()
    {
        dataGroupID = SettingsManager.GetDataGroupID(DataGroup.DataGroups.EVENT, "default");
        color = SettingsManager.GetColourGroup("default");
        tag = DataStrings.Event;
        filler = true;
    }

    public Event(string day, DataGroup dG, ColorGroup cG, string[] attList)
    {
        SetDate(day);
        dataGroupID = dG;
        color = cG;
        tag = DataStrings.Event;
        attributes = attList;
        filler = false;
    }

    public Event(string day, string details)
    {

    }

    public int GetStartTime()
    {
        return TimeConversions.StringTimeToInt(startTime, 60);
    }

    public int GetEndTime()
    {
        return TimeConversions.StringTimeToInt(endTime, 60);
    }

    public string ToXMLText()
    {
        return color.Name + "," + startTime + "," + endTime + "," + ToString();
    }
}

[Serializable]
public class Alarm : Item
{
    public bool report = false;
    public Alarm()
    {
        tag = DataStrings.Alarm;
        attributes = new string[] { "" };
        filler = true;
    }

    public Alarm(string d, string n)
    {
        tag = DataStrings.Alarm;
        SetDate(d);
        attributes = new string[] { n };
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
    public List<string> Officers { private set; get; }
    public string id;

    public DAY()
    {
        Events = new NewEntryList();
        Alarms = new List<Alarm>();
        Officers = new List<string>() { "" };
        id = "";
    }

    public void AddGuide(Event n)
    {
        if(!n.filler)
            if(n.id == "")
                n.id = "_guide." + DataManager.GenerateNewIdFor("Guide");
        Events.Add(n);
    }
    
    public void AddEvent(Alarm n)
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