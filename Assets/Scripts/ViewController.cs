using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour {
    
    public GameObject[] viewModes;
    protected IViewManager viewManager;
    protected Panel panel;
    protected GameObject currentView;
    protected int currentViewIndex;
    public bool InFront { protected set; get; }
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
        gManager.OnLanguageChange += SetLanguage;
    }
    
    protected void ChangeView(int i)
    {
        if (currentView != null)
        {
            panel = currentView.GetComponent<Panel>();
            if (panel)
                panel.Close();
            else
                currentView.SetActive(false);
        }
        
        if (i < viewModes.Length)
        {
            currentView = viewModes[i];
            panel = currentView.GetComponent<Panel>();
            if (panel)
                panel.Open();
            else
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
            {
                r.enabled = flag;
                InFront = flag;
            }
        }
    }

    public void SetAsBackground(bool flag)
    {
        if (!lockedAccess)
            SetActiveRaycast(!flag);
    }

    public virtual void NotifyIllegal(){}

    public virtual void SetLanguage(){}
}
