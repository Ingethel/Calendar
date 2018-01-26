using UnityEngine.UI;
using System;

public class DateValidator : Validator {

    public InputField Day, Month, Year;
    int day, month, year;
    
    public override bool Validate()
    {
        Refresh();
        if (int.TryParse(Day.text, out day))
            if (int.TryParse(Month.text, out month))
                if (int.TryParse(Year.text, out year))
                    if (year > 0)
                        if (TimeConversions.IntInRange(month, 1, 12))
                            if (TimeConversions.IntInRange(day, 1, DateTime.DaysInMonth(year, month)))
                                if (DateTime.Compare(new DateTime(year, month, day), DateTime.Now) >= 0)
                                    return true;
        ShowError();
        return false;
    }

    public void SetDate(int[] date)
    {
        Day.text = date[0] != 0 ? date[0].ToString() : "";
        Month.text = date[1] != 0 ? date[1].ToString() : "";
        Year.text = date[2] != 0 ? date[2].ToString() : "";
    }

    public string GetDate()
    {
        return Day.text + "." + Month.text + "." + Year.text;
    }

}
