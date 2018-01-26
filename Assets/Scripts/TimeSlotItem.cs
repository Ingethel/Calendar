public class TimeSlotItem : AvailableSlotItem
{

    public override void OnClick()
    {
        FindObjectOfType<AvailableTimeSlots>().onSet(text.text);
    }

}
