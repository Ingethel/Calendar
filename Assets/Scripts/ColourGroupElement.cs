using UnityEngine;

public class ColourGroupElement : MonoBehaviour {

    public UnityEngine.UI.Image image;
    public UnityEngine.UI.InputField ifield;
    public ColorGroup cG;

    public void Assign(ColorGroup c)
    {
        cG = c;
        image.color = cG.Colour;
        ifield.text = cG.Name;
    }

    public void Kill()
    {
        SettingsManager.DeleteColourGroup(cG);
        Destroy(gameObject);
    }
}
