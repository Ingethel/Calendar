using UnityEngine;
public class DayManager : IViewManager {

    string[] weekendTimes = { "10:30", "12:00", "13:30", "15:00", "17:30" };

    void Start()
    {
        if (assignedDate.DayOfWeek == System.DayOfWeek.Saturday || assignedDate.DayOfWeek == System.DayOfWeek.Sunday)
            setTime = weekendTimes;
    }

    protected override void SetHeader() {
        header.text = assignedDate.DayOfWeek.ToString() + " " + assignedDate.Day.ToString() + " / " + assignedDate.Month.ToString()  + " / " + assignedDate.Year.ToString();
    }

    protected override void OnSetView()
    {
        if (assignedDate.DayOfWeek == System.DayOfWeek.Monday) { }
        else
        {
            RequestData();
            DisplayInfo();
        }
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
                o_view.SetDetails(n.attributes[2] + ", #"+n.attributes[3]+", "+n.attributes[7]);
        }
    }

    protected override void DisplayInfo()
    {
        if (info == null || info.Count() == 0)
        {
            Debug.Log("List Empty. Filling with template");
            info = new NewEntryList();
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                AddFiller(setTime[i], setTime[i + 1]);
            }
        }
        else {
            Debug.Log("List Not Empty. List Size: "+info.Count());
            if (info.Count() < setTime.Length - 1) {
                Debug.Log("Filling Empty Slots");
                FillEmptySlots();
            }
        }
        base.DisplayInfo();
        Debug.Log("Displaying List Size: " + info.Count());
    }

}
