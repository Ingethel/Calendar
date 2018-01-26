using System.Collections.Generic;

public class NewEntryPanelHandler : ItemPanel<Event> {

    public IAvailableSlotHandler timeGroup, colorGroup;
    public TimeValidator timeValidator;

    public override void PreviewEntry(Event n)
    {
        base.PreviewEntry(n);
        OnValidDate();
    }
    
    public override void SetLanguage()
    {
        setTitle();

        fields[0].label.text = gManager.language.Date;
        fields[1].label.text = gManager.language.Time;
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
        setTitle();

        dateValidator.SetDate(new int[] { item.day, item.month, item.year });
        
        timeValidator.SetStartTime(item.startTime);
        timeValidator.SetEndTime(item.endTime);

        DataGroup dG;
        if (item.filler)
        {
            dG = SettingsManager.GetDataGroup((int)DataGroup.DataGroups.EVENT)[0];
        }
        else
        {
            dG = SettingsManager.GetDataGroupID(DataGroup.DataGroups.EVENT, item.dataGroupID);
        }
        ResetAttibutes(dG);
    }

    protected override void SaveInfo()
    {
        base.SaveInfo();
        List<string> atts = new List<string>();
        AttributeElement[] elements = GetComponentsInChildren<AttributeElement>();
        foreach (AttributeElement element in elements)
            atts.Add(element.value.text);
        atts.Insert(0, timeValidator.GetStartTime());
        atts.Insert(0, timeValidator.GetEndTime());
        //item = new Event(
        //    dateValidator.GetDate(),
        //    dataGroup.Name,
        //    color.Name, 
        //    atts);
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
