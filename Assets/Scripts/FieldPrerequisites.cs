using UnityEngine;
using UnityEngine.UI;

public class FieldPrerequisites : MonoBehaviour {

    public InputField[] fields;
    InputField myField;

    void Start()
    {
        myField = GetComponent<InputField>();
    }

    public void SetValue(float i)
    {
        float sum = 0f;
        foreach(InputField field in fields)
        {
            float ivalue = 0;
            if (float.TryParse(field.text, out ivalue))
                sum += ivalue;
        }
        myField.text = (sum *  i).ToString();
    }
}
