using UnityEngine;
using UnityEngine.UI;

public class SettingsField : MonoBehaviour {

    public enum ValueType { INT, FLOAT, STRING };
    public string id;
    public ValueType valueType;
    InputField field;    
    GameManager gmanager;

    public void OnStart()
    {
        if (!gmanager)
            gmanager = FindObjectOfType<GameManager>();
        if (!field)
            field = GetComponent<InputField>();

        switch (valueType)
        {
            case ValueType.INT:
                field.text = PlayerPrefs.GetInt(id).ToString();
                break;
            case ValueType.FLOAT:
                field.text = PlayerPrefs.GetFloat(id).ToString();
                break;
            case ValueType.STRING:
            default:
                field.text = PlayerPrefs.GetString(id);
                break;
        }
    }

    public void OnEndEdit()
    {
        gmanager.Command(new string[] { "", id, field.text });
    }
    
}
