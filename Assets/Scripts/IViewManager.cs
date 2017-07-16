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

    protected int[] setTime = { 0900, 1030, 1200, 1330 };

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
            //while (info.Count() < 3)
              //  AnalyseData(info);

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

    private void AnalyseData(NewEntryList entries)
    {

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