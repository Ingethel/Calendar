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
        info = new NewEntryList();
    }

    protected virtual void SetHeader() {}

    protected virtual void OnSetView() {}

    public virtual void RequestView() {}

    protected virtual void SetTag() {}

    protected void RequestData()
    {
        SearchResult res = manager.TryGetEntries(_tag);
        if (res.value)
        {
            info = res.info;
        }
    }

    protected virtual void DisplayInfo()
    {
        if(info != null)
        {
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

    protected void AddFiller(string startTime, string endTime)
    {
        NewEntry n = new NewEntry();
        n.attributes[0] = startTime;
        n.attributes[1] = endTime;
        info.Add(n);
    }

    protected void FillEmptySlots()
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
                            int y = k - 1;
                            if (y >= 0) {
                                AddFiller(setTime[y], n1.attributes[0]);
                            }
                            while(y < k - 1) {
                                AddFiller(setTime[y], setTime[y + 1]);
                                y++;
                            }    
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
                            int y = k + 1;
                            if (y < setTime.Length)
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

    protected virtual void Refresh()
    {
        info = new NewEntryList();
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