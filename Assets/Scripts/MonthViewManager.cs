using System;
using UnityEngine;
using UnityEngine.UI;

public class MonthViewManager : IViewManager {

    public GameObject weekParent;
    public GameObject dayLabelsParent;

    WeekOfMonth[] weeks;
    Text[] dayLabels;

    protected override void Awake()
    {
        base.Awake();
        weeks = GetComponentsInChildren<WeekOfMonth>();
        int i = 0;
        dayLabels = new Text[7];
        foreach (Transform t in dayLabelsParent.transform)
        {
            dayLabels[i] = t.gameObject.GetComponentInChildren<Text>();
            i++;
        }
    }

    protected override void SetHeader() {
        header.text = gManager.language.GetMonth(assignedDate.Month - 1) + " " + assignedDate.Year.ToString();
    }

    public override void SetLanguage()
    {
        for (int i = 0; i < dayLabels.Length; i++)
            dayLabels[i].text = gManager.language.GetDay(i);
        SetHeader();
    }

    public override void Refresh()
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
            int dayofweek = (byte)day.DayOfWeek - 1;
            if (dayofweek < 0) dayofweek = 6;
            weeks[weekCounter].days[dayofweek].SetView(day);
            if (dayofweek == 6) weekCounter++;
        }
        // previous month
        DateTime day_lastMonth;
        if (assignedDate.Month == 1)
            day_lastMonth = new DateTime(assignedDate.Year-1, 12, DateTime.DaysInMonth(assignedDate.Year-1, 12));
        else
            day_lastMonth = new DateTime(assignedDate.Year, assignedDate.Month - 1, DateTime.DaysInMonth(assignedDate.Year, assignedDate.Month-1));
        while (day_lastMonth.DayOfWeek != System.DayOfWeek.Sunday)
        {
            weeks[0].days[(byte)day_lastMonth.DayOfWeek - 1].SetView(day_lastMonth);
            day_lastMonth = day_lastMonth.AddDays(-1);
        }
        // next month
        DateTime day_nextMoth;
        if (assignedDate.Month == 12)
            day_nextMoth = new DateTime(assignedDate.Year + 1, 1, 1);
        else
            day_nextMoth = new DateTime(assignedDate.Year, assignedDate.Month + 1, 1);
        while(day_nextMoth.DayOfWeek != System.DayOfWeek.Monday)
        {
            int dayofweek = (byte)day_nextMoth.DayOfWeek - 1;
            if (dayofweek < 0) dayofweek = 6;

            weeks[weekCounter].days[dayofweek].SetView(day_nextMoth);
            day_nextMoth = day_nextMoth.AddDays(1);
        }
    }

    public override void RequestLegacyData()
    {
        dataManager.RequestReadMonth(assignedDate);
        foreach (WeekOfMonth week in weeks)
            foreach (DayOfMonth day in week.days)
                day.RequestLegacyData();
    }
}
