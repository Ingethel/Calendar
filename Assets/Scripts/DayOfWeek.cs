using UnityEngine;

public class DayOfWeek : IViewManager
{

    protected override void Awake()
    {
        base.Awake();
        gManager = FindObjectOfType<GameManager>();
    }

    public override void RequestView()
    {
        calendarController.RequestView(CalendarViewController.State.DAILY, assignedDate);
    }

    protected override void SetHeader()
    {
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
        IItemListView<NewEntry> o_view = o.GetComponent<IItemListView<NewEntry>>();
        if (o_view != null)
            o_view.Allocate(n);
    }
    
}
