public class AlarmItem : IItemListView<Alarm>
{
    public override void OnClick()
    {
    }

    public override void Allocate(Alarm n)
    {
        base.Allocate(n);
        SetDetails(n.attributes[0]);
    }

}
