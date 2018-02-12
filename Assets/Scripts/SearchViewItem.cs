public class SearchViewItem : IItemListView<Event> {
    public override void Allocate(Event n)
    {
        base.Allocate(n);
        SetTime(n.startTime + " - " + n.endTime);
        SetDetails(n.ToString());
        SetDate(n.day.ToString() + " / " + n.month.ToString() + " / " + n.year.ToString());
    }

    public override void OnClick()
    {
        if (item.filler)
        {
            CalendarViewController calendar = FindObjectOfType<CalendarViewController>();
            if (calendar)
                calendar.RequestView(CalendarViewController.State.DAILY, new System.DateTime(item.year, item.month, item.day));
        }
        else
        {
            ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
            if (extras)
                extras.RequestEntryPreview(item, ExtrasViewController.State.NEWENTRY);
        }
    }
}
