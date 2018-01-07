using UnityEngine;

public class SettingsHandler : Panel {

    public GameObject advanced;
    public GameObject button;
    public Sprite expand, collapse;
    
    public override void Open()
    {
        base.Open();
        Init();
    }

    void Init()
    {
        SettingsField[] settings = GetComponentsInChildren<SettingsField>();
        foreach (SettingsField s in settings)
            s.OnStart();
        FindObjectOfType<ColourGroupHandler>().RefreshColorGroups();
        advanced.SetActive(false);        
    }

    public void ShowAdvanced()
    {
        advanced.SetActive(!advanced.activeSelf);
        if (advanced.activeSelf)
        {
            button.GetComponent<UnityEngine.UI.Image>().sprite = collapse;
        }
        else
            button.GetComponent<UnityEngine.UI.Image>().sprite = expand;
    }
    
    public void ButtonCommand(string s)
    {
        gManager.Command(new string[] { "", s });
    }
    
}
