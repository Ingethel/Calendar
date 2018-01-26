using UnityEngine;

public class AvailableColourGroups : IAvailableSlotHandler
{

    protected override void SetData()
    {
        base.SetData();
        ColorGroup[] slots = SettingsManager.GetColorGroups();
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        foreach (ColorGroup cG in slots)
            Spawn(cG.Name);
        slotContainer.GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
    }

}
