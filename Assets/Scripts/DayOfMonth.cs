using UnityEngine;
using UnityEngine.UI;

public class DayOfMonth : IDayView{

    public GameObject DateIndicatorPanel;
    Selectable selectable;
    public Color filled;
    
    public override void Refresh()
    {
        base.Refresh();
        DateIndicatorPanel.SetActive(false);
        if (selectable == null)
            selectable = GetComponent<Selectable>();
        selectable.interactable = false;
    }

    public void Reset()
    {
        Refresh();
    }

    protected override void SetHeader()
    {
        header.text = assignedDate.Day.ToString();
    }

    protected override void DisplayInfo()
    {
        DateIndicatorPanel.SetActive(true);
        selectable.interactable = true;
        if (isMonday)
        {
            Image img = GetComponent<Image>();
            if (img)
                img.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
        }
        else
        {
            if (info.guides.Count() < setTime.Length - 1)
                FillEmptySlots();
            
            base.DisplayInfo();
        }
        if (info.events.Count > 0)
        {
            AlarmIndicatorPanel.SetActive(true);
            flagAlrm = true;
        }
    }

    protected override void OnSetView() {
        base.OnSetView();
        if (!isMonday)
            RequestData();
        DisplayInfo();
    }
    
    public override void RequestView()
    {
        if (isMonday)
            calendarController.RequestView(CalendarViewController.State.WEEKLY, assignedDate);
        else
            calendarController.RequestView(CalendarViewController.State.DAILY, assignedDate);
    }
    
    protected override void AssignInfo(GameObject o, NewEntry n)
    {
        if (!n.filler) {
            Image img = o.GetComponent<Image>();
            if (img)
                img.color = filled;
        }
    }

    public override void RequestLegacyData()
    {
        if(DateIndicatorPanel.activeSelf)
            base.RequestLegacyData();
    }
}
