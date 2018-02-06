using System.Collections.Generic;
using UnityEngine;

public class ExtrasViewController : ViewController {
    
    public enum State
    {
        NEWENTRY,
        ALARM,
        ALARMPREVIEW,
        VIEWMODE,
        SEARCH,
        HELP,
        ILLEGAL
    };

    CalendarViewController calendarController;

    protected override void Start()
    {
        base.Start();
        calendarController = FindObjectOfType<CalendarViewController>();
    }

    private void HideIfClickedOutside()
    {
        if (Input.GetMouseButton(0) && currentView.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                currentView.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            CloseView();
        }
    }

    public override void CloseView()
    {
        RequestView(State.ILLEGAL);
    }

    public override void NotifyIllegal()
    {
        CurrentViewIndex = (int)State.ILLEGAL;
        calendarController.SetAsBackground(false);
    }

    public void RequestView(State s)
    {
        if(s == State.ILLEGAL)
            calendarController.SetAsBackground(false);
        else
            calendarController.SetAsBackground(true);

        if (s == State.NEWENTRY)
            RequestEntryPreview(new Event(), s);
        else if (s == State.ALARM)
            RequestEntryPreview(new Alarm(), s);
        else
            ChangeView((int)s);

    }

    public void RequestEntryPreview<T>(T n, State s) where T : Item{
        ChangeView((int)s);
        calendarController.SetAsBackground(true);
        ItemPanel<T> handler = currentView.GetComponent<ItemPanel<T>>();
        if (handler)
        {
            handler.PreviewEntry(n);
        }
    }

    public void RequestAlarmPreview(List<Alarm> alarms)
    {
        ChangeView((int)State.ALARMPREVIEW);
        calendarController.SetAsBackground(true);
        AlarmPreviewerHandler handler = currentView.GetComponent<AlarmPreviewerHandler>();
        if (handler)
        {
            handler.SetView(alarms);
        }
    }

    public override void SetLanguage()
    {
        if (currentView != null)
        {
            Panel p = currentView.GetComponent<Panel>();
            if (p)
                p.SetLanguage();
        }
    }

}
