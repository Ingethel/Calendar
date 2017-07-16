using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DayOfMonth : IViewManager, ISelectHandler {

    public GameObject DayPanel;
    Selectable selectable;

    private void Start()
    {
        selectable = GetComponent<Selectable>();
    }

    public void Reset()
    {
        DayPanel.SetActive(false);
        selectable.interactable = false;
    }

    protected override void SetHeader()
    {
        header.text = assignedDate.Day.ToString();
    }

    protected override void OnSetView() {
        RequestData();
        DayPanel.SetActive(true);
        selectable.interactable = true;
    }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }

    public override void RequestView()
    {
        manager.RequestView(assignedDate, Manager.ViewState.DAILY);
    }

    public void OnSelect(BaseEventData eventData)
    {
        RequestView();
    }
}
