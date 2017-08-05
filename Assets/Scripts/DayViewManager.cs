using UnityEngine;
using UnityEngine.UI;

public class DayViewManager : IViewManager {

    public Text[] headers;
    public GameObject weeklyButton;

    void Start()
    {
        weeklyButton.SetActive(false);
    }

    protected override void SetHeader() {
        int dayIndex = (byte)assignedDate.DayOfWeek - 1;
        if (dayIndex < 0) dayIndex = 6;
        header.text = gManager.language.GetDay(dayIndex) + " " + assignedDate.Day.ToString() + "/" + assignedDate.Month.ToString() + "/" + assignedDate.Year.ToString();
    }

    public override void Refresh()
    {
        base.Refresh();
        weeklyButton.GetComponentInChildren<Text>().text = gManager.language.WeeklyButton;
        weeklyButton.SetActive(false);
        headers[0].text = gManager.language.OfficerOnDuty;
        headers[1].GetComponentInParent<InputField>().text = "";
        headers[2].text = gManager.language.Time;
        headers[3].text = gManager.language.Details;
    }

    protected override void OnSetView()
    {
        RequestData();
        if (assignedDate.DayOfWeek == System.DayOfWeek.Monday) {
            weeklyButton.SetActive(true);
        }
        else
        {
            DisplayInfo();
        }
        headers[1].GetComponentInParent<InputField>().text = info.officer;
    }

    protected override void SetTag()
    {
        _tag = assignedDate.Day.ToString() + "." + assignedDate.Month.ToString() + "." + assignedDate.Year.ToString();
    }
    
    protected override void AssignInfo(GameObject o, NewEntry n)
    {
        DayGuideView o_view = o.GetComponent<DayGuideView>();
        if (o_view != null)
            o_view.Allocate(n);
    }

    protected override void DisplayInfo()
    {
        if (info == null || info.guides.Count() == 0)
        {
            info = new DAY();
            for (int i = 0; i < setTime.Length - 1; i++)
            {
                AddFiller(setTime[i], setTime[i + 1]);
            }
        }
        else {
            if (info.guides.Count() < setTime.Length - 1) {
                FillEmptySlots();
            }
        }
        base.DisplayInfo();
        if(info.events.Count > 0)
        {
            ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
            extras.RequestAlarmPreview(info.events);
        }
    }

    public void SetOfficerOnDuty()
    {
        dataManager.RequestWriteOfficer(_tag, headers[1].text);
    }

    public void OnClickWeeklyButton()
    {
        calendarController.RequestView(CalendarViewController.State.WEEKLY, assignedDate);
    }
}
