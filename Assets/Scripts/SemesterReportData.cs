using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf;

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
        ReportGeneration.CreatePDF(SettingsManager.Read("ExportPath") + "/SemesterReport.pdf", false);
        ReportGeneration.OpenDoc();
        ComposeReport();
        ReportGeneration.CloseDoc();
        
        CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
        viewController.RequestView(CalendarViewController.State.MONTHLY);
    }

    private void ComposeReport()
    {
        // title
        ReportGeneration.AddTitle("Τριμηνιαία αναφορά στοιχείων απολογισμού δραστηριοτήτων Θωρηκτού «Γ. ΑΒΕΡΩΦ»");
        ReportGeneration.AddEmptyLines(2);
        
        // visitors table
        ReportGeneration.AddNumberedIntent("ΕΠΙΣΚΕΨΕΙΣ ΟΜΑΔΙΚΕΣ-ΙΔΙΩΤΩΝ");
        ReportGeneration.AddEmptyLines(1);
        ReportGeneration.AddElement(VisitorReportTable());

        // rest
        ReportGeneration.AddNumberedIntent("ΑΙΜΟΔΟΣΙΕΣ ΠΡΟΣΩΠΙΚΟΥ");
        ReportGeneration.AddElement(new Paragraph(data[33], ReportGeneration.normalFont));
        ReportGeneration.AddNumberedIntent("ΣΥΜΜΕΤΟΧΗ ΑΝΤΙΠΡΟΣΩΠΕΙΩΝ ΣΕ ΤΟΠΙΚΕΣ ΕΚΔΗΛΩΣΕΙΣ");
        ReportGeneration.AddElement(new Paragraph(data[34], ReportGeneration.normalFont));
        ReportGeneration.AddNumberedIntent("ΔΙΑΘΕΣΗ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ» ΓΙΑ ΚΟΙΝΩΝΙΚΕΣ ΕΚΔΗΛΩΣΕΙΣ");
        ReportGeneration.AddElement(new Paragraph(data[35], ReportGeneration.normalFont));
        ReportGeneration.AddNumberedIntent("ΠΟΛΙΤΙΣΤΙΚΕΣ ΔΡΑΣΤΗΡΙΟΤΗΤΕΣ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ»");
        ReportGeneration.AddElement(new Paragraph(data[36], ReportGeneration.normalFont));
        ReportGeneration.AddNumberedIntent("ΓΕΝΙΚΗ ΕΠΙΘΕΩΡΗΣΗ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ»");
        ReportGeneration.AddElement(new Paragraph(data[37], ReportGeneration.normalFont));
    }

    public PdfPTable VisitorReportTable()
    {
        PdfPTable table = new PdfPTable(6)
        {
            WidthPercentage = 100,
        };
        table.SetWidths(new float[] { 1, .5f, .5f, .1f, 1, .5f });
        table.AddCell(ReportGeneration.AddCell("ΑΝΑΦΟΡΑ ΕΠΙΣΚΕΠΤΩΝ " + data[0] + " ΤΡΙΜΗΝΟΥ " + data[1], 6, Element.ALIGN_CENTER, ReportGeneration.boldFont));
        table.AddCell(ReportGeneration.AddCell(" ", 6, 1, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΕΠΙΣΚΕΠΤΕΣ ΜΕ ΜΕΙΩΜΕΝΟ ΕΙΣΙΤΗΡΙΟ", 3, Element.ALIGN_CENTER, ReportGeneration.boldFont));
        table.AddCell(ReportGeneration.AddCell("", 1, 17, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΕΠΙΣΚΕΠΤΕΣ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 2, Element.ALIGN_CENTER, ReportGeneration.boldFont));
        table.AddCell(ReportGeneration.AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΕΙΣΠΡΑΞΕΙΣ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΦΟΙΤΗΤΕΣ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[2], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[3], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΝΗΠΕΙΑΓΩΓΕΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[4], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("6 - 18 ΕΤΩΝ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[5], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[6], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΔΗΜΟΤΙΚΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[7], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΑΝΩ ΤΩΝ 65", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[8], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[9], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΓΥΜΝΑΣΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[10], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΜΕΙΩΜΕΝΟ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[11], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[12], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΛΥΚΕΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[13], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(" ", 3, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΕΚΠΕΔΕΥΤΙΚΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[14], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΕΠΙΣΚΕΠΤΕΣ ΜΕ ΚΑΝΟΝΙΚΟ ΕΙΣΙΤΗΡΙΟ", 3, Element.ALIGN_CENTER, ReportGeneration.boldFont));
        table.AddCell(ReportGeneration.AddCell("ΑΜΕΑ ΚΑΙ ΣΥΝΟΔΟΙ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[15], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΕΙΣΠΡΑΚΣΕΙΣ", 1, Element.ALIGN_CENTER, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell("ΚΑΤΩ ΤΩΝ 6", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[16], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΠΟΛΙΤΕΣ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[17], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[18], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΠΟΛΥΤΕΚΝΟΙ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[19], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΞΕΝΟΙ ΠΟΛΙΤΕΣ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[20], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[21], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΤΡΑΤΙΩΤΙΚΟΙ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[22], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΚΑΝΟΝΙΚΟ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[23], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[24], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΑΝΕΡΓΟΙ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[25], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(" ", 3, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΕΚΔΗΛΩΣΕΙΣ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[26], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΙΚΟ", 3, Element.ALIGN_CENTER, ReportGeneration.boldFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[27], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell(data[28], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[29], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(" ", 2, 3, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell(data[30], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[31], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell("ΣΥΝΟΛΟ ΕΙΣΠΡΑΞΕΩΝ", 1, Element.ALIGN_LEFT, ReportGeneration.tableBoldFont));
        table.AddCell(ReportGeneration.AddCell(" ", 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        table.AddCell(ReportGeneration.AddCell(data[32], 1, Element.ALIGN_CENTER, ReportGeneration.tableFont));
        return table;
    }
}
