﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CalendarViewController : ViewController
{

	public enum State
    {
        MONTHLY,
        WEEKLY,
        DAILY,
        SEARCH,
        REPORT,
        ILLEGAL
    };
    
    public static Calendar calendar = CultureInfo.InvariantCulture.Calendar;
    private DateTime lastGivenDate;
    bool printflag;

    public GameObject background;
    DataManager data;

    protected override void Start()
    {
        base.Start();
        data = FindObjectOfType<DataManager>();
        printflag = false;
        gManager.PrintMode += PrintMode;

        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, -1));
        data.RequestReadMonth(DateTime.Now);
        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, 1));
        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, 2));

        StartCoroutine(RequestTodaysView());
    }

    public void ChangeDay(int i)
    {

        switch (currentViewIndex)
        {
            case (int)State.DAILY:
                lastGivenDate = (calendar.AddDays(lastGivenDate, i));
                break;
            case (int)State.WEEKLY:
                lastGivenDate = (calendar.AddWeeks(lastGivenDate, i));
                break;
            case (int)State.MONTHLY:
                lastGivenDate = (calendar.AddMonths(lastGivenDate, i));
                break;
            case (int)State.ILLEGAL:
            default:
                break;
        }
        RequestView(lastGivenDate);
    } 
    
    IEnumerator RequestTodaysView()
    {
        yield return 0;
        DateTime currentDate = DateTime.Now;
        if (currentDate.DayOfWeek == System.DayOfWeek.Monday)
            RequestView(State.WEEKLY, currentDate);
        else
            RequestView(State.MONTHLY, currentDate);
        yield return 0;
        List<Alarm> eventsThisWeek = new List<Alarm>();
        DateTime temp = currentDate;
        for(int i = 0; i < 6; i++)
        {
            temp = temp.AddDays(i);
            SearchResult search = data.TryGetEntries(temp.Day.ToString() + "." + temp.Month.ToString() + "." + temp.Year.ToString(), false);
            if (search.value)
                if (search.info.events.Count > 0)
                    eventsThisWeek.AddRange(search.info.events);
        }
        if (eventsThisWeek.Count > 0)
        {
            ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
            if(extras)
                extras.RequestAlarmPreview(eventsThisWeek);
        }
    }

    public void RequestView(State e, DateTime date)
    {
        ChangeView((int)e);
        lastGivenDate = date;
        RefreshView();
    }

    public void RequestView(DateTime date)
    {
        lastGivenDate = date;
        RefreshView();
    }

    public void RequestView(State e)
    {
        ChangeView((int)e);
        RefreshView();
    }

    public void RefreshView()
    {
        SetAsBackground(false);
        viewManager = currentView.GetComponent<IViewManager>();
        if(viewManager)
            viewManager.SetView(lastGivenDate);
    }
    
    public override void SetLanguage()
    {
        if(viewManager)
            viewManager.SetLanguage();
    }

    public void DisplayResultView(DAY day, string result)
    {
        ChangeView((int)State.SEARCH);
        SearchViewManager s_viewManager = currentView.GetComponent<SearchViewManager>();
        s_viewManager.SetView(day, result);
    }

    public void PrintMode()
    {
        background.SetActive(printflag);
        SetActiveRaycast(printflag);

        printflag = !printflag;
        lockedAccess = printflag;
    }

}
