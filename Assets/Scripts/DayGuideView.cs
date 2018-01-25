public class DayGuideView : IItemListView<Event>
{
    
    public override void Allocate(Event n)
    {
        base.Allocate(n);
        SetTime(n.startTime + " - " + n.endTime);
        if (!n.filler)
        {
            SetDetails(n.ToString());
        }
    }
    
    public override void OnClick()
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(item, ExtrasViewController.State.NEWENTRY);
    }
}
