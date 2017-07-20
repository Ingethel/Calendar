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
}
