using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class CalendarViewController : ViewController
{

	public enum State
    {
        MONTHLY,
        WEEKLY,
        DAILY,
        ILLEGAL
    };

    int ViewMode;

    public static Calendar calendar = CultureInfo.InvariantCulture.Calendar;
    private DateTime lastGivenDate;

    protected override void Start()
    {
        base.Start();
        DataManager data = FindObjectOfType<DataManager>();
        data.RequestReadMonth(DateTime.Now);
        RequestTodaysView();
    }

    public void ChangeDay(int i)
    {
        switch (ViewMode)
        {
            case (int)State.DAILY:
                RequestView(calendar.AddDays(lastGivenDate, i));
                break;
            case (int)State.WEEKLY:
                RequestView(calendar.AddWeeks(lastGivenDate, i));
                break;
            case (int)State.MONTHLY:
                RequestView(calendar.AddMonths(lastGivenDate, i));
                break;
            case (int)State.ILLEGAL:
            default:
                break;
        }
    }

    public void RequestTodaysView()
    {
        DateTime currentDate = DateTime.Now;
        if (currentDate.DayOfWeek == System.DayOfWeek.Monday)
            RequestView(State.WEEKLY, currentDate);
        else

            RequestView(State.MONTHLY, currentDate);
    }

    public void RequestView(State e, DateTime date)
    {
        ViewMode = (int)e;
        ChangeView(ViewMode);
        viewManager = currentView.GetComponentInChildren<IViewManager>();
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
    }

    public void RefreshView()
    {
        viewManager.SetView(lastGivenDate);
    }
}
