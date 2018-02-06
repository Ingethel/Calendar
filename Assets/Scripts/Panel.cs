using UnityEngine;
using UnityEngine.UI;

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
        gManager = FindObjectOfType<GameManager>();
    }

    protected virtual void Start() { }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        SetLanguage();
    }

    public virtual void Close()
    {
        if (controller)
            controller.NotifyIllegal();

        gameObject.SetActive(false);
    }
    
    public virtual void Kill()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    protected virtual void KeybordInputHandler(){}
    
    public virtual void Refresh(){}

    public virtual void SetLanguage(){}
}

[System.Serializable]
public class InputFieldObject
{
    public InputField[] inputs;
    public Text label;
}
