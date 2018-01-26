using UnityEngine;

public class AvailableTimeSlots : IAvailableSlotHandler
{
    public Sprite expand, collapse;
    public GameObject button;

    public override void onClick()
    {
        if (!slotContainer.activeSelf)
            SetData();
        
        else
            Clear();
    }

    public override void Clear()
    {
        base.Clear();
        button.GetComponent<UnityEngine.UI.Image>().sprite = expand;
    }

    protected override void SetData()
    {
        base.SetData();
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
            slotContainer.GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
            button.GetComponent<UnityEngine.UI.Image>().sprite = collapse;
        }
        else
            button.SetActive(false);

    }

    public override void onSet(string s)
    {
        GetComponent<TimeValidator>().SetTime(s);
        base.onSet(s);
    }
    
    public override void SetActive(bool b)
    {
        Clear();
        button.SetActive(b);
    }

}
