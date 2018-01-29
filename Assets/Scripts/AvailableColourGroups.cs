using UnityEngine;

public class AvailableColourGroups : IAvailableSlotHandler
{
    ColorGroup value;

    protected override void SetData()
    {
        base.SetData();
        ColorGroup[] slots = SettingsManager.GetColorGroups();
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        foreach (ColorGroup cG in slots)
            Spawn(cG.Name);
        slotContainer.GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
    }

    public override void onSet(string s)
    {
        value = SettingsManager.GetColourGroup(s);
        GetComponent<UnityEngine.UI.Image>().color = value.Colour;
        base.onSet(s);
    }

    public override string GetValue()
    {
        return value.Name;
    }

}
