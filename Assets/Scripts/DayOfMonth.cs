using UnityEngine;
using UnityEngine.UI;

public class DayOfMonth : IViewManager{

    public GameObject DateIndicatorPanel;
    public GameObject AlarmIndicatorPanel;
    Selectable selectable;
    public Color filled;
    bool isMonday;
    bool flagAlrm;

    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
        gManager.printMode += PrintMode;
    }

    public void PrintMode()
    {
        if (flagAlrm)
            AlarmIndicatorPanel.SetActive(!AlarmIndicatorPanel.activeSelf);
    }

    public override void Refresh()
    {
        base.Refresh();
        DateIndicatorPanel.SetActive(false);
        AlarmIndicatorPanel.SetActive(false);
        flagAlrm = false;
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
            {
                img.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
            }
        }
        else
        {
            if (info.guides.Count() < setTime.Length - 1)
            {
                FillEmptySlots();
            }

            base.DisplayInfo();
        }
        if (info.events.Count > 0)
        {
            AlarmIndicatorPanel.SetActive(true);
            flagAlrm = true;
        }
    }

    protected override void OnSetView() {
        isMonday = assignedDate.DayOfWeek == System.DayOfWeek.Monday;
        if (!isMonday)
        {
            RequestData();
        }
        DisplayInfo();
    }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }

    public override void RequestView()
    {
        if (isMonday) {
            calendarController.RequestView(CalendarViewController.State.WEEKLY, assignedDate);
        }
        else
        {
            calendarController.RequestView(CalendarViewController.State.DAILY, assignedDate);
        }
    }
    
    protected override void AssignInfo(GameObject o, NewEntry n)
    {
        if (!n.filler) {
            Image img = o.GetComponent<Image>();
            if (img)
            {
                img.color = filled;
            }
        }
        
    }
    
}
