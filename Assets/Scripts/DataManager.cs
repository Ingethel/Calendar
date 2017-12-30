using System.Collections.Generic;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, DAY> entries;
    DataReader reader;
    GameManager manager;

    void Awake()
    {
        entries = new Dictionary<string, DAY>();
        reader = new DataReader();
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
            entries[s] = temp[s];
    }

    private string DateToPath(DateTime date) {
        if(date.Year * 12 + date.Month >= DateTime.Now.Year * 12 + DateTime.Now.Month-1)
            return Application.dataPath + @"/Calendar Data/Data/" + date.Year.ToString() + "/" + date.Month.ToString() + "/" + DataStrings.file;
        else
            return Application.dataPath + @"/Calendar Data/Legacy/" + date.Year.ToString() + "/" + date.Month.ToString() + "/" + DataStrings.file;
    }

    private string TagToPath(string s)
    {
        string[] split = s.Split('.');
        int month, year;
        int.TryParse(split[1], out month);
        int.TryParse(split[2], out year);
        if (year * 12 + month >= DateTime.Now.Year * 12 + DateTime.Now.Month - 1)
            return Application.dataPath + @"/Calendar Data/Data/" + split[2] + "/" + split[1] + "/" + DataStrings.file;
        else
            return Application.dataPath + @"/Calendar Data/Legacy/" + split[2] + "/" + split[1] + "/" + DataStrings.file;
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
            entries[id] = list;
        else
            entries[id] = new DAY();
    }
    
    public void RequestWrite<T>(T e) where T : Item
    {
        string filename = TagToPath(e.Date);
        DAY day_info;
        if (!entries.TryGetValue(e.Date, out day_info))
            day_info = new DAY();

        if(e.tag == DataStrings.NewEntry)
            day_info.AddGuide(e as NewEntry);
        else if (e.tag == DataStrings.Event)
            day_info.AddEvent(e as Alarm);

        entries[e.Date] = day_info;
        reader.Write(filename, e);
        manager.ReloadScene();
    }

    public void RequestWriteOfficer(string id, string officer)
    {
        string filename = TagToPath(id);
        DAY day_info;
        if (!entries.TryGetValue(id, out day_info))
            day_info = new DAY();

        day_info.SetOfficer(officer);

        entries[id] = day_info;
        reader.Write(filename, id, officer);
        manager.ReloadScene();
    }

    public void RequestDelete<T>(T e) where T : Item
    {
        string filename = TagToPath(e.Date);
        DAY day_info;
        if (entries.TryGetValue(e.Date, out day_info))
        {
            if (e.tag == DataStrings.NewEntry)
                day_info.Guides.Remove(e as NewEntry);
            else if (e.tag == DataStrings.Event)
                day_info.Events.Remove(e as Alarm);

            entries[e.Date] = day_info;
        }
        reader.DeleteItem(filename, e);
        manager.ReloadScene();
    }

    public SearchResult TryGetEntries(string id, bool searchFileData)
    {
        SearchResult res = new SearchResult();
        DAY day_info;
        res.value = entries.TryGetValue(id, out day_info);
        if (!res.value && searchFileData)
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
            DAY dayInfo = reader.SearchItem(text, manager.DATA_FOLDER);
            CalendarViewController calendarController = FindObjectOfType<CalendarViewController>();
            calendarController.DisplayResultView(dayInfo, text);
        }
    }

    public void SearchLegacy(string term)
    {
        DAY dayInfo = reader.SearchItem(term, manager.LEGACY_FOLDER);
        CalendarViewController calendarController = FindObjectOfType<CalendarViewController>();
        calendarController.DisplayResultView(dayInfo, term);
    }
}

public class SearchResult
{
    public bool value;
    public DAY info;
}