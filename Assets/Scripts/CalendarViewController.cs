using System;
using System.Globalization;

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

    protected override void Start()
    {
        base.Start();
        DataManager data = FindObjectOfType<DataManager>();

        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, -1));
        data.RequestReadMonth(DateTime.Now);
        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, 1));
        data.RequestReadMonth(calendar.AddMonths(DateTime.Now, 2));

        RequestTodaysView();
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
        viewManager = currentView.GetComponentInChildren<IViewManager>();
        viewManager.SetView(lastGivenDate);
    }

    public void DisplayResultView(DAY day, string result)
    {
        ChangeView((int)State.SEARCH);
        SearchViewManager s_viewManager = currentView.GetComponentInChildren<SearchViewManager>();
        s_viewManager.SetView(day, result);
    }
}
