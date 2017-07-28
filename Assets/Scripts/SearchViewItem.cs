public class SearchViewItem : IItemListView<NewEntry> {

    public override void Allocate(NewEntry n)
    {
        base.Allocate(n);
        SetTime(n.attributes[0] + " - " + n.attributes[1]);
        SetDetails(n.attributes[2] + ", #" + n.attributes[3] + ", " + n.attributes[7]);
        SetDate(n.day.ToString() + " / " + n.month.ToString() + " / " + n.year.ToString());
    }
    
}
