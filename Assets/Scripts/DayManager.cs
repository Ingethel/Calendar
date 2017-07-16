using System.Collections.Generic;
using UnityEngine;
public class DayManager : IViewManager {

    protected override void SetHeader() {
        header.text = assignedDate.DayOfWeek.ToString() + " " + assignedDate.Day.ToString() + " / " + assignedDate.Month.ToString()  + " / " + assignedDate.Year.ToString();
    }

    protected override void OnSetView()
    {
        RequestData();
    }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }
    
    protected override void AssignInfo(GameObject o, NewEntry n)
    {
        DayGuideView o_view = o.GetComponent<DayGuideView>();
        if (o_view != null)
        {
            o_view.SetTime(n.attributes[0] + " - " + n.attributes[1]);
        }
    }

    protected override void DisplayInfo()
    {
        if (info == null)
        {
            info = new NewEntryList();
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                NewEntry n = new NewEntry();
                n.attributes[0] = NewEntry.IntTimeToString(setTime[i]);
                n.attributes[1] = NewEntry.IntTimeToString(setTime[i + 1]);
                info.Add(n);
            }

        }
        base.DisplayInfo();
    }

}
