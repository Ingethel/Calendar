using System.Collections.Generic;
using UnityEngine.UI;

public class OptionsExpandedHandler : Panel
{
    public Dropdown viewModes;
    public Text[] labels;

    void Start()
    {
        gManager.OnLanguageChange += SetLanguage;
    }

    public override void SetLanguage()
    {
        int currentValue = viewModes.value;

        Dropdown.OptionData option1 = new Dropdown.OptionData(gManager.language.Monthly);
        Dropdown.OptionData option2 = new Dropdown.OptionData(gManager.language.Weekly);
        Dropdown.OptionData option3 = new Dropdown.OptionData(gManager.language.Daily);
        viewModes.ClearOptions();
        viewModes.AddOptions(new List<Dropdown.OptionData>() { option1, option2, option3 });
        viewModes.value = currentValue;

        labels[0].text = gManager.language.ViewBy;
        labels[1].text = gManager.language.Search;
        labels[2].text = gManager.language.AddGuide;
        labels[3].text = gManager.language.AddAlarm;
        labels[4].text = gManager.language.Print;
    }
}
