using System;
using UnityEngine;
using UnityEngine.UI;

public class IViewManager : Panel
{
    protected DAY info;

    protected DateTime assignedDate;
    public string _tag;
    public Text header;

    public GameObject guideView;
    public GameObject guideList;

    protected string[] weekTimes = { "09:00", "10:30", "12:00", "13:30" };
    protected string[] weekendTimes = { "10:30", "12:00", "13:30", "15:00", "16:30" };
    protected string[] setTime;

    protected int fillerSlots;

    protected override void Awake()
    {
        base.Awake();
        info = new DAY();
    }

    public virtual void OnClick()
    {
        RequestView();
    }

    protected virtual void SetHeader() {}

    protected virtual void OnSetView() {}

    public virtual void RequestView() {}

    protected virtual void SetTag() {}

    protected virtual void RequestData(){}

    protected virtual void DisplayInfo()
    {
        if(info != null)
            for (int i = 0; i < info.Guides.Count(); i++)
            {
                NewEntry n;
                if (info.Guides.TryGet(i, out n)){
                    GameObject o = Instantiate(guideView);
                    o.transform.SetParent(guideList.transform);
                    o.transform.localScale = Vector3.one;
                    AssignInfo(o, n);
                }
            }
    }

    protected void AddFiller(string startTime, string endTime)
    {
        NewEntry n = new NewEntry();
        n.attributes[0] = startTime;
        n.attributes[1] = endTime;
        n.SetDate(assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString());
        n.filler = true;
        info.AddGuide(n);
    }

    protected void FillEmptySlots()
    {
        int threshold = PlayerPrefs.GetInt("TimeThreshold");
        int minTime = 100000, maxTime = 0;

        NewEntry n1;
        if (info.Guides.TryGet(0, out n1))
        {
            if (n1.GetStartTime() < minTime) minTime = n1.GetStartTime();
            if (n1.GetEndTime() > maxTime) maxTime = n1.GetEndTime();

            for (int i = 1; i < info.Guides.Count(); i++)
            {
                if (info.Guides.TryGet(i, out n1))
                {

                    if (n1.GetStartTime() - maxTime >= threshold)
                    {
                        AddFiller(TimeConversions.IntTimeToString(maxTime, 60), n1.attributes[0]);
                    }

                    if (n1.GetStartTime() < minTime) minTime = n1.GetStartTime();
                    if (n1.GetEndTime() > maxTime) maxTime = n1.GetEndTime();
                }
            }

            if(minTime - TimeConversions.StringTimeToInt(setTime[0], 60) >= threshold)
                AddFiller(setTime[0], TimeConversions.IntTimeToString(minTime, 60));
/*            for (int k = 1; k < setTime.Length - 1; k++)
            {
                int timeDif = Math.Abs(minTime - TimeConversions.StringTimeToInt(setTime[k], 60));
                if (timeDif < threshold)
                {
                    int y = 0;
                    while (y < k - 1)
                    {
                        AddFiller(setTime[y], setTime[y + 1]);
                        y++;
                    }
                    {
                    if(minTime - TimeConversions.StringTimeToInt(setTime[y], 60) >= threshold)
                        AddFiller(setTime[y], TimeConversions.IntTimeToString(minTime, 60));
                    }
                    break;
                    }
            }*/
            if(TimeConversions.StringTimeToInt(setTime[setTime.Length-1], 60) - maxTime >= threshold)
                AddFiller(TimeConversions.IntTimeToString(maxTime, 60), setTime[setTime.Length - 1]);
/*            for (int k = setTime.Length - 2; k > 0; k--)
            {
                int timeDif = Math.Abs(TimeConversions.StringTimeToInt(setTime[k], 60) - maxTime);
                if (timeDif < threshold)
                {
                    int y = k + 1;
                    if(TimeConversions.StringTimeToInt(setTime[y], 60) - maxTime >= threshold)
                        AddFiller(TimeConversions.IntTimeToString(maxTime, 60), setTime[y]);
                    while(y < setTime.Length - 1)
                    {
                        AddFiller(setTime[y], setTime[y + 1]);
                        y++;
                    }
                    break;
                    }
                }
             */
        }
    }

    protected virtual void AssignInfo(GameObject o, NewEntry n) {}

    public override void Refresh()
    {
        fillerSlots = 0;
        info = new DAY();
        if(guideList != null)
            foreach (Transform t in guideList.transform)
                Destroy(t.gameObject);
        _tag = "";

        if (assignedDate.DayOfWeek == System.DayOfWeek.Saturday || assignedDate.DayOfWeek == System.DayOfWeek.Sunday)
            setTime = weekendTimes;
        else
            setTime = weekTimes;
    }

    public void SetView(DateTime date) {
        assignedDate = date;
        Refresh();
        SetTag();
        SetHeader();
        OnSetView();
    }

    public virtual void RequestLegacyData(){}
}