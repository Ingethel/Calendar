using System;
using UnityEngine;
using UnityEngine.UI;

public class IViewManager : Panel
{
    protected Manager manager;
    protected NewEntryList info;

    protected DateTime assignedDate;
    public string _tag;
    public Text header;

    public GameObject guideView;
    public GameObject guideList;

    protected string[] setTime = { "09:00", "10:30", "12:00", "13:30" };

    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    protected virtual void SetHeader() {}

    protected virtual void OnSetView() {}

    public virtual void RequestView() {}

    protected virtual void SetTag() {}

    protected void RequestData()
    {
        SearchResult res = manager.GetEntries(_tag);
        if (res.value)
        {
            info = res.info;       
        }
        DisplayInfo();
    }

    protected virtual void DisplayInfo()
    {
        if (info != null)
        {
            if (info.Count() < 3)
                AnalyseData();

            for (int i = 0; i < info.Count(); i++)
            {
                NewEntry n;
                if (info.TryGet(i, out n)){
                    GameObject o = Instantiate(guideView);
                    o.transform.SetParent(guideList.transform);
                    o.transform.localScale = Vector3.one;
                    AssignInfo(o, n);
                }
            }
        }
    }

    protected void AddFiller(String startTime, String endTime)
    {
        NewEntry n = new NewEntry();
        n.attributes[0] = startTime;
        n.attributes[1] = endTime;
        info.Add(n);
    }

    private void AnalyseData()
    {
        for (int i = 0; i < info.Count(); i++)
        {
            NewEntry n1, n2;
            if (info.TryGet(i, out n1))
            {
                if (info.TryGet(i + 1, out n2))
                {
                    if (n2.GetStartTime() - n1.GetEndTime() > 45) {
                        AddFiller(TimeConversions.IntTimeToString(n1.GetStartTime()), TimeConversions.IntTimeToString(n2.GetStartTime()));
                    }
                }
                if (i == 0)
                {
                    for(int k = 0; k < setTime.Length - 1; k++)
                    {
                        if (n1.GetStartTime() - TimeConversions.StringTimeToInt(setTime[k]) < 45)
                        {
                            int y = 0;
                            while(y < k) {
                                AddFiller(setTime[y], setTime[y + 1]);
                                y++;
                            }
                            if(n1.GetStartTime() - TimeConversions.StringTimeToInt(setTime[y]) > 45)
                                AddFiller(setTime[y], n1.attributes[0]);
                            break;
                        }
                    }
                }
                if (i == info.Count() - 1)
                {
                    for (int k = setTime.Length - 1; k > 0; k--)
                    {
                        if (TimeConversions.StringTimeToInt(setTime[k]) - n1.GetEndTime() < 45)
                        {
                            int y = setTime.Length - 1;
                            while(y > k)
                            {
                                AddFiller(setTime[y-1], setTime[y]);
                                y--;
                            }
                            if (TimeConversions.StringTimeToInt(setTime[y]) - n1.GetEndTime() > 45)
                                AddFiller(n1.attributes[1], setTime[y]);
                            
                        break;
                        }
                    }
                }    
                
            }
        }
    }

    protected virtual void AssignInfo(GameObject o, NewEntry n) {}

    private void Refresh()
    {
        info = null;
        if(guideList != null)
            foreach (Transform t in guideList.transform)
                Destroy(t.gameObject);
        _tag = "";
    }

    public void SetView(DateTime date) {
        Refresh();
        assignedDate = date;
        SetTag();
        SetHeader();
        OnSetView();
    }
    
}