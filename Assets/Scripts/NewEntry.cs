using System.Collections.Generic;
using System;

public class NewEntry
{

    public string[] labels = {Strings.StartTime, Strings.EndTime, Strings.NameOfTeam, Strings.NumberOfPeople,Strings.PersonInCharge, Strings.Telephone, Strings.ConfirmationDate, Strings.Guide, Strings.Notes };

    public string[] attributes = { "", "", "", "", "", "", "", "", "" };
    public int day, month, year;
    public bool filler;

    public NewEntry()
    {
        filler = true;
    }

    public NewEntry(string[] list)
    {
        attributes = list;
        filler = false;
    }

    public int GetStartTime()
    {
        return TimeConversions.StringTimeToInt(attributes[0]);
    }

    public int GetEndTime()
    {
        return TimeConversions.StringTimeToInt(attributes[1]);
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
    public static string IntTimeToString(int time)
    {
        string minutes = (time % 100).ToString();
        string hours = (time / 100).ToString();
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

    public static int StringTimeToInt(string s)
    {
        int[] c_time = SplitTime(s);
        int time_i = c_time[0] * 60 + c_time[1];
        return time_i;
    }
}