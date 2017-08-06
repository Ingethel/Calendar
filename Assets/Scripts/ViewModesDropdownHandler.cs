using UnityEngine;
using UnityEngine.UI;

public class ViewModesDropdownHandler : Panel {

    public Text[] labels;
    
	void Update () {
        KeybordInputHandler();
	}

    public override void SetLanguage()
    {
        labels[0].text = gManager.language.Monthly;
        labels[1].text = gManager.language.Weekly;
        labels[2].text = gManager.language.Daily;
    }
    
    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void OnValueChange(int i)
    {
        calendarController.RequestView((CalendarViewController.State)i);
        Close();
    }
}
