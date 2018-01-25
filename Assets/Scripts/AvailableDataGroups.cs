using UnityEngine;

public class AvailableDataGroups : IAvailableSlotHandler
{

    public override void SetData()
    {
        DataGroup[] slots = SettingsManager.GetDataGroup(FindObjectOfType<ExtrasViewController>().CurrentViewIndex).ToArray();
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        foreach (DataGroup dG in slots)
            Spawn(dG.Name);
        GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
    }

}
