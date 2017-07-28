using UnityEngine;

public class DayOfWeek : IViewManager
{

    public override void RequestView()
    {
        calendarController.RequestView(CalendarViewController.State.DAILY, assignedDate);
    }

    protected override void SetHeader()
    {
        header.text = assignedDate.DayOfWeek.ToString() + " " + assignedDate.Day.ToString() + "/" + assignedDate.Month.ToString() + "/" + assignedDate.Year.ToString();
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
