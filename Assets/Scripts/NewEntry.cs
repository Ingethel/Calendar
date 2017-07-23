﻿using System.Collections.Generic;
using System;

public class Item
{
    public string[] labels;
    public string[] attributes;
    public string Date {
        get; protected set;
    }
    public string id = "";
}

public class NewEntry : Item
{
    public int day, month, year;
    public bool filler;

    public void SetDate(string s)
    {
        Date = s;
        string[] temp = Date.Split('.');
        int.TryParse(temp[0], out day);
        int.TryParse(temp[1], out month);
        int.TryParse(temp[2], out year);
    }

    public NewEntry()
    {
        labels = new string[]{ Strings.StartTime, Strings.EndTime, Strings.NameOfTeam, Strings.NumberOfPeople, Strings.PersonInCharge, Strings.Telephone, Strings.ConfirmationDate, Strings.Guide, Strings.Notes };
        attributes = new string[] { "", "", "", "", "", "", "", "", "" };
        filler = true;
    }

    public NewEntry(string[] list, string d)
    {
        labels = new string[] { Strings.StartTime, Strings.EndTime, Strings.NameOfTeam, Strings.NumberOfPeople, Strings.PersonInCharge, Strings.Telephone, Strings.ConfirmationDate, Strings.Guide, Strings.Notes };
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
}

public class Alarm : Item
{
    public string note = "";
    public int repeat_days = 0, repeat_months = 0, repeat_years = 0;

    public void SetDate(string s)
    {
        Date = s;
    }

    public Alarm()
    {
        labels = new string[] { Strings.Notes, Strings.R_Days, Strings.R_Months, Strings.R_Years};
        attributes = new string[] { "", "", "", "" };
    }

    public Alarm(string d, string n)
    {
        SetDate(d);
        note = n;
    }

    public Alarm(string d, string n, int repeat, int by)
    {
        SetDate(d);
        note = n;
        switch (by)
        {
            case 0:
                repeat_days = by;
                break;
            case 1:
                repeat_months = by;
                break;
            case 2:
                repeat_years = by;
                break;
            default:
                break;
        }
    }

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

public class NewEntryList : ICloneable
{
    private List<NewEntry> list; 
    private NewEntryComparer comparer;

    public NewEntryList()
    {
        comparer = new NewEntryComparer();
    }

    public void Remove(NewEntry e)
    {
        list.Remove(e);
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void Add(NewEntry e)
    {
        if (list == null || list.Count == 0)
            list = new List<NewEntry> { e };
        else
        {
            int index = list.BinarySearch(e, comparer);
            if (index < 0)
                list.Insert(~index, e);
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
    private static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    private static string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    public static string GetDay(int i)
    {
        if (i < days.Length)
            return days[i];
        return days[0];
    }

    public static string GetMonth(int i)
    {
        if (i < months.Length)
            return months[i];
        return months[0];
    }

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

    private static int[] SplitTime(string s)
    {
        string[] time_s = s.Split(':');
        int[] time_int = new int[time_s.Length];
        for (int i = 0; i < time_s.Length; i++)
            int.TryParse(time_s[i], out time_int[i]);
        return time_int;
    }

    public static int StringTimeToInt(string s, int mod)
    {
        int[] c_time = SplitTime(s);
        int time_i = c_time[0] * mod + c_time[1];
        return time_i;
    }
}

public class DAY
{
    public NewEntryList guides;
    public List<Alarm> events;

    public DAY()
    {
        guides = new NewEntryList();
        events = new List<Alarm>();
    }

    public void AddGuide(NewEntry n)
    {
        if(!n.filler)
            n.id = n.Date + "_" + guides.Count();
        guides.Add(n);
    }

    public void AddEvent(Alarm n)
    {
        n.id = n.Date + "+" + events.Count;
        events.Add(n);
    }
}