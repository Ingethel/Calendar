using System.Collections;
using System.Collections.Generic;
using System;

public class MonthManager : IViewManager {
    private string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

    public WeekOfMonth[] weeks;
    
	void Start () {
        weeks = GetComponentsInChildren<WeekOfMonth>();
    }

    protected override void SetHeader() {
        header.text = months[assignedDate.Month - 1] + " " + assignedDate.Year.ToString();
    }

    protected override void OnSetView() {
        foreach (WeekOfMonth week in weeks)
            foreach (DayOfMonth day in week.days)
                day.Reset();
        int maxDays = DateTime.DaysInMonth(assignedDate.Year, assignedDate.Month);
        int weekCounter = 0;
        for (int i = 0; i < maxDays; i++) {
            DateTime day = new DateTime(assignedDate.Year, assignedDate.Month, i+1);
            byte dayofweek = (byte)day.DayOfWeek;
            weeks[weekCounter].days[dayofweek].SetView(day);
            if (dayofweek == 0) weekCounter++;
        }
    }
    
}
