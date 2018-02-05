using UnityEngine;
using UnityEngine.UI;

public class WeekViewManager : IViewManager
{
    DayOfWeek[] days;
    public GameObject signPanel;
    public GameObject titlePanel;

    protected override void Awake()
    {
        base.Awake();
        days = GetComponentsInChildren<DayOfWeek>();
        signPanel.SetActive(false);
        titlePanel.SetActive(false);
    }
    
    void Start()
    {
        gManager.PrintMode += PrintMode;
    }
    
    public void PrintMode()
    {
        signPanel.SetActive(!signPanel.activeSelf);
        titlePanel.SetActive(!signPanel.activeSelf);
        Text[] signs = signPanel.GetComponentsInChildren<Text>();
        signs[0].text = gManager.language.ChiefOfMuseum;
        signs[1].text = gManager.language.NavalOfficer;
        Text titleValue = titlePanel.GetComponentInChildren<Text>();
        titleValue.text = gManager.language.WeeklyGuideSchedule;
        header.enabled = !header.enabled;
    }
    
    protected override void SetHeader()
    {
        header.text = gManager.language.GetMonth(assignedDate.Month - 1) + " " + assignedDate.Year.ToString();
    }

    protected override void OnSetView()
    {
        assignedDate = assignedDate.AddDays(-(byte)assignedDate.DayOfWeek + 1);
        for (int i = 0; i < 7; i++)
            days[i].SetView(assignedDate.AddDays(i));
    }

    public override void SetLanguage()
    {
        SetHeader();
    }

    public override void RequestLegacyData()
    {
        for (int i = 0; i < 7; i++)
            days[i].RequestLegacyData();
    }
}
