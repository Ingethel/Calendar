using UnityEngine;
using UnityEngine.UI;

public class AvailableSlotItem : MonoBehaviour {

    protected NewEntryPanelHandler handler;
    protected Text text;

    protected virtual void Start()
    {
        handler = FindObjectOfType<NewEntryPanelHandler>();
    }

    public virtual void OnClick(){}

    public virtual void Assign(string s)
    {
        text = GetComponentInChildren<Text>();
        text.text = s;
    }

}
