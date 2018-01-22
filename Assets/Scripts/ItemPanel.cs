using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemPanel<T> : Panel where T : Item {

    public T item = null;
    protected bool dublicate = false;
    public InputFieldObject[] fields;
    public Text title;
    protected EventSystem system;
    protected bool flag = false;

    public GameObject editButtons, newEntryButtons;

    public DateValidator dateValidator;
    public TimeValidator timeValidator;

    public List<string> attributes;
    public GameObject attributeList;
    public GameObject attributeElement;

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
        if (!item.filler && !dublicate)
            dataManager.RequestDelete(item);
    }

    protected virtual void DisplayInfo() { }

    public virtual void EditEntry()
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

    public void Dublicate()
    {
        dublicate = true;
        Save();
        dublicate = false;
    }

    protected string TryGetText(InputField a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }

    public void GetAttributes()
    {
        if (attributeList != null)
            foreach (Transform t in attributeList.transform)
                Destroy(t.gameObject);
        for(int i = 0; i < attributes.Count; i++)
            SpawnAttribute(attributes[i], item.attributes[i]);
    }

    public void SpawnAttribute(string label, string value)
    {
        GameObject o = Instantiate(attributeElement);
        o.transform.SetParent(attributeList.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponent<AttributeElement>().Assign(label, value);
    }

}
