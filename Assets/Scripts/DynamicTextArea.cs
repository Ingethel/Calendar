using UnityEngine;
using UnityEngine.UI;

public class DynamicTextArea : MonoBehaviour {

    public Sprite extend, collapse;
    public Button extenderButton;
    public GameObject list;
    public GameObject listItem;
    int numberOfItems;

    ContentSizeFitter panelSizeFitter;
    RectTransform panelRectTr;

    string compactText;
    Vector3 initialPos;
    Vector3 expandedPos;

    private void Start()
    {
        panelRectTr = list.GetComponent<RectTransform>();
        initialPos = panelRectTr.localPosition;
        expandedPos = initialPos;
        panelSizeFitter = list.GetComponent<ContentSizeFitter>();
        extenderButton.gameObject.SetActive(false);
    }

    public string GetText()
    {
        int i = 0;
        string compactText = "";
        InputField[] inputs = list.GetComponentsInChildren<InputField>();
        foreach (InputField input in inputs)
        {
            string s;
            if(TryGetText(input, out s))
            {
                i++;
                compactText += i.ToString() + "  " + s + System.Environment.NewLine;
            }
        }
        if(i == 0)
        {
            compactText = "Ουδεμία για αυτό το τρίμηνο";
        }
        Debug.Log(compactText);
        return compactText;
    }

    bool TryGetText(InputField input, out string s)
    {
        if (input)
        {
            if(input.text != null && input.text != "" && input.text != " ")
            {
                s = input.text;
                return true;
            }
        }
        s = "";
        return false;
    }

    public void SpawnItem()
    {
        if (!panelSizeFitter.enabled)
            panelSizeFitter.enabled = true;
        if (!extenderButton.gameObject.activeSelf)
        {
            extenderButton.gameObject.SetActive(true);
        }
        extenderButton.image.sprite = collapse;
        numberOfItems++;
        GameObject o = Instantiate(listItem) as GameObject;
        o.transform.SetParent(list.transform);
        o.transform.localScale = Vector3.one;
        expandedPos += new Vector3(0, 15, 0);
        panelRectTr.localPosition = expandedPos;
    }

    public void Collapse()
    {
        if (panelSizeFitter.enabled)
        {
            panelSizeFitter.enabled = false;
            panelRectTr.sizeDelta = new Vector2(0, 30);
            panelRectTr.localPosition = initialPos;
            extenderButton.image.sprite = extend;
        }
        else
        {
            panelSizeFitter.enabled = true;
            panelRectTr.localPosition = expandedPos;
            extenderButton.image.sprite = collapse;
        }
    }
}
