using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlarmPanelHandler : Panel {

    private EventSystem system;
    public InputField Day, Month, Year, Repeat, Notes;
    private int RepeatFreqModifier;
    Alarm alarm;

    void Start ()
    {
        system = EventSystem.current;
    }

    void Update ()
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

    private bool CheckSaveEligibility()
    {
        if (Day.text != "" && Month.text != "" && Year.text != "" && Notes.text != "")
            return true;
        return false;
    }

    private void SaveInfo()
    {
        int temp;
        if (int.TryParse(Repeat.text, out temp))
            alarm = new Alarm(Day.text + "." + Month.text + "." + Year.text, Notes.text, temp, RepeatFreqModifier);
        else
            alarm = new Alarm(Day.text + "." + Month.text + "." + Year.text, Notes.text);
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
