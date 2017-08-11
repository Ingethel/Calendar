public class AlarmItem : IItemListView<Alarm>
{
    public override void Allocate(Alarm n)
    {
        base.Allocate(n);
        SetDetails(n.attributes[0]);
    }

    public override void OnClick()
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(item, ExtrasViewController.State.ALARM);
    }
}
