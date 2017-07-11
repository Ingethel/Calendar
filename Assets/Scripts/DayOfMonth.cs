using UnityEngine;
using System.Collections.Generic;

public class DayOfMonth : IViewManager {

    public GameObject DayPanel;
    HoverPanelAnimation hoverScript;
    private List<NewEntry> info;

    private void Start()
    {
        hoverScript = GetComponent<HoverPanelAnimation>();
        Reset();
    }

    public void Reset()
    {
        DayPanel.SetActive(false);
        hoverScript.hoverActive = false;
    }

    protected override void SetHeader()
    {
        header.text = assignedDate.Day.ToString();
    }

    protected override void OnSetView() {
        DayPanel.SetActive(true);
        hoverScript.hoverActive = true;
    }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }

    public override void RequestView()
    {
        manager.RequestView(assignedDate, Manager.ViewState.DAILY);
    }

    protected override void RequestData() {
        SearchResult res = manager.GetEntries(_tag);
        if (res.value)
        {
            info = res.info;
        }
    }

}
