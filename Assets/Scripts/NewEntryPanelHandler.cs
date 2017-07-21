using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class NewEntryPanelHandler : Panel {

    public InputField StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes;
    public InputField[] fields;
    private EventSystem system;

    public GameObject editButtons, newEntryButtons;

    void Start() {
        system = EventSystem.current;
        fields = new InputField[]{ StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes };
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
            NewEntry n = new NewEntry(inputs, Day.text+"."+ Month.text+"."+ Year.text);
            int.TryParse(Day.text, out n.day);
            int.TryParse(Month.text, out n.month);
            int.TryParse(Year.text, out n.year);
            dataManager.RequestWrite(n);
            System.DateTime date = new System.DateTime(n.year, n.month, n.day);
            calendarController.RequestView(CalendarViewController.State.DAILY, date);
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

    public void PreviouEntry(NewEntry n)
    {
        foreach (InputField field in fields)
            field.interactable = false;

        newEntryButtons.SetActive(false);
        editButtons.SetActive(true);

        string[] split = n.attributes[0].Split(':');
        StartTimeH.text = split[0];
        StartTimeM.text = split[1];
        split = n.attributes[1].Split(':');
        EndTimeH.text = split[0];
        EndTimeM.text = split[1];
        split = n.date.Split('.');
        Day.text = split[0];
        Month.text = split[1];
        Year.text = split[2];
        NameOfTeam.text = n.attributes[2];
        NumberOfPeople.text = n.attributes[3];
        PersonInCharge.text = n.attributes[4];
        Telephone.text = n.attributes[5];
        ConfirmationDate.text = n.attributes[6];
        Guide.text = n.attributes[7];
        Notes.text = n.attributes[8];
    }

    public void EditEntry()
    {
        foreach (InputField field in fields)
            field.interactable = true;

        editButtons.SetActive(false);
        newEntryButtons.SetActive(true);
    }
    
}