using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemPanel<T> : Panel where T : Item {

    
    public T item = null;
    public InputFieldObject[] fields;
    public Text title;
    protected EventSystem system;
    protected bool flag = false;

    public GameObject editButtons, newEntryButtons;

    public DateValidator dateValidator;
    public TimeValidator timeValidator;

    protected virtual void Start()
    {
        system = EventSystem.current;
    }

    protected virtual void Update()
    {
        KeybordInputHandler();
    }

    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public override void Close()
    {
        item = null;
        base.Close();
    }

    protected virtual bool CheckSaveEligibility() { return false; }

    public void Save()
    {
        if (CheckSaveEligibility())
        {
            SaveInfo();
            dataManager.RequestWrite(item);
            CalendarRequestOnSave();
            Close();
        }
    }

    public void Delete()
    {
        if (!dataManager)
            dataManager = FindObjectOfType<DataManager>();
        dataManager.RequestDelete(item);
        calendarController.RefreshView();
        Close();
    }
    
    protected virtual void CalendarRequestOnSave() { }

    protected virtual void SaveInfo()
    {
        if (item != null && !item.filler)
            dataManager.RequestDelete(item);
    }

    protected virtual void DisplayInfo() { }

    public void EditEntry()
    {
        foreach (InputFieldObject fieldObj in fields)
            foreach (InputField field in fieldObj.inputs)
                field.interactable = true;

        setTitle();

        editButtons.SetActive(false);
        newEntryButtons.SetActive(true);
    }

    protected virtual void setTitle()
    {
        title.text = "";
    }

    public virtual void PreviewEntry(T n)
    {
        item = n;
        flag = n.filler;

        foreach (InputFieldObject fieldObj in fields)
            foreach (InputField field in fieldObj.inputs)
                field.interactable = flag;
        
        newEntryButtons.SetActive(flag);
        editButtons.SetActive(!flag);

        DisplayInfo();
    }

    protected string TryGetText(InputField a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }
}
