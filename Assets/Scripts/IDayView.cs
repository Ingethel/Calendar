using UnityEngine;

public class IDayView : IViewManager
{
    public GameObject AlarmIndicatorPanel;
    protected bool isMonday;
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
        isMonday = assignedDate.DayOfWeek == System.DayOfWeek.Monday;
    }

    public void OnClickAlarmIndicator()
    {
        ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
        extras.RequestAlarmPreview(info.Events);
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

        if (!isMonday 
            && (assignedDate.Year <= gManager.currentDate.Year && assignedDate.Month < gManager.currentDate.Month - 1)
            && info.Guides.Count() == 0)
            if(!calendarController.LegacyButton.activeSelf)
                calendarController.LegacyButton.SetActive(true);
        
        if (assignedDate.Month >= gManager.currentDate.Month && 
            assignedDate.Month % 3 == 0 && 
            TimeConversions.IntInRange(assignedDate.Day, 20, 31))
        {
            Alarm reportAlarm = new Alarm();
            reportAlarm.attributes[0] = gManager.language.ReportAlarmNotes;
            reportAlarm.report = true;
            info.Events.Insert(0, reportAlarm);
        }
    }

    public override void RequestLegacyData()
    {
        searchLegacy = true;
        SetView(assignedDate);
        searchLegacy = false;
    }
}
