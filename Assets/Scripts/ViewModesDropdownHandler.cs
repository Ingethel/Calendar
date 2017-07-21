using UnityEngine;

public class ViewModesDropdownHandler : Panel {
    
	void Update () {
        KeybordInputHandler();
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
