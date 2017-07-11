using System;
using UnityEngine;
using UnityEngine.UI;

public class IViewManager : MonoBehaviour
{

    protected Manager manager;

    protected DateTime assignedDate;
    public string _tag;
    public Text header;

    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    protected virtual void SetHeader() {}

    protected virtual void OnSetView() {}

    public virtual void RequestView() {}

    protected virtual void SetTag() {}

    protected virtual void RequestData() {}

    public void SetView(DateTime date) {
        assignedDate = date;
        SetTag();
        OnSetView();
        SetHeader();
        RequestData();
    }

}
