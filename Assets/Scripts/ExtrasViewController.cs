using System.Collections.Generic;

public class ExtrasViewController : ViewController {
    
    public enum State
    {
        NEWENTRY,
        SEARCH,
        ALARM,
        VIEWMODE,
        ALARMPREVIEW,
        ILLEGAL
    };

    public void RequestView(State s)
    {
        if (s == State.NEWENTRY)
            RequestEntryPreview(new NewEntry());
        ChangeView((int)s);
    }

    public void RequestEntryPreview(NewEntry n) {
        ChangeView((int)State.NEWENTRY);
        NewEntryPanelHandler handler = currentView.GetComponent<NewEntryPanelHandler>();
        if (handler)
            handler.PreviewEntry(n);
    }

    public void RequestAlarmPreview(List<Alarm> alarms)
    {
        ChangeView((int)State.ALARMPREVIEW);
        AlarmPreviewerHandler handler = currentView.GetComponent<AlarmPreviewerHandler>();
        if (handler)
            handler.SetView(alarms);
    }
}
