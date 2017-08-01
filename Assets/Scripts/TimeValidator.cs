using UnityEngine.UI;

public class TimeValidator : Validator {

    public InputField SH, SM, EH, EM;
    int sH, sM, eH, eM;

    public override bool Validate()
    {
        Refresh();
        if (int.TryParse(SH.text, out sH))
            if (int.TryParse(SM.text, out sM))
                if (int.TryParse(EH.text, out eH))
                    if (int.TryParse(EM.text, out eM))
                        if (TimeConversions.IntInRange(sH, 0, 24))
                            if (TimeConversions.IntInRange(eH, 0, 24))
                                if (TimeConversions.IntInRange(sM, 0, 59))
                                    if (TimeConversions.IntInRange(eM, 0, 59))
                                    {
                                        if (sH < eH)
                                            return true;
                                        else
                                        {
                                            if (sH == eH)
                                                if (sM < eM)
                                                    return true;
                                        }
                                    }
        ShowError();
        return false;
    }

}
