public class WeekViewManager : IViewManager
{
    DayOfWeek[] days;

    protected override void Awake()
    {
        base.Awake();
        days = GetComponentsInChildren<DayOfWeek>();
        if (!gManager)
            gManager = FindObjectOfType<GameManager>();

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

}
