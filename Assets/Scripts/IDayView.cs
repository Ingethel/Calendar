using UnityEngine;

public class IDayView : IViewManager
{
    public GameObject AlarmIndicatorPanel;
    protected bool isClosed;
    protected bool flagAlrm;
    protected bool searchLegacy;

    protected virtual void Start ()
    {
        gManager.PrintMode += PrintMode;
    }

    public void PrintMode()
    {
        if (flagAlrm)
            AlarmIndicatorPanel.SetActive(!AlarmIndicatorPanel.activeSelf);
    }

    public override void Refresh()
    {
        base.Refresh();
        AlarmIndicatorPanel.SetActive(false);
        flagAlrm = false;
        calendarController.LegacyButton.SetActive(false);
    }

    protected override void OnSetView()
    {
        isClosed = SettingsManager.GetTimetable(assignedDate.DayOfWeek.ToString()) == "0";
    }

    public void OnClickAlarmIndicator()
    {
        ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
        extras.RequestAlarmPreview(info.Alarms);
    }

    protected override void SetTag()
    {
        _tag = TimeConversions.DateTimeToString(assignedDate);
    }

    protected override void RequestData()
    {
        SearchResult res;

        if (searchLegacy)
            res = dataManager.TryGetEntries(_tag, true);
        else
            res = dataManager.TryGetEntries(_tag, assignedDate.Year * 12 + assignedDate.Month >= gManager.currentDate.Year * 12 + gManager.currentDate.Month - 1);

        if (res.value)
        {
            info = res.info;
        }

        if (!isClosed 
            && (assignedDate.Year <= gManager.currentDate.Year && assignedDate.Month < gManager.currentDate.Month - 1)
            && info.Events.Count() == 0)
            if(!calendarController.LegacyButton.activeSelf)
                calendarController.LegacyButton.SetActive(true);
        
        if (assignedDate.Month >= gManager.currentDate.Month && 
            assignedDate.Month % 3 == 0 && 
            TimeConversions.IntInRange(assignedDate.Day, 20, 31))
        {
            Alarm reportAlarm = new Alarm();
            reportAlarm.XMLToObject(gManager.language.ReportAlarmNotes);
            reportAlarm.report = true;
            info.AddAlarm(reportAlarm);
        }
    }

    public override void RequestLegacyData()
    {
        searchLegacy = true;
        SetView(assignedDate);
        searchLegacy = false;
    }
    
}
