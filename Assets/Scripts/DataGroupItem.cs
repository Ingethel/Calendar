public class DataGroupItem : AvailableSlotItem
{

    public override void OnClick()
    {
        FindObjectOfType<AvailableDataGroups>().onSet(text.text);
    }

}
