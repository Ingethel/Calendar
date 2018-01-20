using System.Collections.Generic;
using UnityEngine;

public class DataGroupHandler : MonoBehaviour {

    private string[] datagroupattributes = new string[]{ "Date,Colour Group,Start Time,End Time", "Date" };

    public GameObject groupItem;
    public GameObject groupList;
    
    public UnityEngine.UI.InputField[] attributes;

    void Start()
    {
        FindObjectOfType<SettingsHandler>().InitialiseEvents += RefreshDataGroups;
    }

    public void AddDataGroup(int value)
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
        Spawn(SettingsManager.CreateDataGroup(name, attList, value));
    }
    
    public void Spawn(DataGroup dG)
    {
        GameObject o = Instantiate(groupItem);
        o.transform.SetParent(groupList.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponent<DataGroupElement>().Assign(dG);
    }

    public void RefreshDataGroups()
    {
        if (groupList != null)
            foreach (Transform t in groupList.transform)
                Destroy(t.gameObject);
        List<List<DataGroup>> list = SettingsManager.GetDataGroups();
        foreach (List<DataGroup> group in list)
            foreach (DataGroup element in group)
                Spawn(element);
    }
}
