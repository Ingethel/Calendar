public class TimeSlotItem : AvailableSlotItem
{

    public override void OnClick()
    {
        handler.SetTime(text.text);
        handler.OnClickSlotExpander();
    }

}
