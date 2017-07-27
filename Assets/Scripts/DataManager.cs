using System.Collections.Generic;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, DAY> entries;
    ThreadReader reader;
    GameManager manager;

    void Awake()
    {
        entries = new Dictionary<string, DAY>();
        reader = new ThreadReader();
    }
    
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public static int GenerateNewIdFor(string s)
    {
        int id = PlayerPrefs.GetInt(s);
        PlayerPrefs.SetInt(s, id + 1);
        return id;
    }

    private void Append(Dictionary<string, DAY> temp)
    {
        foreach(string s in temp.Keys)
        {
            entries[s] = temp[s];
        }
    }

    private string DateToPath(DateTime date) {
        return Application.dataPath + @"/Calendar Data/Data/" + date.Year.ToString() + "/" + date.Month.ToString() + "/" + Strings.file;
    }

    private string TagToPath(string s)
    {
        string[] split = s.Split('.');
        return Application.dataPath + @"/Calendar Data/Data/" + split[2] + "/" + split[1] + "/" + Strings.file;
    }


    public void RequestReadMonth(DateTime date)
    {
        string filepath = DateToPath(date);
        Append(reader.Read(filepath));
    }
    
    public void RequestReadDay(string id)
    {
        string filepath = TagToPath(id);
        DAY list = reader.Read(filepath, id);
        if(list != null)
        {
            entries[id] = list;
        }
    }

    public void RequestWrite(NewEntry e)
    {
        string filename = TagToPath(e.Date);
        DAY day_info;
        if (!entries.TryGetValue(e.Date, out day_info))
            day_info = new DAY();

        day_info.AddGuide(e);

        entries[e.Date] = day_info;
        reader.Write(filename, e);
    }
    
    
    public void RequestWrite<T>(T e) where T : Item
    {
        string filename = TagToPath(e.Date);
        DAY day_info;
        if (!entries.TryGetValue(e.Date, out day_info))
            day_info = new DAY();

        if(e.tag == Strings.NewEntry)
            day_info.AddGuide(e as NewEntry);
        else if (e.tag == Strings.Event)
            day_info.AddEvent(e as Alarm);

        entries[e.Date] = day_info;
        reader.Write(filename, e);
    }


    public void RequestDelete(NewEntry e)
    {
        string filename = TagToPath(e.Date);
        Debug.Log(e.Date);
        DAY day_info;
        if (entries.TryGetValue(e.Date, out day_info))
        {
            day_info.guides.Remove(e);
            entries[e.Date] = day_info;
        }
        reader.DeleteItem(filename, e);
    }

    public SearchResult TryGetEntries(string id)
    {
        SearchResult res = new SearchResult();
        DAY day_info;
        res.value = entries.TryGetValue(id, out day_info);
        if (!res.value)
        {
            RequestReadDay(id);
            res.value = entries.TryGetValue(id, out day_info);
        }
        if (res.value)
            res.info = day_info;
        return res;
    }

    private bool RequestDayView(string[] temp)
    {
        if (temp.Length == 3) {
            int day, month, year;
            int.TryParse(temp[0], out day);
            int.TryParse(temp[1], out month);
            int.TryParse(temp[2], out year);
            if (year > 0)
                if (month > 0 && month <= 12)
                    if (day > 0 && day <= 31)
                    {
                        day = day <= DateTime.DaysInMonth(year, month) ? day : DateTime.DaysInMonth(year, month);

                        CalendarViewController calendarController = FindObjectOfType<CalendarViewController>();
                        calendarController.RequestView(CalendarViewController.State.DAILY, new DateTime(year, month, day));
                        return true;
                    }
        }
        return false;
    }

    public void SearchTerm(string text)
    {
        // command
        if (text.StartsWith("$"))
        {
            string[] split = text.Split(' ');
            manager.Command(split);
        }
        // date
        else if (text.StartsWith("_"))
        {
            string temp = text.Substring(1);
            string[] split = temp.Split('/');
            if (!RequestDayView(split))
            {
                split = temp.Split('.');
                RequestDayView(split);
            }
        }
        // general term
        else
        {
            DAY dayInfo = reader.SearchItem(text);
            if (dayInfo != null && dayInfo.guides.Count() > 0)
            {
                CalendarViewController calendarController = FindObjectOfType<CalendarViewController>();
                calendarController.DisplayResultView(dayInfo, text);
            }
        }
    }
}

public class SearchResult
{
    public bool value;
    public DAY info;
}