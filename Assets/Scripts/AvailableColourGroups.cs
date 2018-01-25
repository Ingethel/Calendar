using UnityEngine;

public class AvailableColourGroups : IAvailableSlotHandler
{

    public override void SetData()
    {
        ColorGroup[] slots = SettingsManager.GetColorGroups();
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        foreach (ColorGroup cG in slots)
            Spawn(cG.Name);
        GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
    }

}
