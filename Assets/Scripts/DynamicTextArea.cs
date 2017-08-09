using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        panelRectTr = list.GetComponent<RectTransform>();
        panelSizeFitter = list.GetComponent<ContentSizeFitter>();
        extenderButton.gameObject.SetActive(false);
    }

    public void UpdateText()
    {
        InputField[] inputs = list.GetComponentsInChildren<InputField>();
        foreach (InputField input in inputs)
        {

        }
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
    }

    public void Collapse()
    {
        if (panelSizeFitter.enabled)
        {
            Vector3 temp = panelRectTr.localPosition;
            panelSizeFitter.enabled = false;
            panelRectTr.sizeDelta = new Vector2(0, 30);
            panelRectTr.localPosition = temp;
            extenderButton.image.sprite = extend;
        }
        else
        {
            panelSizeFitter.enabled = true;
            extenderButton.image.sprite = collapse;
        }
    }
}
