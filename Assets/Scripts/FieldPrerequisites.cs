using UnityEngine;
using UnityEngine.UI;

public class FieldPrerequisites : MonoBehaviour {

    public InputField[] fields;
    InputField myField;

    void Start()
    {
        myField = GetComponent<InputField>();
    }

    public void SetValue(int i)
    {
        float price = 0;

        if (i == 1)
            price = 1;
        else if (i == 2)
            price = PlayerPrefs.GetFloat("ReducedTicketPrice", 1.5f);
        else if (i == 3)
            price = PlayerPrefs.GetFloat("TicketPrice", 3);

        float sum = 0f;
        foreach(InputField field in fields)
        {
            float ivalue = 0;
            if (float.TryParse(field.text, out ivalue))
                sum += ivalue;
        }
        myField.text = (sum *  price).ToString();
    }
}
