using UnityEngine;

public class Panel : MonoBehaviour
{

    protected CalendarViewController calendarController;
    protected DataManager dataManager;
    protected ViewController controller;
    protected GameManager gManager;

    protected virtual void Awake()
    {
        calendarController = FindObjectOfType<CalendarViewController>();
        dataManager = FindObjectOfType<DataManager>();
        controller = GetComponentInParent<ViewController>();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (controller)
            controller.CloseView();
        else
            gameObject.SetActive(false);
    }
    
    public void Kill()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    protected virtual void KeybordInputHandler()
    {

    }
}
