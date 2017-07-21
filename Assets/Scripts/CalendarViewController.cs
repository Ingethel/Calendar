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
}
