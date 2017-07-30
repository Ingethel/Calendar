using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour {
    
    public GameObject[] viewModes;
    protected IViewManager viewManager;
    protected GameObject currentView;
    protected int currentViewIndex;
    
    protected GameManager gManager;

    protected bool lockedAccess;
    
    protected virtual void Awake()
    {
        lockedAccess = false;
    }

	protected virtual void Start () {
        currentViewIndex = -1;
        foreach (GameObject o in viewModes)
            o.SetActive(false);
        gManager = FindObjectOfType<GameManager>();
    }

    protected void ChangeView(int i)
    {
            if (currentView != null)
                currentView.SetActive(false);

            if (i < viewModes.Length)
            {
                currentView = viewModes[i];
                currentView.SetActive(true);
            }
            currentViewIndex = i;
        
    }

    public virtual void CloseView(){}

    protected void SetActiveRaycast(bool flag)
    {
        if (currentView != null)
        {
            GraphicRaycaster r = currentView.GetComponent<GraphicRaycaster>();
            if (r)
                r.enabled = flag;
        }
    }

    public void SetAsBackground(bool flag)
    {
        if (!lockedAccess)
        {
            SetActiveRaycast(!flag);   
        }
        
    }
}
