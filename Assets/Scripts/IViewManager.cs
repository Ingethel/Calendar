using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IViewManager : MonoBehaviour
{
    protected Manager manager;
    protected List<NewEntry> info;

    protected DateTime assignedDate;
    public string _tag;
    public Text header;

    public GameObject guideView;
    public GameObject guideList;

    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    protected virtual void SetHeader() {}

    protected virtual void OnSetView() {}

    public virtual void RequestView() {}

    protected virtual void SetTag() {}

    protected virtual void RequestData() {}

    protected virtual void DisplayInfo() {}

    public void SetView(DateTime date) {
        assignedDate = date;
        SetTag();
        OnSetView();
        SetHeader();
        RequestData();
        DisplayInfo();
    }

}