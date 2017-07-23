using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour {
    
    public GameObject[] viewModes;
    protected IViewManager viewManager;
    protected GameObject currentView;
    protected int currentViewIndex;
    
	protected virtual void Start () {
        currentViewIndex = -1;
        foreach (GameObject o in viewModes)
            o.SetActive(false);
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

    public void SetAsBackground()
    {
        if(currentView != null)
        {
            GraphicRaycaster r = currentView.GetComponent<GraphicRaycaster> ();
            if (r)
                r.enabled = !r.enabled;
        }
    }
}
