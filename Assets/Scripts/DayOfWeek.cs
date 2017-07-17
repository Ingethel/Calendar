using UnityEngine;
using UnityEngine.EventSystems;

public class DayOfWeek : IViewManager, ISelectHandler
{

    public override void RequestView()
    {
        manager.RequestView(assignedDate, Manager.ViewState.DAILY);
    }

    public void OnSelect(BaseEventData eventData)
    {
        RequestView();
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
        DayGuideView o_view = o.GetComponent<DayGuideView>();
        if (o_view != null)
        {
            o_view.SetTime(n.attributes[0] + " - " + n.attributes[1]);
            if (!n.filler)
                o_view.SetDetails(n.attributes[2] + ", #" + n.attributes[3] + ", " + n.attributes[7]);
        }
    }
    
}
