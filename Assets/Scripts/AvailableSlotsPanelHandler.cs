using UnityEngine;

public class AvailableSlotsPanelHandler : MonoBehaviour {

    public GameObject slotItem;
    int slotterTranslation;
    
    public void Clear()
    {
        GetComponent<RectTransform>().localPosition -= (new Vector3(0, slotterTranslation, 0));
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }

    public void SetData(string[] data)
    {
        int year, month, day;
        int.TryParse(data[0], out day);
        int.TryParse(data[1], out month);
        int.TryParse(data[2], out year);

        string[] slots = FindObjectOfType<CalendarViewController>().RequestEmptySlots(new System.DateTime(year, month, day));
        slotterTranslation = (slots.Length - 1) / 2 * -30;
        if (slots != null && slots.Length > 0)
        {
            foreach (string s in slots)
            {
                GameObject o = Instantiate(slotItem);
                o.transform.SetParent(transform);
                o.transform.localScale = Vector3.one;
                o.GetComponentInChildren<UnityEngine.UI.Text>().text = s;
            }
            GetComponent<RectTransform>().localPosition += (new Vector3(0, slotterTranslation, 0));
        }
    }
}
