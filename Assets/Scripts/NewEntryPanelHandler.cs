using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewEntryPanelHandler : Panel {

    public InputField StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes;
    private EventSystem system;

    void Start() {
        system = EventSystem.current;
    }

    void Update()
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

    public void Save()
    {
        if (CheckSaveEligibility()) {
            string[] inputs = { StartTimeH.text + ":" + StartTimeM.text, EndTimeH.text + ":" + EndTimeM.text,
                TryGetText(NameOfTeam), TryGetText(NumberOfPeople), TryGetText(PersonInCharge), TryGetText(Telephone),
                TryGetText(ConfirmationDate), TryGetText(Guide), TryGetText(Notes)};
            NewEntry n = new NewEntry(inputs);
            int.TryParse(Day.text, out n.day);
            int.TryParse(Month.text, out n.month);
            int.TryParse(Year.text, out n.year);
            manager.SaveEntry(n);
            Close();
        }
    }

    private bool CheckSaveEligibility()
    {
        if (StartTimeH.text != "" && StartTimeM.text != "" && 
            EndTimeH.text != "" && EndTimeM.text != "" && 
            Day.text != "" && Month.text != "" && Year.text != "")
            return true;
        return false;
    }

    private string TryGetText(InputField a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }

}