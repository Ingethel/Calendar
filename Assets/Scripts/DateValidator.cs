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
}
