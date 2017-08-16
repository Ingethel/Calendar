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
    protected string[] weekendTimes = { "10:30", "12:00", "13:30", "15:00", "17:30" };
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
            for (int i = 0; i < info.guides.Count(); i++)
            {
                NewEntry n;
                if (info.guides.TryGet(i, out n)){
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
        fillerSlots++;
        Debug.Log(fillerSlots);
    }

    protected void FillEmptySlots()
    {
        int threshold = PlayerPrefs.GetInt("TimeThreshold");
        for (int i = 0; i < info.guides.Count(); i++)
        {
            NewEntry n1, n2;
            if (info.guides.TryGet(i, out n1))
            {
                if (info.guides.TryGet(i + 1, out n2))
                {
                    if (n2.GetStartTime() - n1.GetEndTime() > threshold) {
                        AddFiller(n1.attributes[1], n2.attributes[0]);
                    }
                }
                if (i == 0)
                {
                    for(int k = 1; k < setTime.Length - 1; k++)
                    {
                        int timeDif = n1.GetStartTime() - TimeConversions.StringTimeToInt(setTime[k], 60);
                        if (timeDif < 45)
                        {
                            int y = 0;
                            while (y < k - 1)
                            {
                                AddFiller(setTime[y], setTime[y + 1]);
                                y++;
                            }
                            {
                                if(n1.GetStartTime() - TimeConversions.StringTimeToInt(setTime[y], 60) >= threshold)
                                    AddFiller(setTime[y], n1.attributes[0]);
                            }
                            break;
                        }
                    }
                }
                if (i == info.guides.Count() - 1)
                {
                    for (int k = setTime.Length - 2; k > 0; k--)
                    {
                        int timeDif = TimeConversions.StringTimeToInt(setTime[k], 60) - n1.GetEndTime();
                        if (timeDif < 45)
                        {
                            int y = k + 1;
                            if (y < setTime.Length)
                                if(TimeConversions.StringTimeToInt(setTime[y], 60) - n1.GetEndTime() >= threshold)
                                    AddFiller(n1.attributes[1], setTime[y]);
                            while(y < setTime.Length - 1)
                            {
                                AddFiller(setTime[y], setTime[y + 1]);
                                y++;
                            }
                            break;
                        }
                    }
                }    
                
            }
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