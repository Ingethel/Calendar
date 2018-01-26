using UnityEngine;

public class DataGroupElement : MonoBehaviour {

    public UnityEngine.UI.Text ifield;
    public DataGroup dG;

    public void Assign(DataGroup d)
    {
        dG = d;
        ifield.text = DataGroup.Groups[d.type] + " : " + d.Name + " : " + d.Attributes;
    }

    public void Kill()
    {
        SettingsManager.DeleteDataGroup(dG);
        Destroy(gameObject);
    }
}
