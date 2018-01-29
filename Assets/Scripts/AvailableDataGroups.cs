using UnityEngine;

public class AvailableDataGroups : IAvailableSlotHandler
{

    protected override void SetData()
    {
        base.SetData();
        DataGroup[] slots = SettingsManager.GetDataGroup(FindObjectOfType<ExtrasViewController>().CurrentViewIndex).ToArray();
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        foreach (DataGroup dG in slots)
            Spawn(dG.Name);
        slotContainer.GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
    }

    public override void onSet(string s)
    {
        DataGroup temp;
        temp = SettingsManager.GetDataGroupID((DataGroup.DataGroups)FindObjectOfType<ExtrasViewController>().CurrentViewIndex, s);
        GetComponent<UnityEngine.UI.Text>().text = temp.Name;
        base.onSet(s);
    }

    public override string GetValue()
    {
        return GetComponent<UnityEngine.UI.Text>().text;
    }

}
