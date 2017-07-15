using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public enum ViewState
    {
        MONTHLY,
        WEEKLY,
        DAILY,
        ILLEGAL
    };

    public DateTime currentDate
    {
        private set; get;
    }
    private DateTime lastGivenDate;

    public static Calendar calendar = CultureInfo.InvariantCulture.Calendar;
    public Dropdown view;
    private MonthManager monthViewManager;
    private DayManager dayViewManager;

    public Canvas MonthlyView, WeeklyView, DailyView;
    private Canvas currentView;
    private ViewState currentState;
    public GameObject NewEntryView, SearchView;
    
    private Dictionary<string, List<NewEntry>> entries;

    void Start ()
    {
        currentState = ViewState.ILLEGAL;
        currentDate = DateTime.Now;
        ThreadReader reader = new ThreadReader();
        monthViewManager = FindObjectOfType<MonthManager>();
        dayViewManager = FindObjectOfType<DayManager>();
        NewEntryView.SetActive(false);
        SearchView.SetActive(false);

        MonthlyView.enabled = false;
        DailyView.enabled = false;

        string filepath = Application.dataPath + @"/Calendar Data/Data/" + currentDate.Year.ToString() + "/" + currentDate.Month.ToString() + "/"+Strings.file;
        entries = reader.Read(filepath);
        
        SetView(currentDate);
    }

    public SearchResult GetEntries(string id)
    {
        SearchResult res = new SearchResult();
        res.value = entries.TryGetValue(id, out res.info);
        return res;
    }

    public void ChangeView(int i)
    {
        if(i != 0)
            switch (view.value)
            {
                case 2:
                    lastGivenDate = calendar.AddDays(lastGivenDate, i);
                    break;
                case 1:
                    lastGivenDate = calendar.AddWeeks(lastGivenDate, i);
                    break;
                case 0:
                    lastGivenDate = calendar.AddMonths(lastGivenDate, i);
                    break;
            }
        SetView(lastGivenDate);
    }

    public void RequestView(DateTime date, ViewState state)
    {
        view.value = (int)state;
        SetView(date);
    }

    private void SetView(DateTime date)
    {
        lastGivenDate = date;
        ChangeState((ViewState)view.value);
        switch (view.value)
        {
            case 2:
                dayViewManager.SetView(lastGivenDate);
                break;
            case 1:
                //weekViewManager.SetView(lastGivenDate);
                break;
            case 0:
            default:
                monthViewManager.SetView(lastGivenDate);
                break;
        }
    }

    public void NewEntry()
    {
        NewEntryView.SetActive(!NewEntryView.activeSelf);
    }

    public void SaveEntry(NewEntry e)
    {
        string filename = e.year + "/" + e.month;
        string tag = e.day + "." + e.month + "." + e.year;
        ThreadReader reader = new ThreadReader();
        List<NewEntry> list;
        if (!entries.TryGetValue(tag, out list))
            list = new List<NewEntry>();
        list.Add(e);
        entries[tag] = list;
        reader.Write(filename, tag, list);
        NewEntry();
        DateTime date = new DateTime(e.year, e.month, e.day);
        RequestView(date, ViewState.DAILY);
    }

    public void Search()
    {
        SearchView.SetActive(true);
    }

    void ChangeState(ViewState state)
    {
        if (currentState != state) {
            if (currentView != null)
                currentView.enabled = false;

            currentState = state;

            switch (currentState)
            {
                case ViewState.MONTHLY:
                    currentView = MonthlyView;
                    break;
                case ViewState.WEEKLY:
                    currentView = WeeklyView;
                    break;
                case ViewState.DAILY:
                    currentView = DailyView;
                    break;
            }

            currentView.enabled = true;
        }
    }

}

public class SearchResult {
    public bool value;
    public List<NewEntry> info;
}