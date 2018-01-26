using UnityEngine;

public class IAvailableSlotHandler : MonoBehaviour {

    public GameObject slotItem;
    public GameObject slotContainer;
    protected int slotterTranslation;
    
    protected void Start()
    {
        slotContainer.SetActive(false);
    }

    public virtual void Clear()
    {
        slotContainer.GetComponent<RectTransform>().localPosition -= (new Vector3(0, slotterTranslation, 0));
        foreach (Transform t in slotContainer.transform)
            Destroy(t.gameObject);
        slotContainer.SetActive(false);
    }

    protected void Spawn(string s)
    {
        GameObject o = Instantiate(slotItem);
        o.transform.SetParent(slotContainer.transform);
        o.transform.localScale = Vector3.one;
        o.GetComponentInChildren<AvailableSlotItem>().Assign(s);
    }

    protected virtual void SetData()
    {
        slotContainer.SetActive(true);
    }
    
    public virtual void onClick()
    {
        SetData();
    }

    public virtual void onSet(string s)
    {
        Clear();
    }

    public virtual string GetValue()
    {
        return "";
    }

    public virtual void SetActive(bool b) {}

}
