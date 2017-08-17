using UnityEngine;
using UnityEngine.UI;

public class AvailableSlotItem : MonoBehaviour {

    NewEntryPanelHandler handler;
    Text s;

    void Start()
    {
        handler = FindObjectOfType<NewEntryPanelHandler>();
        s = GetComponentInChildren<Text>();
    }

    public void OnClick()
    {
        handler.SetTime(s.text);
        handler.OnClickSlotExpander();
    }

}
