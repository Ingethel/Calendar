using System.Collections;
using System.Collections.Generic;
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

}
