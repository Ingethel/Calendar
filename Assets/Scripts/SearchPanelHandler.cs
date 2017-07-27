using UnityEngine;
using UnityEngine.UI;

public class SearchPanelHandler : Panel
{
    public Text input;

    void Update()
    {
        KeybordInputHandler();
    }

    public override void Close()
    {
        input.text = "";
        base.Close();
    }

    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dataManager.SearchTerm(input.text);
            
            Close();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
}
