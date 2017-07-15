using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewEntryPanelHandler : MonoBehaviour {

    public InputField StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes;
    private Manager manager;
    private EventSystem system;

    void Start() {
        manager = FindObjectOfType<Manager>();
        system = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if(next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if(inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }
            }
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
        }
    }

    private bool CheckSaveEligibility()
    {
        if (StartTimeH.text != null && StartTimeM.text != null && 
            EndTimeH.text != null && EndTimeM.text != null && 
            Day.text != null && Month.text != null && Year.text != null)
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