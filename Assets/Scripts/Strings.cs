using System;

public abstract class Language
{
    public string Title { protected set; get; }
    public string Time { protected set; get; }
    public string Details { protected set; get; }
    public string Date { protected set; get; }
    public string NameOfTeam { protected set; get; }
    public string NumberOfPeople { protected set; get; }
    public string PersonInCharge { protected set; get; }
    public string Telephone { protected set; get; }
    public string DateOfConfirmation { protected set; get; }
    public string Guide { protected set; get; }
    public string Notes { protected set; get; }
    public string Search { protected set; get; }
    public string Monthly { protected set; get; }
    public string Weekly { protected set; get; }
    public string Daily { protected set; get; }
    public string RepeatEvery { protected set; get; }
    public string Days { protected set; get; }
    public string Months { protected set; get; }
    public string Weeks { protected set; get; }
    public string NewEntry { protected set; get; }
    public string NewEntryPreview { protected set; get; }
    public string SearchResult { protected set; get; }
    public string OfficerOnDuty { protected set; get; }
    public string NavalOfficer { protected set; get; }
    public string ChiefOfMuseum { protected set; get; }
    public string WeeklyGuideSchedule { protected set; get; }
    public string WeeklyButton { protected set; get; }
    public string NewAlarm { protected set; get; }
    public string AlarmPreview { protected set; get; }
    public string ReportAlarmNotes { protected set; get; }

    protected string[] MonthLabels;
    protected string[] DayLabels;

    public string GetDay(int i)
    {
        if (i < DayLabels.Length)
            return DayLabels[i];
        return DayLabels[0];
    }

    public string GetMonth(int i)
    {
        if (i < MonthLabels.Length)
            return MonthLabels[i];
        return MonthLabels[0];
    }
}

public class English : Language
{
    public English()
    {
        Title = "Battleship G. Averof";
        MonthLabels = new string[]{ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        DayLabels = new string[]{ "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        Time = "Time";
        Details = "Details";
        Date = "Date";
        NameOfTeam = "Name of Team";
        NumberOfPeople = "Number of People";
        PersonInCharge = "Person in Charge";
        Telephone = "Telephone";
        DateOfConfirmation = "Date of Confirmation";
        Guide = "Guide";
        Notes = "Notes";
        Search = "Search";
        Monthly = "Monthly";
        Weekly = "Weekly";
        Daily = "Daily";
        Months = "Months";
        Weeks = "Weeks";
        Days = "Days";
        RepeatEvery = "Repeat every ";
        NewEntry = "Add Guided Tour";
        NewEntryPreview = "Guided Tour";
        SearchResult = "Search Result for: ";
        OfficerOnDuty = "Officer on Duty: ";
        NavalOfficer = "   Naval Officer";
        ChiefOfMuseum = "   Chief of Museum Department";
        WeeklyGuideSchedule = "Weekly Guide Scedule Battleship Averof";
        WeeklyButton = "Show Weekly Schedule";
        NewAlarm = "Add Alarm";
        AlarmPreview = "Alarm";
        ReportAlarmNotes = "Prepare Semester Report";
    }
}


public class Greek : Language
{
    public Greek()
    {
        Title = "Θ/Κ Γ. Αβέρωφ";
        MonthLabels = new string[]{ "Ιανουάριος", "Φεβρουάριος", "Μάρτιος", "Απρίλιος", "Μάιος", "Ιούνιος", "Ιούλιος", "Αύγουστος", "Σεπτέμβριος", "Οκτόβριος", "Νοέμβριος", "Δεκέμβριος" };
        DayLabels = new string[] { "Δευτέρα", "Τρίτη", "Τετάρτη", "Πέμπτη", "Παρασκευή", "Σάββατο", "Κυριακή" };
        Time = "Ώρα";
        Details = "Πληροφορίες";
        Date = "Ημερομηνία";
        NameOfTeam = "Όνομα Ομάδας";
        NumberOfPeople = "Αριθμός Ατόμων";
        PersonInCharge = "Υπεύθηνος Ομάδας";
        Telephone = "Τηλέφωνο";
        DateOfConfirmation = "Ημερομηνία Αποδοχής";
        Guide = "Ξεναγός";
        Notes = "Συμείωση";
        Search = "Αναζήτηση";
        Monthly = "Μηνιαία";
        Weekly = "Εβδομαδιαία";
        Daily = "Ημερήσια";
        Months = "Μήνες";
        Weeks = "Εβδομάδες";
        Days = "Μέρες";
        RepeatEvery = "Επανάληψη καθε ";
        NewEntry = "Εισαγωγή Ξενάγησης";
        NewEntryPreview = "Ξενάγηση";
        SearchResult = "Αποτέλεσμα Αναζήτησης: ";
        OfficerOnDuty = "ΑΦ: ";
        NavalOfficer = "   Ύπαρχος";
        ChiefOfMuseum = "   Τμηματάρχης Μουσείου";
        WeeklyGuideSchedule = "Εβδομαδιαίο Πρόγραμμα Ξεωαγήσεων Θ/Κ Αβέρωφ";
        WeeklyButton = "Προβολή Εβδομαδιαίου Προγράμματος";
        NewAlarm = "Εισαγωγή Υπενθύμισης";
        AlarmPreview = "Υπενθύμιση";
        ReportAlarmNotes = "Υπενθύμιση Τριμηνιαίας Αναφοράς";
    }
}

public class Strings{

    public static string Entries = "Entries";
    public static string NewEntry = "NewEntry";
    public static string Officer = "Officer";
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
        "<!ELEMENT Event (Notes, RepeatDays, RepeatMonths, RepeatYears)>" + Environment.NewLine +
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
        "<!ELEMENT RepeatMonths (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT RepeatYears (#PCDATA)>" + Environment.NewLine +
        "<!ATTLIST Day id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Day officer CDATA #IMPLIED>" + Environment.NewLine +
        "<!ATTLIST NewEntry id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Event id ID #REQUIRED>";
}