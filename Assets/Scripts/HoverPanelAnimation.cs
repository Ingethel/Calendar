using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverPanelAnimation : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{

    public bool hoverActive;
    Image image;
    public Color32 normal, highlihted;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(hoverActive)
            image.color = highlihted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverActive)
            image.color = normal;
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (hoverActive) {
            IViewManager i_manager = GetComponent<IViewManager>();
            if (i_manager != null) {
                i_manager.RequestView();
            }
        }
    }

}
