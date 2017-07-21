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
        ChangeView((int)s);
    }

    public void RequestEntryPreview(NewEntry n) {
        ChangeView((int)State.NEWENTRY);
        NewEntryPanelHandler handler = currentView.GetComponent<NewEntryPanelHandler>();
        if (handler)
        {
            handler.PreviouEntry(n);
        }
    }
}
