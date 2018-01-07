using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGroupHandler : MonoBehaviour
{

    public GameObject colorGroupItem;
    public GameObject colorGroupList;
    public GameObject colorPicker;
    ColorPickerTriangle cP;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.InputField text;

    void Start()
    {
        cP = colorPicker.GetComponentInChildren<ColorPickerTriangle>();
        colorPicker.SetActive(false);
    }

    void Update()
    {
        if (colorPicker.activeSelf)
            image.color = cP.TheColor;
    }

    private void Spawn(ColorGroup cg)
    {
        GameObject o = Instantiate(colorGroupItem);
        o.transform.SetParent(colorGroupList.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponent<ColourGroupElement>().Assign(cg);
    }

    public void AddColourGroup()
    {
        Spawn(SettingsManager.CreateColorGroup(image.color, text.text));
    }

    public void RefreshColorGroups()
    {
        ColorGroup[] cGs = SettingsManager.GetColorGroups();
        foreach(ColorGroup cg in cGs)
            Spawn(cg);
    }

    public void OpenColorPicker()
    {
        colorPicker.SetActive(!colorPicker.activeSelf);
        if (colorPicker.activeSelf)
            cP.SetNewColor(image.color);
    }

}
