public class NewEntry
{

    public string[] labels = {Strings.StartTime, Strings.EndTime, Strings.NameOfTeam, Strings.NumberOfPeople,Strings.PersonInCharge, Strings.Telephone, Strings.ConfirmationDate, Strings.Guide, Strings.Notes };

    public string[] attributes = { "", "", "", "", "", "", "", "", "" };
    public int day, month, year;
    public NewEntry() { }

    public NewEntry(string[] list)
    {
        attributes = list;
    }
}