using System.Collections.Generic;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    DataReader reader;
    GameManager manager;

    void Awake()
    {
        reader = new DataReader();
    }
    
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
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
    
    public DAY RequestReadDay(string id)
    {
        string filepath = TagToPath(id);
        DAY list = reader.Read(filepath, id);
        if(list != null)
            return list;
        else
            return new DAY();
    }
    
    public void RequestWrite<T>(T e) where T : Item
    {
        string filename = TagToPath(e.Date);
        e.id = SettingsManager.GenerateNewId();
        reader.Write(filename, e);
    }

    public void RequestWriteAttribute(string id, string att, string val)
    {
        string filename = TagToPath(id);
        reader.WriteAttribute(filename, id, att, val);
    }
    
    public void RequestDelete<T>(T e) where T : Item
    {
        string filename = TagToPath(e.Date);
        reader.DeleteItem(filename, e);
    }

    public SearchResult TryGetEntries(string id)
    {
        SearchResult res = new SearchResult();
        res.info = RequestReadDay(id);
        res.value = res.info != null;
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