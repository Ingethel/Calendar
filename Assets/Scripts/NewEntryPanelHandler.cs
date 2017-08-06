using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewEntryPanelHandler : Panel {

    public InputFieldObject[] fields;
    public Text title;
    private EventSystem system;

    public GameObject editButtons, newEntryButtons;
    public DateValidator dateValidator;
    public TimeValidator timeValidator;

    public NewEntry guide = null;
    
    void Start() {
        system = EventSystem.current;
    }

    void Update()
    {
        KeybordInputHandler();
    }

    public override void Close()
    {
        guide = null;
        base.Close();
    }

    public override void SetLanguage()
    {
        title.text = gManager.language.NewEntry;
        fields[0].label.text = gManager.language.NameOfTeam;
        fields[1].label.text = gManager.language.NumberOfPeople;
        fields[2].label.text = gManager.language.PersonInCharge;
        fields[3].label.text = gManager.language.Telephone;
        fields[4].label.text = gManager.language.Date;
        fields[5].label.text = gManager.language.Time;
        fields[6].label.text = gManager.language.DateOfConfirmation;
        fields[7].label.text = gManager.language.Guide;
        fields[8].label.text = gManager.language.Notes;
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

    public void Save()
    {
        if (CheckSaveEligibility()) {

            if (guide != null && !guide.filler)
                    dataManager.RequestDelete(guide);
             
            SaveInfo();
            dataManager.RequestWrite(guide);
            calendarController.RequestView(CalendarViewController.State.DAILY, new System.DateTime(guide.year, guide.month, guide.day));
            Close();
        }
    }

    public void Delete()
    {
        dataManager.RequestDelete(guide);
        calendarController.RefreshView();
        Close();
    }

    private bool CheckSaveEligibility()
    {
        if (dateValidator.Validate() && timeValidator.Validate())
            return true;
        return false;
    }

    private string TryGetText(InputField a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }

    public void PreviewEntry(NewEntry n)
    {
        guide = n;
        bool flag = n.filler;

        foreach (InputFieldObject fieldObj in fields)
            foreach(InputField field in fieldObj.inputs)
                field.interactable = flag;

        title.text = flag ? gManager.language.NewEntry : gManager.language.NewEntryPreview;
        
        newEntryButtons.SetActive(flag);
        editButtons.SetActive(!flag);

        DisplayInfo();
    }

    private void DisplayInfo()
    {
        string[] split = guide.attributes[0].Split(':');
        if(split.Length == 2)
        {
            fields[5].inputs[0].text = split[0];
            fields[5].inputs[1].text = split[1];
        }
        split = guide.attributes[1].Split(':');
        if (split.Length == 2)
        {
            fields[5].inputs[2].text = split[0];
            fields[5].inputs[3].text = split[1];
        }
        fields[4].inputs[0].text = guide.day != 0 ? guide.day.ToString() : "";
        fields[4].inputs[1].text = guide.month != 0 ? guide.month.ToString() : "";
        fields[4].inputs[2].text = guide.year != 0 ? guide.year.ToString() : "";
        fields[0].inputs[0].text = guide.attributes[2];
        fields[1].inputs[0].text = guide.attributes[3];
        fields[2].inputs[0].text = guide.attributes[4];
        fields[3].inputs[0].text = guide.attributes[5];
        fields[6].inputs[0].text = guide.attributes[6];
        fields[7].inputs[0].text = guide.attributes[7];
        fields[8].inputs[0].text = guide.attributes[8];
    }

    private void SaveInfo()
    {
        string[] inputs = { fields[5].inputs[0].text + ":" + fields[5].inputs[1].text, fields[5].inputs[2].text + ":" + fields[5].inputs[3].text,
                TryGetText(fields[0].inputs[0]), TryGetText(fields[1].inputs[0]), TryGetText(fields[2].inputs[0]), TryGetText(fields[3].inputs[0]),
                TryGetText(fields[6].inputs[0]), TryGetText(fields[7].inputs[0]), TryGetText(fields[8].inputs[0])};
        guide = new NewEntry(inputs, fields[4].inputs[0].text + "." + fields[4].inputs[1].text + "." + fields[4].inputs[2].text);
    }

    public void EditEntry()
    {
        foreach(InputFieldObject fieldObj in fields)
            foreach (InputField field in fieldObj.inputs)
            field.interactable = true;

        title.text = gManager.language.NewEntry;

        editButtons.SetActive(false);
        newEntryButtons.SetActive(true);
    }
    
}