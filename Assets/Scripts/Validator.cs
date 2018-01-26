using UnityEngine;
using UnityEngine.UI;

public abstract class Validator : MonoBehaviour {

    public Image errorImg;
    Color cError = new Color(1, 0, 0, 0.4f);
    Color cNorm = new Color(1, 0, 0, 0);

    protected void Awake()
    {
        Refresh();
    }
    
    public abstract bool Validate();

    public void ShowError()
    {
        errorImg.color = cError;
    }

    public void Refresh()
    {
        errorImg.color = cNorm;
    }
}
