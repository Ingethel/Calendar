using UnityEngine;

public class Panel : MonoBehaviour {

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    protected virtual void KeybordInputHandler()
    {

    }
}
