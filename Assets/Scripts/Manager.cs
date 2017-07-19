using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

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
    
    public GameObject[] viewModes;
    private IViewManager viewManager;
    private GameObject currentView;
    
    private ViewState currentState;
    public GameObject NewEntryView, SearchView;

    private Dictionary<string, NewEntryList> entries;

    void Start()
    {
        currentState = ViewState.ILLEGAL;
        currentDate = DateTime.Now;

        foreach (GameObject o in viewModes)
            o.SetActive(false);

        ThreadReader reader = new ThreadReader();

        NewEntryView.SetActive(false);
        SearchView.SetActive(false);
        
        string filepath = Application.dataPath + @"/Calendar Data/Data/" + currentDate.Year.ToString() + "/" + currentDate.Month.ToString() + "/" + Strings.file;
        entries = reader.Read(filepath);

        if (currentDate.DayOfWeek == System.DayOfWeek.Monday)
            RequestView(currentDate, ViewState.WEEKLY);
        else
            RequestView(currentDate, ViewState.MONTHLY);
    }

    public SearchResult TryGetEntries(string id)
    {
        SearchResult res = new SearchResult();
        NewEntryList list;
        res.value = entries.TryGetValue(id, out list);
        if (res.value)
            res.info = (NewEntryList)list.Clone();
        return res;
    }

    public void ChangeView(int i)
    {
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
        viewManager.SetView(lastGivenDate);
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
        NewEntryList list;
        if (!entries.TryGetValue(tag, out list))
            list = new NewEntryList();

        list.Add(e);

        entries[tag] = list;
        reader.Write(filename, tag, e);
        DateTime date = new DateTime(e.year, e.month, e.day);
        RequestView(date, ViewState.DAILY);
    }

    public void Search()
    {
        SearchView.SetActive(true);
    }
    
    void ChangeState(ViewState state)
    {
        if (currentState != state)
        {
            if (currentView != null)
                currentView.SetActive(false);

            currentState = state;
            currentView = viewModes[(int)state];
            viewManager = currentView.GetComponentInChildren<IViewManager>();
            currentView.SetActive(true);
        }
    }

    public void SearchTerm(string text) { }

}

public class SearchResult
{
    public bool value;
    public NewEntryList info;
}