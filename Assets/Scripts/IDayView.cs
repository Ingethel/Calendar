using UnityEngine;

public class IDayView : IViewManager
{
    public GameObject AlarmIndicatorPanel;
    protected bool isMonday;
    protected bool flagAlrm;

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
        base.RequestData();
        if (assignedDate.Month % 3 == 0 && TimeConversions.IntInRange(assignedDate.Day, 20, 30))
        {
            Alarm reportAlarm = new Alarm();
            reportAlarm.attributes[0] = "Prepare Semester Report";
            reportAlarm.report = true;
            info.events.Insert(0, reportAlarm);
        }
    }
}
