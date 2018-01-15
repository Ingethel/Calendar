using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGroupHandler : MonoBehaviour {

    private enum Group { EVENT, ALARM };

    private string[] datagroup = new string[] { "EventGroup", "AlarmGroup" };
    private string[] datagroupattributes = new string[]{ "Date,Colour Group,Start Time,End Time", "Date" };

    public GameObject groupItem;
    public GameObject groupList;
    
    public UnityEngine.UI.InputField[] attributes;
    
    private void Spawn(int value)
    {
        string name;
        string attList;

        string[] parts = attributes[value].text.Split(':');
        if (parts.Length >= 1)
        {
            name = parts[0];
        }
        else
            name = "default";

        if (parts.Length >= 2)
        {
            attList = datagroupattributes[value] + "," + parts[1];
        }
        else
            attList = datagroupattributes[value];

    }

    public void SpawnEvent()
    {
        Spawn(0);
    }

    public void SpawnAlarm()
    {
        Spawn(1);
    }
}
