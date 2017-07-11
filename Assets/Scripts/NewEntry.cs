public class NewEntry
{

    public string startTime, endTime, nameOfTeam, numberOfPeople, personInCharge, telephone, dateOfConfirmation, guide, notes;

    public NewEntry()
    {
        nameOfTeam = "";
        numberOfPeople = "";
        personInCharge = "";
        telephone = "";
        dateOfConfirmation = "";
        guide = "";
        notes = "";
    }

    public NewEntry(string i_nameOfTeam, string i_numberOfPeople, string i_personInCharge, string i_telephone, string i_dateOfConfirmation, string i_guide, string i_notes)
    {
        nameOfTeam = i_nameOfTeam;
        numberOfPeople = i_numberOfPeople;
        personInCharge = i_personInCharge;
        telephone = i_telephone;
        dateOfConfirmation = i_dateOfConfirmation;
        guide = i_guide;
        notes = i_notes;
    }

}