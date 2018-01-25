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

    protected string[] setTime;

    protected int fillerSlots;

    protected override void Awake()
    {
        base.Awake();
        
        gManager.OnUpdateWeekTimes += UpdateTimetable;
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
            for (int i = 0; i < info.Events.Count(); i++)
            {
                Event n;
                if (info.Events.TryGet(i, out n)){
                    GameObject o = Instantiate(guideView);
                    o.transform.SetParent(guideList.transform);
                    o.transform.localScale = Vector3.one;
                    AssignInfo(o, n);
                }
            }
    }

    protected void AddFiller(string startTime, string endTime)
    {
        Event n = new Event();
        n.startTime = startTime;
        n.endTime = endTime;
        n.SetDate(assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString());
        n.filler = true;
        info.AddGuide(n);
    }

    protected void FillEmptySlots()
    {
        int threshold = (int)SettingsManager.ReadFloat("MinimumTourTime");
        int minTime = 100000, maxTime = 0;

        Event n1;
        if (info.Events.TryGet(0, out n1))
        {
            if (n1.GetStartTime() < minTime) minTime = n1.GetStartTime();
            if (n1.GetEndTime() > maxTime) maxTime = n1.GetEndTime();

            for (int i = 1; i < info.Events.Count(); i++)
            {
                if (info.Events.TryGet(i, out n1))
                {
                    if (n1.GetStartTime() - maxTime >= threshold)
                        AddFiller(TimeConversions.IntTimeToString(maxTime, 60), n1.attributes[0]);
                    
                    if (n1.GetStartTime() < minTime) minTime = n1.GetStartTime();
                    if (n1.GetEndTime() > maxTime) maxTime = n1.GetEndTime();
                }
            }
            int k = 0;
            for (k = 1; k < setTime.Length - 1; k++)
            {
                if (TimeConversions.StringTimeToInt(setTime[k], 60) <= minTime)
                    AddFiller(setTime[k - 1], setTime[k]);
                else break;
            }
            if(k == 1)
            {
                k--;
                if (minTime - TimeConversions.StringTimeToInt(setTime[k], 60) >= threshold)
                    AddFiller(setTime[k], TimeConversions.IntTimeToString(minTime, 60));
            }
            for (k = setTime.Length - 2; k > 0; k--)
            {
                if (TimeConversions.StringTimeToInt(setTime[k], 60) >= maxTime)
                    AddFiller(setTime[k], setTime[k + 1]);
                else break;
            }
            if (k == setTime.Length - 2)
            {
                k++;
                if (TimeConversions.StringTimeToInt(setTime[k], 60) - maxTime >= threshold)
                    AddFiller(TimeConversions.IntTimeToString(maxTime, 60), setTime[k]);
            }
        }
    }

    protected virtual void AssignInfo(GameObject o, Event n) {}

    public void UpdateTimetable()
    {
        setTime = SettingsManager.Read(assignedDate.DayOfWeek.ToString()).Split(',');
    }
    
    public override void Refresh()
    {
        fillerSlots = 0;
        info = new DAY();
        if(guideList != null)
            foreach (Transform t in guideList.transform)
                Destroy(t.gameObject);
        _tag = "";
        UpdateTimetable();
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