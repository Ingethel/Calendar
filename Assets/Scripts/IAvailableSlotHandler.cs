using UnityEngine;

public class IAvailableSlotHandler : MonoBehaviour {

    public GameObject slotItem;
    protected int slotterTranslation;

    public void Clear()
    {
        GetComponent<RectTransform>().localPosition -= (new Vector3(0, slotterTranslation, 0));
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }

    protected void Spawn(string s)
    {
        GameObject o = Instantiate(slotItem);
        o.transform.SetParent(transform);
        o.transform.localScale = Vector3.one;
        o.GetComponentInChildren<AvailableSlotItem>().Assign(s);
    }

    public virtual void SetData() {}
    
}
