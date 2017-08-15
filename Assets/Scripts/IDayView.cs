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
    }

    protected override void OnSetView()
    {
        isMonday = assignedDate.DayOfWeek == System.DayOfWeek.Monday;
    }

    public void OnClickAlarmIndicator()
    {
        ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
        extras.RequestAlarmPreview(info.events);
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
            res = dataManager.TryGetEntries(_tag, assignedDate.Month > gManager.currentDate.Month + 2);

        if (res.value)
        {
            info = res.info;
        }

        if (!isMonday && assignedDate.Month < gManager.currentDate.Month - 1 && info.guides.Count() == 0)
            if(!calendarController.LegacyButton.activeSelf)
                calendarController.LegacyButton.SetActive(true);
        

        if (assignedDate.Month >= gManager.currentDate.Month && 
            assignedDate.Month % 3 == 0 && 
            TimeConversions.IntInRange(assignedDate.Day, 20, 31))
        {
            Alarm reportAlarm = new Alarm();
            reportAlarm.attributes[0] = gManager.language.ReportAlarmNotes;
            reportAlarm.report = true;
            info.events.Insert(0, reportAlarm);
        }
    }

    public override void RequestLegacyData()
    {
        searchLegacy = true;
        SetView(assignedDate);
        searchLegacy = false;
    }
}
