public class ColourGroupItem : AvailableSlotItem
{
    public UnityEngine.UI.Image image;

    public override void OnClick()
    {
        FindObjectOfType<AvailableColourGroups>().onSet(text.text);
    }

    public override void Assign(string s)
    {
        ColorGroup temp = SettingsManager.GetColourGroup(s);
        base.Assign(temp.Name);
        image.color = temp.Colour;
    }
}
