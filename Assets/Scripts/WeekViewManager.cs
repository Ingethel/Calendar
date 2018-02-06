using UnityEngine;
using UnityEngine.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class WeekViewManager : IViewManager
{
    DayOfWeek[] days;
    public GameObject signPanel;
    public GameObject titlePanel;

    protected override void Awake()
    {
        base.Awake();
        days = GetComponentsInChildren<DayOfWeek>();
        signPanel.SetActive(false);
        titlePanel.SetActive(false);
    }
    
    void Start()
    {
        gManager.PrintMode += PrintMode;
    }
    
    public void PrintMode()
    {
        signPanel.SetActive(!signPanel.activeSelf);
        titlePanel.SetActive(!signPanel.activeSelf);
        Text[] signs = signPanel.GetComponentsInChildren<Text>();
        signs[0].text = gManager.language.ChiefOfMuseum;
        signs[1].text = gManager.language.NavalOfficer;
        Text titleValue = titlePanel.GetComponentInChildren<Text>();
        titleValue.text = gManager.language.WeeklyGuideSchedule;
        header.enabled = !header.enabled;
    }
    
    protected override void SetHeader()
    {
        header.text = gManager.language.GetMonth(assignedDate.Month - 1) + " " + assignedDate.Year.ToString();
    }

    protected override void OnSetView()
    {
        assignedDate = assignedDate.AddDays(-(byte)assignedDate.DayOfWeek + 1);
        for (int i = 0; i < 7; i++)
            days[i].SetView(assignedDate.AddDays(i));
    }

    public override void SetLanguage()
    {
        SetHeader();
    }

    public override void RequestLegacyData()
    {
        for (int i = 0; i < 7; i++)
            days[i].RequestLegacyData();
    }

    public override void GenerateReport()
    {
        ReportGeneration.CreatePDF(SettingsManager.Read("ExportPath") + "/WeeklyReport.pdf", true);
        ReportGeneration.OpenDoc();

        ReportGeneration.AddTitle(gManager.language.WeeklyGuideSchedule);
        ReportGeneration.AddEmptyLines(2);
        {
            PdfPTable table = new PdfPTable(5)
            {
                WidthPercentage = 100,
            };
            table.SetWidths(new float[] { 1, 1, 3, 1, 1 });
            table.AddCell(ReportGeneration.AddCell(gManager.language.Date, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
            table.AddCell(ReportGeneration.AddCell(gManager.language.Time, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
            table.AddCell(ReportGeneration.AddCell(gManager.language.Details, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
            table.AddCell(ReportGeneration.AddCell(gManager.language.TourGuies, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
            table.AddCell(ReportGeneration.AddCell(gManager.language.OfficerOnDuty, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
            foreach (DayOfWeek day in days)
            {
                NewEntryList list = day.GetEvents();
                Event e;
                for (int i = 0; i < list.Count(); i++)
                {
                    if(i == 0)
                        table.AddCell(ReportGeneration.AddCell(day.header.text, 1, list.Count(), Element.ALIGN_CENTER, ReportGeneration.boldFont));
                    if (list.TryGet(i, out e))
                    {
                        table.AddCell(ReportGeneration.AddCell(e.startTime + " - " + e.endTime, 1, Element.ALIGN_CENTER, ReportGeneration.normalFont));
                        table.AddCell(ReportGeneration.AddCell(e.ToString(), 1, Element.ALIGN_CENTER, ReportGeneration.normalFont));
                    }
                    if (i == 0)
                    {
                        table.AddCell(ReportGeneration.AddCell(day.Guides.text, 1, list.Count(), Element.ALIGN_CENTER, ReportGeneration.boldFont));
                        table.AddCell(ReportGeneration.AddCell(day.AF.text, 1, list.Count(), Element.ALIGN_CENTER, ReportGeneration.boldFont));
                    }
                }
                if(list == null || list.Count() == 0)
                {
                    table.AddCell(ReportGeneration.AddCell(day.header.text, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
                    table.AddCell(ReportGeneration.AddCell("", 1, Element.ALIGN_CENTER, ReportGeneration.normalFont));
                    table.AddCell(ReportGeneration.AddCell("", 1, Element.ALIGN_CENTER, ReportGeneration.normalFont));
                    table.AddCell(ReportGeneration.AddCell(day.Guides.text, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
                    table.AddCell(ReportGeneration.AddCell(day.AF.text, 1, Element.ALIGN_CENTER, ReportGeneration.boldFont));
                }
            }
            ReportGeneration.AddElement(table);
            ReportGeneration.AddEmptyLines(3);
            ReportGeneration.AddElement(new Paragraph(gManager.language.ChiefOfMuseum + "                                                      " + gManager.language.NavalOfficer, ReportGeneration.titleFont));
        }
        ReportGeneration.CloseDoc();
    }

}
