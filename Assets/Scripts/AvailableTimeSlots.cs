using UnityEngine;

public class AvailableTimeSlots : IAvailableSlotHandler
{
    
    public override void SetData()
    {
        NewEntryPanelHandler handler = FindObjectOfType<NewEntryPanelHandler>();
        int year, month, day;
        int.TryParse(handler.fields[0].inputs[0].text, out day);
        int.TryParse(handler.fields[0].inputs[1].text, out month);
        int.TryParse(handler.fields[0].inputs[2].text, out year);

        string[] slots = FindObjectOfType<CalendarViewController>().RequestEmptySlots(new System.DateTime(year, month, day));
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        if (slots != null && slots.Length > 0)
        {
            foreach (string s in slots)
                Spawn(s);
            GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
        }
    }

}
