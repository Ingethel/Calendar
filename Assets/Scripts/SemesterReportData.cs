using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SemesterReportData : MonoBehaviour {

    InputField[] tableData;
    DynamicTextArea[] restData;
    string[] data;
    EventSystem system;

    void Start () {
        tableData = GetComponentsInChildren<InputField>().Where(o => o.transform.parent == transform).ToArray();
        restData = GetComponentsInChildren<DynamicTextArea>();
        system = EventSystem.current;
    }

    void Update()
    {
        KeybordInputHandler();
    }

    void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }
            }
        }
    }

    void ComposeData()
    {
        data = new string[tableData.Length + restData.Length];
        for (int i = 0; i < tableData.Length; i++)
            data[i] = tableData[i].text;
        for (int i = 0; i < restData.Length; i++)
            data[tableData.Length + i] = restData[i].GetText();
    }

    public void WriteReport()
    {
        GameManager gManager = FindObjectOfType<GameManager>();
        ComposeData();
        if (gManager)
        {
            ReportGeneration.CreatePDF(gManager.DESKTOP, data);
        }
    }
}
