using System.Collections.Generic;

public class NewEntry
{

    public string[] labels = {Strings.StartTime, Strings.EndTime, Strings.NameOfTeam, Strings.NumberOfPeople,Strings.PersonInCharge, Strings.Telephone, Strings.ConfirmationDate, Strings.Guide, Strings.Notes };

    public string[] attributes = { "", "", "", "", "", "", "", "", "" };
    public int day, month, year;
    public NewEntry() { }

    public NewEntry(string[] list)
    {
        attributes = list;
    }

    private int[] SplitTime(string s)
    {
        string[] time_s = s.Split(':');
        int[] time_int = new int[time_s.Length];
        for (int i = 0; i < time_s.Length; i++)
            int.TryParse(time_s[i], out time_int[i]);
        return time_int;
    }

    private int GetIntTime(string s)
    {
        string[] time_s = s.Split(':');
        string time = "";
        for (int i = 0; i < time_s.Length; i++)
            time += time_s[i];
        int time_i = 0;
        int.TryParse(time, out time_i);
        return time_i;
    }

    public static string IntTimeToString(int time)
    {
        string minutes = (time % 100).ToString();
        string hours = (time / 100).ToString();
        while(hours.Length < 2)
        {
            hours = "0" + hours;
        }
        while (minutes.Length < 2)
        {
            minutes = "0" + minutes;
        }
        return hours + ":" + minutes;
    }

    public int GetStartTime()
    {
        return GetIntTime(attributes[0]);
    }

    public int GetEndTime()
    {
        return GetIntTime(attributes[1]);
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

public class NewEntryList
{
    private List<NewEntry> list; 
    private NewEntryComparer comparer;

    public NewEntryList()
    {
        comparer = new NewEntryComparer();
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
