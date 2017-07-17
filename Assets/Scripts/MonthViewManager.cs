using System;

public class MonthViewManager : IViewManager {
    
    public WeekOfMonth[] weeks;
    
	void Start () {
        weeks = GetComponentsInChildren<WeekOfMonth>();
    }

    protected override void SetHeader() {
        header.text = TimeConversions.GetMonth(assignedDate.Month - 1) + " " + assignedDate.Year.ToString();
    }

    protected override void Refresh()
    {
        foreach (WeekOfMonth w in weeks)
            foreach (DayOfMonth d in w.days)
                d.Reset();
    }

    protected override void OnSetView() {
        // current month
        int maxDays = DateTime.DaysInMonth(assignedDate.Year, assignedDate.Month);
        int weekCounter = 0;
        for (int i = 0; i < maxDays; i++) {
            DateTime day = new DateTime(assignedDate.Year, assignedDate.Month, i+1);
            byte dayofweek = (byte)day.DayOfWeek;
            weeks[weekCounter].days[dayofweek].SetView(day);
            if (dayofweek == 0) weekCounter++;
        }
        // previous month
        DateTime day_lastMonth = new DateTime(assignedDate.Year, assignedDate.Month - 1, DateTime.DaysInMonth(assignedDate.Year, assignedDate.Month-1));
        while (day_lastMonth.DayOfWeek != System.DayOfWeek.Sunday)
        {
            weeks[0].days[(byte)day_lastMonth.DayOfWeek].SetView(day_lastMonth);
            day_lastMonth = day_lastMonth.AddDays(-1);
        }
        // next month
        DateTime day_nextMoth = new DateTime(assignedDate.Year, assignedDate.Month + 1, 1);
        while(day_nextMoth.DayOfWeek != System.DayOfWeek.Monday)
        {
            weeks[weekCounter].days[(byte)day_nextMoth.DayOfWeek].SetView(day_nextMoth);
            day_nextMoth = day_nextMoth.AddDays(1);
        }
    }

}
