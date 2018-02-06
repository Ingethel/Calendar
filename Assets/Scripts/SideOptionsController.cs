public class SideOptionsController : ViewController {
    
    public enum State
    {
        COMPACT,
        EXTENDED,
        ILLEGAL
    };

    ExtrasViewController optionsController;

    protected override void Start()
    {
        base.Start();
        optionsController = FindObjectOfType<ExtrasViewController>();
        OpenCompact();
    }

    public void AlarmPressed()
    {
        optionsController.RequestView(ExtrasViewController.State.ALARM);
    }

    public void NewEntryPressed()
    {
        optionsController.RequestView(ExtrasViewController.State.NEWENTRY);
    }

    public void SearchPressed()
    {
        optionsController.RequestView(ExtrasViewController.State.SEARCH);
    }

    public void ViewPressed()
    {
        optionsController.RequestView(ExtrasViewController.State.VIEWMODE);
    }

 /*   public void ViewChanged(int i)
    {
        optionsController.RequestView(ExtrasViewController.State.ILLEGAL);
        calendarController.RequestView((CalendarViewController.State)specialSnowflake.value);
    }
   */ 
    public void CloseCurrentView()
    {
        optionsController.RequestView(ExtrasViewController.State.ILLEGAL);
    }
    
    public void OpenCompact()
    {
        ChangeView((int)State.COMPACT);
    }

    public void OpenExtended()
    {
        ChangeView((int)State.EXTENDED);
    }

    public override void CloseView()
    {
        ChangeView((int)State.ILLEGAL);
    }
    
    public void PrintMode()
    {
        currentView.SetActive(!currentView.activeSelf);
    }
}
