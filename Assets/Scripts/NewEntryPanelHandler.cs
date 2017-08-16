using UnityEngine;

public class NewEntryPanelHandler : ItemPanel<NewEntry> {

    public GameObject slotExpander;
    public GameObject slotPanel;
    public GameObject slotItem;
    public Sprite expand, collapse;

    public override void PreviewEntry(NewEntry n)
    {
        base.PreviewEntry(n);

        slotExpander.SetActive(false);
        slotPanel.SetActive(false);
    }
    
    public override void SetLanguage()
    {
        setTitle();
        fields[0].label.text = gManager.language.NameOfTeam;
        fields[1].label.text = gManager.language.NumberOfPeople;
        fields[2].label.text = gManager.language.PersonInCharge;
        fields[3].label.text = gManager.language.Telephone;
        fields[4].label.text = gManager.language.Date;
        fields[5].label.text = gManager.language.Time;
        fields[6].label.text = gManager.language.DateOfConfirmation;
        fields[7].label.text = gManager.language.Guide;
        fields[8].label.text = gManager.language.Notes;
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
        string[] split = item.attributes[0].Split(':');
        if(split.Length == 2)
        {
            fields[5].inputs[0].text = split[0];
            fields[5].inputs[1].text = split[1];
        }
        split = item.attributes[1].Split(':');
        if (split.Length == 2)
        {
            fields[5].inputs[2].text = split[0];
            fields[5].inputs[3].text = split[1];
        }
        fields[4].inputs[0].text = item.day != 0 ? item.day.ToString() : "";
        fields[4].inputs[1].text = item.month != 0 ? item.month.ToString() : "";
        fields[4].inputs[2].text = item.year != 0 ? item.year.ToString() : "";
        fields[0].inputs[0].text = item.attributes[2];
        fields[1].inputs[0].text = item.attributes[3];
        fields[2].inputs[0].text = item.attributes[4];
        fields[3].inputs[0].text = item.attributes[5];
        fields[6].inputs[0].text = item.attributes[6];
        fields[7].inputs[0].text = item.attributes[7];
        fields[8].inputs[0].text = item.attributes[8];
    }

    protected override void SaveInfo()
    {
        base.SaveInfo();
        string[] inputs = { fields[5].inputs[0].text + ":" + fields[5].inputs[1].text, fields[5].inputs[2].text + ":" + fields[5].inputs[3].text,
                TryGetText(fields[0].inputs[0]), TryGetText(fields[1].inputs[0]), TryGetText(fields[2].inputs[0]), TryGetText(fields[3].inputs[0]),
                TryGetText(fields[6].inputs[0]), TryGetText(fields[7].inputs[0]), TryGetText(fields[8].inputs[0])};
        item = new NewEntry(inputs, fields[4].inputs[0].text + "." + fields[4].inputs[1].text + "." + fields[4].inputs[2].text);
    }

    protected override void setTitle()
    {
        title.text = flag ? gManager.language.NewEntry : gManager.language.NewEntryPreview;
    }

    public void RequestAvalableSlots()
    {
        int year, month, day;
        int.TryParse(fields[4].inputs[0].text, out day);
        int.TryParse(fields[4].inputs[1].text, out month);
        int.TryParse(fields[4].inputs[2].text, out year);

        string[] slots = calendarController.RequestEmptySlots(new System.DateTime(year, month, day));
        if (slots != null && slots.Length > 0)
        {
            foreach(string s in slots)
            {
                GameObject o = Instantiate(slotItem);
                o.transform.SetParent(slotPanel.transform);
                o.transform.localScale = Vector3.one;
                o.GetComponentInChildren<UnityEngine.UI.Text>().text = s;
            }
            //slotPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, (slots.Length - 1) / 2 * -15, 0);
        }
        else
        {
            slotPanel.SetActive(false);
            slotExpander.SetActive(false);
        }
    }

    public void OnClickSlotExpander()
    {
        slotPanel.SetActive(!slotPanel.activeSelf);
        if (slotPanel.activeSelf)
        {
            slotExpander.GetComponent<UnityEngine.UI.Image>().sprite = collapse;
            RequestAvalableSlots();
        }
        else
        {
            slotExpander.GetComponent<UnityEngine.UI.Image>().sprite = expand;
        }
    }
    
    public void OnDateReady()
    {
        if (dateValidator.Validate())
        {
            slotExpander.SetActive(true);
            slotExpander.GetComponent<UnityEngine.UI.Image>().sprite = expand;
        }
    }
}