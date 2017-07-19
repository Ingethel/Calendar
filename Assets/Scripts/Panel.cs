using UnityEngine;

public class Panel : MonoBehaviour {

    protected Manager manager;

    protected virtual void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    
    public void Kill()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    protected virtual void KeybordInputHandler()
    {

    }
}
