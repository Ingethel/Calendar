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

    public void SetTime(string time)
    {
        string[] times = time.Split('-');
        if(times.Length == 2)
        {
            SetStartTime(times[0]);
            SetEndTime(times[1]);
        }
    }

    public void SetStartTime(string time)
    {
        if(time != null)
        {
            string[] temp = time.Split(':');
            if(temp.Length == 2)
            {
                SH.text = temp[0];
                SM.text = temp[1];
            }
        }
    }

    public void SetEndTime(string time)
    {
        if (time != null)
        {
            string[] temp = time.Split(':');
            if (temp.Length == 2)
            {
                EH.text = temp[0];
                EM.text = temp[1];
            }
        }
    }
    
    public string GetStartTime()
    {
        return SH.text + ":" + SM.text;
    }

    public string GetEndTime()
    {
        return EH.text + ":" + EM.text;
    }

}
