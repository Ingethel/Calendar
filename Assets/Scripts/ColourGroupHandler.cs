using UnityEngine;

public class ColourGroupHandler : MonoBehaviour
{

    public GameObject groupItem;
    public GameObject groupList;
    public GameObject colorPicker;
    ColorPickerTriangle cP;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.InputField text;

    void Start()
    {
        FindObjectOfType<SettingsHandler>().InitialiseEvents += RefreshColorGroups;
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
        GameObject o = Instantiate(groupItem);
        o.transform.SetParent(groupList.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponent<ColourGroupElement>().Assign(cg);
    }

    public void AddColourGroup()
    {
        Spawn(SettingsManager.CreateColorGroup(image.color, text.text));
    }

    public void RefreshColorGroups()
    {
        if (groupList != null)
            foreach (Transform t in groupList.transform)
                Destroy(t.gameObject);
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
