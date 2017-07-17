public class WeekViewManager : IViewManager
{
    DayOfWeek[] days;

    void Start()
    {
        days = GetComponentsInChildren<DayOfWeek>();
    }

    protected override void SetHeader()
    {
        header.text = TimeConversions.GetMonth(assignedDate.Month - 1) + " " + assignedDate.Year.ToString();
    }

    protected override void OnSetView()
    {
        for (int i = 0; i < 7; i++)
            days[i].SetView(assignedDate.AddDays(i));
    }
}
