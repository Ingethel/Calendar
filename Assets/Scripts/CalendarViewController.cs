using System;
using System.Collections;
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
        gManager.printMode += PrintMode;

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
        SearchResult search = data.TryGetEntries(currentDate.Day.ToString() + "." + currentDate.Month.ToString() + "." + currentDate.Year.ToString(), false);
        if (search.value)
            if (search.info.events.Count > 0)
            {
                ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
                extras.RequestAlarmPreview(search.info.events);
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
        viewManager = currentView.GetComponentInChildren<IViewManager>();
        viewManager.SetView(lastGivenDate);
    }

    public void DisplayResultView(DAY day, string result)
    {
        ChangeView((int)State.SEARCH);
        SearchViewManager s_viewManager = currentView.GetComponentInChildren<SearchViewManager>();
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
