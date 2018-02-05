using UnityEngine;
using UnityEngine.UI;

public class AttributeElement : MonoBehaviour {
    public Text label;
    public InputField value;

    public void Assign(string l, string v)
    {
        label.text = l;
        value.text = v;
    }

}
