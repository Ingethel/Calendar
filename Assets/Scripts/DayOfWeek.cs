using UnityEngine;
using UnityEngine.UI;

public class DayOfWeek : IViewManager
{
    public Text AF;

    private void Start()
    {
        gManager.OnLanguageChange += SetLanguage;
    }

    public override void Refresh()
    {
        base.Refresh();
        AF.text = "";
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
        RequestData();
        if (assignedDate.DayOfWeek == System.DayOfWeek.Monday) { }
        else
        {
            DisplayInfo();
        }
        AF.text = info.officer;
    }

    public override void SetLanguage()
    {
        SetHeader();
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
