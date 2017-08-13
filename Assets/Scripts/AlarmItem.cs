public class AlarmItem : IItemListView<Alarm>
{
    public override void Allocate(Alarm n)
    {
        base.Allocate(n);
        SetDetails(n.attributes[0]);
    }

    public override void OnClick()
    {
        FindObjectOfType<ExtrasViewController>().RequestEntryPreview(item, ExtrasViewController.State.ALARM);
    }

    public void ReportAlarmOnClick()
    {
        CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
        if (viewController)
            viewController.RequestView(CalendarViewController.State.REPORT);
            
        ExtrasViewController exrasController = FindObjectOfType<ExtrasViewController>();
        if (exrasController)
            exrasController.RequestView(ExtrasViewController.State.ILLEGAL); 
    }
}
