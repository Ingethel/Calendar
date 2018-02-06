using UnityEngine;

public class IDayView : IViewManager
{
    public GameObject AlarmIndicatorPanel;
    protected bool isClosed;
    protected bool flagAlrm;
   
    public override void Refresh()
    {
        base.Refresh();
        AlarmIndicatorPanel.SetActive(false);
        flagAlrm = false;
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
        SearchResult res = dataManager.TryGetEntries(_tag);

        if (res.value)
        {
            info = res.info;
        }
    
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
    
}
