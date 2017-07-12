using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewEntryPanelHandler : MonoBehaviour {

    public Text StartTime, EndTime, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes;
    
    public void Save()
    {
        if (CheckSaveEligibility()) {
            string[] inputs = { TryGetText(StartTime), TryGetText(EndTime), TryGetText(NameOfTeam), TryGetText(NumberOfPeople), TryGetText(PersonInCharge), TryGetText(Telephone), TryGetText(ConfirmationDate), TryGetText(Guide), TryGetText(Notes)};
            NewEntry n = new NewEntry(inputs);
            Entry r = new Entry();
            
        }
    }

    private bool CheckSaveEligibility()
    {
        if (StartTime.text != null && EndTime.text != null && Day.text != null && Month.text != null && Year.text != null)
            return true;
        return false;
    }
    
    //check numeric value

    private string TryGetText(Text a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }

}

public class Entry
{
    public string filename;
    public string tag;
    public NewEntry newEntry;
}