using UnityEngine.UI;

public class NewEntryPanelHandler : ItemPanel<Event> {

    public IAvailableSlotHandler timeGroup, colorGroup;
    public TimeValidator timeValidator;
    public Text timeLabel;

    protected override int attributeIntent
    { get{ return 4; } }
    
    public override void PreviewEntry(Event n)
    {
        base.PreviewEntry(n);
        OnValidDate();
    }
    
    public override void SetLanguage()
    {
        base.SetLanguage();
        timeLabel.text = gManager.language.Time;
    }

    protected override void CalendarRequestOnSave()
    {
        calendarController.RequestView(CalendarViewController.State.DAILY, new System.DateTime(item.year, item.month, item.day));
    }

    protected override bool CheckSaveEligibility()
    {
        if (dateValidator.Validate() && timeValidator.Validate())
            return true;
        return false;
    }
    
    protected override void DisplayInfo()
    {
        base.DisplayInfo();
        timeValidator.SetStartTime(item.startTime);
        timeValidator.SetEndTime(item.endTime);
        colorGroup.onSet(item.color);
    }

    protected override void SaveInfo()
    {
        base.SaveInfo();
        attributeValues.Insert(0, timeValidator.GetEndTime());
        attributeValues.Insert(0, timeValidator.GetStartTime());
        item = new Event(dateValidator.GetDate(), eventGroup.GetValue(), colorGroup.GetValue(), attributeValues);
    }

    protected override void setTitle()
    {
        title.text = flag ? gManager.language.NewEntry : gManager.language.NewEntryPreview;
    }
    
    public override void EditEntry()
    {
        base.EditEntry();
        OnValidDate();
    }
    
    public void OnValidDate()
    {
        timeGroup.SetActive(dateValidator.Validate());
    }

}
