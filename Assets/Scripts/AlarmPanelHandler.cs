public class AlarmPanelHandler : ItemPanel<Alarm> {

//    public Dropdown Modifier;
//    private int RepeatFreqModifier;

    public override void SetLanguage()
    {
        setTitle();
        fields[0].label.text = gManager.language.Date;
        fields[1].label.text = gManager.language.RepeatEvery;
        fields[2].label.text = gManager.language.Notes;
        /*Modifier.options[0].text = gManager.language.Days;
        Modifier.options[1].text = gManager.language.Weeks;
        Modifier.options[2].text = gManager.language.Months;*/
    }

    protected override void CalendarRequestOnSave()
    {
        calendarController.RefreshView();
    }

    protected override bool CheckSaveEligibility()
    {
        if (dateValidator.Validate() && fields[2].inputs[0].text != "")
            return true;
        return false;
    }

    protected override void DisplayInfo()
    {
        setTitle();
        fields[0].inputs[0].text = item.day != 0 ? item.day.ToString() : "";
        fields[0].inputs[1].text = item.month != 0 ? item.month.ToString() : "";
        fields[0].inputs[2].text = item.year != 0 ? item.year.ToString() : "";

        fields[2].inputs[0].text = item.attributes[0];
    }

    protected override void SaveInfo()
    {
        base.SaveInfo();

        /*      int temp;
                if (int.TryParse(fields[1].inputs[0].text, out temp))
                    item = new Alarm(fields[0].inputs[0].text + "." + fields[0].inputs[1].text + "." + fields[0].inputs[2].text, fields[2].inputs[0].text, temp, RepeatFreqModifier);
                else*/
        item = new Alarm(fields[0].inputs[0].text + "." + fields[0].inputs[1].text + "." + fields[0].inputs[2].text, fields[2].inputs[0].text);
    }

    protected override void setTitle()
    {
        title.text = flag ? gManager.language.NewAlarm : gManager.language.AlarmPreview;
    }
    /*
public void ChangeModifier(int i)
{
    RepeatFreqModifier = i;
}*/
}
