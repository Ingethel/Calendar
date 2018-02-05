using System;

public abstract class Language
{
    public string Title { protected set; get; }
    public string Time { protected set; get; }
    public string Details { protected set; get; }
    public string Date { protected set; get; }
    public string Search { protected set; get; }
    public string Monthly { protected set; get; }
    public string Weekly { protected set; get; }
    public string Daily { protected set; get; }
    public string NewEntry { protected set; get; }
    public string NewEntryPreview { protected set; get; }
    public string SearchResult { protected set; get; }
    public string OfficerOnDuty { protected set; get; }
    public string TourGuies { protected set; get; }
    public string NavalOfficer { protected set; get; }
    public string ChiefOfMuseum { protected set; get; }
    public string WeeklyGuideSchedule { protected set; get; }
    public string WeeklyButton { protected set; get; }
    public string NewAlarm { protected set; get; }
    public string AlarmPreview { protected set; get; }
    public string ReportAlarmNotes { protected set; get; }
    public string LegacyButton { protected set; get; }
    public string Help { protected set; get; }

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
        Search = "Search";
        Monthly = "Monthly";
        Weekly = "Weekly";
        Daily = "Daily";
        NewEntry = "Add New Event";
        NewEntryPreview = "Event";
        SearchResult = "Search Result for: ";
        OfficerOnDuty = "Officer on Duty: ";
        TourGuies = "Tour Guides: ";
        NavalOfficer = "   Naval Officer";
        ChiefOfMuseum = "   Chief of Museum Department";
        WeeklyGuideSchedule = "Weekly Guide Scedule Battleship Averof";
        WeeklyButton = "Show Weekly Schedule";
        NewAlarm = "Add New Alarm";
        AlarmPreview = "Alarm";
        ReportAlarmNotes = "Prepare Semester Report";
        LegacyButton = "Associated data is marked as old." + Environment.NewLine + Environment.NewLine + "Click to retrieve it.";
        Help = "For any enquiry please refer to the manual or contact me at stavros_anast@hotmail.com";
    }
}

public class Greek : Language
{
    public Greek()
    {
        Title = "Θ/Κ Γ. Αβέρωφ";
        MonthLabels = new string[]{ "Ιανουάριος", "Φεβρουάριος", "Μάρτιος", "Απρίλιος", "Μάιος", "Ιούνιος", "Ιούλιος", "Αύγουστος", "Σεπτέμβριος", "Οκτώβριος", "Νοέμβριος", "Δεκέμβριος" };
        DayLabels = new string[] { "Δευτέρα", "Τρίτη", "Τετάρτη", "Πέμπτη", "Παρασκευή", "Σάββατο", "Κυριακή" };
        Time = "Ώρα";
        Details = "Πληροφορίες";
        Date = "Ημερομηνία";
        Search = "Αναζήτηση";
        Monthly = "Μηνιαία";
        Weekly = "Εβδομαδιαία";
        Daily = "Ημερήσια";
        NewEntry = "Εισαγωγή Ξενάγησης";
        NewEntryPreview = "Ξενάγηση";
        SearchResult = "Αποτέλεσμα Αναζήτησης: ";
        OfficerOnDuty = "ΑΦ: ";
        TourGuies = "Ξεναγοί: ";
        NavalOfficer = "   Ύπαρχος";
        ChiefOfMuseum = "   Τμηματάρχης Μουσείου";
        WeeklyGuideSchedule = "Εβδομαδιαίο Πρόγραμμα Ξεναγήσεων Θ/Κ Αβέρωφ";
        WeeklyButton = "Προβολή Εβδομαδιαίου Προγράμματος";
        NewAlarm = "Εισαγωγή Υπενθύμισης";
        AlarmPreview = "Υπενθύμιση";
        ReportAlarmNotes = "Υπενθύμιση Τριμηνιαίας Αναφοράς";
        LegacyButton = "Τα σχετικά δεδομένα είναι παλαιά." + Environment.NewLine + Environment.NewLine + "Πατήστε για να ανακτηθούν.";
        Help = "Εάν εχετε ερωτήσεις παρακαλώ αναφερθείτε στο εγχειρίδιο, αλλιώς επικοινωνήστε στο stavros_anast@hotmail.com";
    }
}

public class DataStrings{

    public static string Entries = "Entries";
    public static string Event = "Event";
    public static string Officer = "Officer";
    public static string Day = "Day";
    public static string file = "Guides.xml";
    public static string Colour = "Colour";
    public static string Alarm = "Alarm";
    
    public static string doctype =
        "<!ELEMENT Entries (Day*)>" + Environment.NewLine +
        "<!ELEMENT Day (Event*, Alarm*)>" + Environment.NewLine +
        "<!ELEMENT Event (#PCDATA)>" + Environment.NewLine +
        "<!ELEMENT Alarm (#PCDATA)>" + Environment.NewLine +
        "<!ATTLIST Day id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Day officer CDATA #IMPLIED>" + Environment.NewLine +
        "<!ATTLIST Day guides CDATA #IMPLIED>" + Environment.NewLine +
        "<!ATTLIST Event id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Event dataGroup CDATA #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Alarm id ID #REQUIRED>" + Environment.NewLine +
        "<!ATTLIST Alarm dataGroup CDATA #REQUIRED>" + Environment.NewLine;

}
