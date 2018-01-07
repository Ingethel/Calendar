using UnityEngine;

public class IItemListView<T> : MonoBehaviour where T : Item
{

    public UnityEngine.UI.Text Date, Time, Details;
    protected T item;

    public virtual void Allocate(T n)
    {
        item = n;
    }
    
    public virtual void OnClick() {}

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
