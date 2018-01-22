using UnityEngine;

public class AttributeElement : MonoBehaviour {
    public UnityEngine.UI.Text label, value;

    public void Assign(string l, string v)
    {
        label.text = l;
        value.text = v;
    }

}
