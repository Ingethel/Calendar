using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayViewManager : IDayView {

    public Text[] headers;
	public InputField[] headerValues;

    public GameObject weeklyButton;

    public string[] GetEmptySlots() {
        List<string>  EmptySlots = new List<string>();
        for (int i = 0; i < info.Events.Count(); i++)
        {
            Event n;
            info.Events.TryGet(i, out n);
            if (n.filler)
            {
                EmptySlots.Add(n.startTime + "-" + n.endTime);
            }
        }
        return EmptySlots.ToArray();
    }

    protected override void Start()
    {
        base.Start();
        weeklyButton.SetActive(false);
    }

    protected override void SetHeader() {
        int dayIndex = (byte)assignedDate.DayOfWeek - 1;
        if (dayIndex < 0) dayIndex = 6;
        header.text = gManager.language.GetDay(dayIndex) + " " + assignedDate.Day.ToString() + "/" + assignedDate.Month.ToString() + "/" + assignedDate.Year.ToString();
    }

    public override void Refresh()
    {
        base.Refresh();
        weeklyButton.GetComponentInChildren<Text>().text = gManager.language.WeeklyButton;
        weeklyButton.SetActive(false);
        headerValues[0].text = "";
        headerValues[1].text = "";
    }

    public override void SetLanguage()
    {
        headers[0].text = gManager.language.OfficerOnDuty;
        headers[1].text = gManager.language.TourGuies;
        headers[2].text = gManager.language.Time;
        headers[3].text = gManager.language.Details;
        SetHeader();
    }

    protected override void OnSetView()
    {
        base.OnSetView();
        RequestData();
        if (isClosed) {
            weeklyButton.SetActive(true);
        }
        else
        {
            DisplayInfo();
        }
        headerValues[0].text = info.GetOfficer();
        headerValues[1].text = info.GetTourGuides();
    }
    
    protected override void AssignInfo(GameObject o, Event n)
    {
        DayGuideView o_view = o.GetComponent<DayGuideView>();
        if (o_view != null)
            o_view.Allocate(n);
    }

    protected override void DisplayInfo()
    {
        if (info == null || info.Events.Count() == 0)
        {
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                AddFiller(setTime[i], setTime[i + 1]);
            }
        }
        else {
                FillEmptySlots();
        }
        base.DisplayInfo();
        if (info.Alarms.Count > 0)
        {
            AlarmIndicatorPanel.SetActive(true);
            flagAlrm = true;
        }
    }

    public void OnClickWeeklyButton()
    {
        calendarController.RequestView(CalendarViewController.State.WEEKLY, assignedDate);
    }

    public void WriteAF()
    {
        dataManager.RequestWriteOfficer(_tag, headerValues[0].text);
    }

    public void WriteGuides()
    {
        dataManager.RequestWriteGuides(_tag, headerValues[1].text);
    }

}
