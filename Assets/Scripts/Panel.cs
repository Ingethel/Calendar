using UnityEngine;

public class Panel : MonoBehaviour {

    protected CalendarViewController calendarController;
    protected DataManager dataManager;

    protected virtual void Awake()
    {
        calendarController = FindObjectOfType<CalendarViewController>();
        dataManager = FindObjectOfType<DataManager>();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
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
