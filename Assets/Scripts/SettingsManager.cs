public class SettingsManager : Panel {

    public override void Open()
    {
        base.Open();
        init();
    }

    void init()
    {
        SettingsField[] settings = GetComponentsInChildren<SettingsField>();
        foreach (SettingsField s in settings)
            s.OnStart();
    }

}
