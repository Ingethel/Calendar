using UnityEngine;
using UnityEngine.UI;

public class IItemListView<T> : MonoBehaviour where T : Item
{

    public Text Date, Time, Details;
    protected T item;

    public virtual void Allocate(T n)
    {
        item = n;
    }
    
    public virtual void OnClick() {}

    protected virtual void SetTime(string s)
    {
        Time.text = s;
    }

    protected virtual void SetDetails(string s)
    {
        Details.text = s;
    }

    protected virtual void SetDate(string s)
    {
        Date.text = s;
    }

}
