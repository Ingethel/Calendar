public class DayGuideView : IItemListView<Event>
{
    
    public override void Allocate(Event n)
    {
        base.Allocate(n);
        SetTime(n.attributes[0] + " - " + n.attributes[1]);
        if (!n.filler)
        {
            string s = "";
            if (n.attributes[2] != "")
                s += n.attributes[2];
            if (n.attributes[3] != "")
                s += ", #" + n.attributes[3];
            if (n.attributes[7] != "")
                s += ", " + n.attributes[7];
            SetDetails(s);
        }
    }
    
    public override void OnClick()
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(item, ExtrasViewController.State.NEWENTRY);
    }
}
