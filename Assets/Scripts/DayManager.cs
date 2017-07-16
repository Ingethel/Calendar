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
            if(!n.filler)
                o_view.SetDetails(n.attributes[2] + ", "+n.attributes[3]+", "+n.attributes[4] + ", "+n.attributes[7]);
        }
    }

    protected override void DisplayInfo()
    {
        if (info == null)
        {
            info = new NewEntryList();
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                AddFiller(setTime[i], setTime[i + 1]);
            }
        }
        base.DisplayInfo();
    }

}
