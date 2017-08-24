using UnityEngine;
using UnityEngine.UI;

public class HelpPanelHandler : Panel {
    public Text text;

    protected virtual void Update()
    {
        KeybordInputHandler();
    }

    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public override void SetLanguage()
    {
        text.text = gManager.language.Help;
    }
}
