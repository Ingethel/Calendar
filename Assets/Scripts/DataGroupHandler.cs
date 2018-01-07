using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGroupHandler : MonoBehaviour {

    private enum Group { EVENT, ALARM };

    private string[] datagroup = new string[] { "EventGroup", "AlarmGroup" };
    private string[] datagroupattributes = new string[]{ "Date,Colour Group,Start Time,End Time", "Date" };

    public GameObject groupItem;
    public GameObject groupList;
    public GameObject extraPanel;

    public UnityEngine.UI.Text label;
    public UnityEngine.UI.Text attributes;

    int value;
    
    void Start () {
        extraPanel.SetActive(false);
	}
	
	public void ExtraPanel()
    {
        extraPanel.SetActive(extraPanel.activeSelf);
    }

    public void ChangeDataGroup(int i)
    {
        value = i;
    }

    public void Spawn()
    {

    }
}
