public class ExtrasViewController : ViewController {
    
    public enum State
    {
        NEWENTRY,
        SEARCH,
        ALARM,
        VIEWMODE,
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
        {
            handler.PreviewEntry(n);
        }
    }
}
