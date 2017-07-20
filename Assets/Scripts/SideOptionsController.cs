using UnityEngine;

public class SideOptionsController : MonoBehaviour {

    public GameObject[] SideOptions;
    ExtrasViewController optionsController;
    CalendarViewController calendarController;

    private void Start()
    {
        optionsController = FindObjectOfType<ExtrasViewController>();
        calendarController = FindObjectOfType<CalendarViewController>();
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

    public void ViewChanged(int i)
    {
        optionsController.RequestView(ExtrasViewController.State.ILLEGAL);
        calendarController.RequestView((CalendarViewController.State)i);
    }
    
    public void CloseCurrentView()
    {
        optionsController.RequestView(ExtrasViewController.State.ILLEGAL);
    }

    public void OpenCompact()
    {
        SideOptions[1].SetActive(false);
        SideOptions[0].SetActive(true);
    }

    public void OpenExtended()
    {
        SideOptions[0].SetActive(false);
        SideOptions[1].SetActive(true);
    }
}
