using System;

public abstract class Language
{
    public string Title { protected set; get; }
}

public class English : Language
{
    public English()
    {
        Title = "Battleship G. Averof";
    }
}


public class Greek : Language
{
    public Greek()
    {
        Title = "Θ/Κ Γ. Αβέρωφ";
    }
}

public class Strings{

    public static string Entries = "Entries";
    public static string NewEntry = "NewEntry";
    public static string StartTime = "StartTime";
    public static string EndTime = "EndTime";
    public static string NameOfTeam = "NameOfTeam";
    public static string NumberOfPeople = "NumberOfPeople";
    public static string PersonInCharge = "PersonInCharge";
    public static string Telephone = "Telephone";
    public static string ConfirmationDate = "ConfirmationDate";
    public static string Guide = "Guide";
    public static string Notes = "Notes";
    public static string Day = "Day";
    public static string file = "Guides.xml";

    public static string Event = "Event";
    public static string R_Days = "RepeatDays";
    public static string R_Months = "RepeatMonths";
    public static string R_Years = "RepeatYears";
    
    public static string doctype = 
        "<!ELEMENT Entries (Day*)>" + Environment.NewLine + 
        "<!ELEMENT Day (NewEntry*, Event*)>" + Environment.NewLine + 
        "<!ELEMENT NewEntry (StartTime, EndTime, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes)>" + Environment.NewLine + 
        "<!ELEMENT Event (Notes, RepeatDays, RepeatMoths, RepeatYears)>" + Environment.NewLine + 
        "<!ELEMENT StartTime (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT EndTime (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT NameOfTeam (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT NumberOfPeople (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT PersonInCharge (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT Telephone (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT ConfirmationDate (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT Guide (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT Notes (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT RepeatDays (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT RepeatMoths (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT RepeatYears (#PCDATA)>" + Environment.NewLine +
        "<!ATTLIST Day id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST NewEntry id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Event id ID #REQUIRED>";
}