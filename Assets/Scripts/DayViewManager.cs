using UnityEngine;

public class DayViewManager : IViewManager {

    protected override void Awake()
    {
        base.Awake();
        gManager = FindObjectOfType<GameManager>();
    }

    protected override void SetHeader() {
        int dayIndex = (byte)assignedDate.DayOfWeek - 1;
        if (dayIndex < 0) dayIndex = 6;
        header.text = gManager.language.GetDay(dayIndex) + " " + assignedDate.Day.ToString() + "/" + assignedDate.Month.ToString() + "/" + assignedDate.Year.ToString();
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
            o_view.Allocate(n);
    }

    protected override void DisplayInfo()
    {
        if (info == null || info.guides.Count() == 0)
        {
            info = new DAY();
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                AddFiller(setTime[i], setTime[i + 1]);
            }
        }
        else {
            if (info.guides.Count() < setTime.Length - 1) {
                FillEmptySlots();
            }
        }
        base.DisplayInfo();
        if(info.events.Count > 0)
        {
            ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
            extras.RequestAlarmPreview(info.events);
        }
    }

}
