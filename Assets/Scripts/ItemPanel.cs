using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemPanel<T> : Panel where T : Item {
    
    public T item = null;
    protected bool dublicate = false;
    public Text dateLabel;
    public Text title;
    protected EventSystem system;
    protected bool flag = false;

    public GameObject editButtons, newEntryButtons;

    public DateValidator dateValidator;

    protected List<string> attributeLabels;
    protected List<string> attributeValues;

    public GameObject attributeList;
    public GameObject attributeElement;

    public IAvailableSlotHandler eventGroup;

    public Button[] buttons;

    protected virtual int attributeIntent {
        get { return 0; } }

    protected override void Start()
    {
        system = EventSystem.current;
        eventGroup.OnValueChange += ResetAttibutes;
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

    public override void SetLanguage()
    {
        setTitle();
        dateLabel.text = gManager.language.Date;
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

        AttributeElement[] elements = GetComponentsInChildren<AttributeElement>();
        if (attributeValues != null)
            attributeValues.Clear();
        else
            attributeValues = new List<string>();
        foreach (AttributeElement element in elements)
            attributeValues.Add(element.value.text);
    }

    protected virtual void DisplayInfo()
    {
        setTitle();
        dateValidator.SetDate(new int[] { item.day, item.month, item.year });
        eventGroup.onSet(item.dataGroupID);
        ResetAttibutes();
    }

    public virtual void EditEntry()
    {
        setTitle();

        editButtons.SetActive(false);
        newEntryButtons.SetActive(true);
        foreach (Button b in buttons)
            b.interactable = true;
    }

    protected virtual void setTitle()
    {
        title.text = "";
    }

    public virtual void PreviewEntry(T n)
    {
        item = n;
        flag = n.filler;

        newEntryButtons.SetActive(flag);
        editButtons.SetActive(!flag);
        foreach (Button b in buttons)
            b.interactable = flag;

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

    public void ResetAttibutes()
    {
        DataGroup dG = SettingsManager.GetDataGroupID(item.Type, eventGroup.GetValue());
        string temp = dG.Attributes;
        string[] tempArr = temp.Split(',');
        attributeLabels = new List<string>();
        for (int i = attributeIntent; i < tempArr.Length; i++)
        {
            attributeLabels.Add(tempArr[i]);
        }
        GetAttributes();
    }

    public void GetAttributes()
    {
        if (attributeList != null)
            foreach (Transform t in attributeList.transform)
                Destroy(t.gameObject);
        for (int i = 0; i < attributeLabels.Count; i++)
            if (item.filler || i >= item.attributes.Length)
                SpawnAttribute(attributeLabels[i], "");
            else
                SpawnAttribute(attributeLabels[i], item.attributes[i]);
    }

    public void SpawnAttribute(string label, string value)
    {
        GameObject o = Instantiate(attributeElement);
        o.transform.SetParent(attributeList.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponent<AttributeElement>().Assign(label, value);
    }

}
