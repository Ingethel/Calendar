using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlarmPanelHandler : Panel {

    private EventSystem system;
    public InputFieldObject[] fields;
    public DateValidator dateValidator;
    public Dropdown Modifier;
    private int RepeatFreqModifier;
    Alarm alarm;

    void Start ()
    {
        system = EventSystem.current;
        Refresh();
    }

    void Update ()
    {
        KeybordInputHandler();
    }

    public override void Refresh()
    {
        fields[0].label.text = gManager.language.Date;
        fields[1].label.text = gManager.language.RepeatEvery;
        fields[2].label.text = gManager.language.Notes;
        Modifier.options[0].text = gManager.language.Days;
        Modifier.options[1].text = gManager.language.Weeks;
        Modifier.options[2].text = gManager.language.Months;
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

    private bool CheckSaveEligibility()
    {
        if (dateValidator.Validate() && fields[2].inputs[0].text != "")
            return true;
        return false;
    }

    private void SaveInfo()
    {
        int temp;
        if (int.TryParse(fields[1].inputs[0].text, out temp))
            alarm = new Alarm(fields[0].inputs[0].text + "." + fields[0].inputs[1].text + "." + fields[0].inputs[2].text, fields[2].inputs[0].text, temp, RepeatFreqModifier);
        else
            alarm = new Alarm(fields[0].inputs[0].text + "." + fields[0].inputs[1].text + "." + fields[0].inputs[2].text, fields[2].inputs[0].text);
    }

    public void Save()
    {
        if (CheckSaveEligibility())
        {
            SaveInfo();
            dataManager.RequestWrite(alarm);
            calendarController.RefreshView();
            Close();
        }
    }

    public void ChangeModifier(int i)
    {
        RepeatFreqModifier = i;
    }
}
