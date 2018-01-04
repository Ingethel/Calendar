public class SearchViewItem : IItemListView<Event> {
    public override void Allocate(Event n)
    {
        base.Allocate(n);
        if (item.filler)
        {
            SetDetails(n.attributes[8]);
            SetDate(n.day.ToString() + " / " + n.month.ToString() + " / " + n.year.ToString());
        }
        else
        {
            SetTime(n.attributes[0] + " - " + n.attributes[1]);
            SetDetails(n.attributes[2] + ", #" + n.attributes[3] + ", " + n.attributes[7]);
            SetDate(n.day.ToString() + " / " + n.month.ToString() + " / " + n.year.ToString());
        }
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
