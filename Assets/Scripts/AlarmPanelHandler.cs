public class AlarmPanelHandler : ItemPanel<Alarm> {

    protected override int attributeIntent
    { get { return 1; } }
    
    protected override void CalendarRequestOnSave()
    {
        calendarController.RefreshView();
    }

    protected override bool CheckSaveEligibility()
    {
        if (dateValidator.Validate())
            return true;
        return false;
    }

    protected override void SaveInfo()
    {
        base.SaveInfo();        
        item = new Alarm(dateValidator.GetDate(), eventGroup.GetValue(), attributeValues);
    }

    protected override void setTitle()
    {
        title.text = flag ? gManager.language.NewAlarm : gManager.language.AlarmPreview;
    }

}
