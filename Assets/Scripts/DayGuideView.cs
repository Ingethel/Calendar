using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DayGuideView : MonoBehaviour, ISelectHandler
{

    public Text Time, Details;
    public int id;
    private NewEntry guide;

    public void Allocate(NewEntry n)
    {
        guide = n;
        SetTime(n.attributes[0] + " - " + n.attributes[1]);
        if (!n.filler)
            SetDetails(n.attributes[2] + ", #" + n.attributes[3] + ", " + n.attributes[7]);
    }

    public void SetTime(string s) {
        Time.text = s;
    }

    public void SetDetails(string s) {
        Details.text = s;
    }

    public void OnSelect(BaseEventData eventData)
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(guide);
    }

}
