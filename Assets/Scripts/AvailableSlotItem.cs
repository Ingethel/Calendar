using UnityEngine;
using UnityEngine.UI;

public class AvailableSlotItem : MonoBehaviour {

    protected Text text;
    
    public virtual void OnClick(){}

    public virtual void Assign(string s)
    {
        text = GetComponentInChildren<Text>();
        text.text = s;
    }

}
