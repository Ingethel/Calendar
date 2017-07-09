using UnityEngine;

public class DayOfMonth : IViewManager {

    public GameObject DayPanel;
    HoverPanelAnimation hoverScript;
    
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

    public override void RequestView()
    {
        manager.RequestView(assignedDate, Manager.ViewState.DAILY);
    }
}
