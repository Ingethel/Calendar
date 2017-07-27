using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IItemListView : MonoBehaviour
{

    public Text Date, Time, Details;
    protected NewEntry guide;

    public virtual void Allocate(NewEntry n)
    {
        guide = n;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(guide);
    }

    public virtual void OnClick()
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(guide);
    }

    protected void SetTime(string s)
    {
        Time.text = s;
    }

    protected void SetDetails(string s)
    {
        Details.text = s;
    }

    protected void SetDate(string s)
    {
        Date.text = s;
    }
}
