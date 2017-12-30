using UnityEngine;

public class ColourGroupElement : MonoBehaviour {

    public UnityEngine.UI.Image image;
    public UnityEngine.UI.InputField ifield;
    
    public void Assign(Color c, string s)
    {
        image.color = c;
        ifield.text = s;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
