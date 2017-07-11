﻿using System.Collections.Generic;

public class DayManager : IViewManager {

    private List<NewEntry> info;

    protected override void SetHeader() {
        header.text = assignedDate.DayOfWeek.ToString() + " " + assignedDate.Day.ToString() + " / " + assignedDate.Month.ToString()  + " / " + assignedDate.Year.ToString();
    }

    protected override void OnSetView() { }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }

    protected override void RequestData()
    {
        SearchResult res = manager.GetEntries(_tag);
        if (res.value)
        {
            info = res.info;
        }
    }

}
