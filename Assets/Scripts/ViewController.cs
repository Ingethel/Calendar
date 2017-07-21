using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
