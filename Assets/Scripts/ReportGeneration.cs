using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public class ReportGeneration : MonoBehaviour {
    static Document doc;

    static string combineStr = Path.Combine(Application.dataPath + "/Plugins", "Arial.ttf");
    static BaseFont bf = BaseFont.CreateFont(combineStr, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

    static iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 16f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    static iTextSharp.text.Font normalFont = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    static iTextSharp.text.Font boldFont = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    static iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bf, 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    static iTextSharp.text.Font tableBoldFont = new iTextSharp.text.Font(bf, 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

    static int numberedIntent = 0;

    private static Paragraph AddEmptyLines(int n_lines)
    {
        Paragraph empty = new Paragraph();
        for (int i = 0; i < n_lines; i++)
        {
            empty.Add(new Paragraph(" "));
        }
        return empty;
    }

    static void AddTitle(string s)
    {
        Paragraph p = new Paragraph(new Paragraph(s, titleFont))
        {
            Alignment = Element.ALIGN_CENTER,
        };
        doc.Add(p);
    }

    static void AddNumberedIntent(string s)
    {
        numberedIntent++;
        Paragraph p = new Paragraph(new Paragraph(numberedIntent.ToString() + ". " + s, boldFont));
        doc.Add(p);
    }

    static PdfPCell AddCell(string s, int colSpan, int align, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(s, font))
        {
            Colspan = colSpan,
            HorizontalAlignment = align
        };
        return cell;
    }

    static PdfPCell AddCell(string s, int colSpan, int rowSpan, int align, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(s, font))
        {
            Colspan = colSpan,
            Rowspan = rowSpan,
            HorizontalAlignment = align
        };
        return cell;
    }

    static void AddVisitorReportTable()
    {
        doc.Add(AddEmptyLines(1));
        PdfPTable table = new PdfPTable(6)
        {
            WidthPercentage = 100,
        };
        table.SetWidths(new float[] { 1, .5f, .5f, .1f, 1, .5f });
        table.AddCell(AddCell("ΑΝΑΦΟΡΑ ΕΠΙΣΚΕΠΤΩΝ      ΤΡΙΜΗΝΟΥ      ", 6, Element.ALIGN_CENTER, boldFont));
        table.AddCell(AddCell(" ", 6, 1, tableFont));
        table.AddCell(AddCell("ΕΠΙΣΚΕΠΤΕΣ ΜΕ ΜΕΙΩΜΕΝΟ ΕΙΣΙΤΗΡΙΟ", 3, Element.ALIGN_CENTER, boldFont));
        table.AddCell(AddCell("", 1, 17, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΕΠΙΣΚΕΠΤΕΣ ΜΕ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 2, Element.ALIGN_CENTER, boldFont));
        table.AddCell(AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΕΙΣΠΡΑΞΕΙΣ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΦΟΙΤΗΤΕΣ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("1", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("2", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΝΗΠΕΙΑΓΩΓΕΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("3", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("6 - 18 ΕΤΩΝ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("4", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("5", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΔΗΜΟΤΙΚΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("6", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΑΝΩ ΤΩΝ 65", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("7", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("8", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΓΥΜΝΑΣΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("9", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΜΕΙΩΜΕΝΟ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("10", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("11", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΛΥΚΕΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("12", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell(" ", 3, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΕΚΠΕΔΕΥΤΙΚΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("13", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΕΠΙΣΚΕΠΤΕΣ ΜΕ ΚΑΝΟΝΙΚΟ ΕΙΣΙΤΗΡΙΟ", 3, Element.ALIGN_CENTER, boldFont));
        table.AddCell(AddCell("ΑΜΕΑ ΚΑΙ ΣΥΝΟΔΟΙ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("14", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΚΑΤΗΓΟΡΙΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΑΤΟΜΑ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΕΙΣΠΡΑΚΣΕΙΣ", 1, Element.ALIGN_CENTER, tableBoldFont));
        table.AddCell(AddCell("ΚΑΤΩ ΤΩΝ 6", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("15", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΠΟΛΙΤΕΣ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("16", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("17", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΠΟΛΥΤΕΚΝΟΙ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("18", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΞΕΝΟΙ ΠΟΛΙΤΕΣ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("19", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("20", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΤΡΑΤΙΩΤΙΚΟΙ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("21", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΚΑΝΟΝΙΚΟ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("22", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("23", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΑΝΕΡΓΟΙ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("24", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell(" ", 3, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΕΚΔΗΛΩΣΕΙΣ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("26", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΙΚΟ", 3, Element.ALIGN_CENTER, boldFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, tableFont));
        //table.AddCell(AddCell("27", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΜΕ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, tableBoldFont));
        //table.AddCell(AddCell("28", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("29", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell(" ", 2, 3, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΠΙΣΚΕΠΤΩΝ ΧΩΡΙΣ ΕΙΣΙΤΗΡΙΟ", 1, Element.ALIGN_LEFT, tableBoldFont));
        //table.AddCell(AddCell("30", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("31", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        table.AddCell(AddCell("ΣΥΝΟΛΟ ΕΙΣΠΡΑΞΕΩΝ", 1, Element.ALIGN_LEFT, tableBoldFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        //table.AddCell(AddCell("32", 1, Element.ALIGN_RIGHT, tableFont));
        table.AddCell(AddCell(" ", 1, Element.ALIGN_CENTER, tableFont));
        doc.Add(table);
    }

    public static void CreatePDF(string filepath, string[] data)
    {
        doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(filepath + "/SemesterReport.pdf", FileMode.Create));
        doc.Open();

        doc.NewPage();
        AddTitle("Τριμηνιαία αναφορά στοιχείων απολογισμού δραστηριοτήτων Θωρηκτού «Γ. ΑΒΕΡΩΦ»");
        doc.Add(AddEmptyLines(2));
        AddNumberedIntent("ΕΠΙΣΚΕΨΕΙΣ ΟΜΑΔΙΚΕΣ-ΙΔΙΩΤΩΝ");
        AddVisitorReportTable();
        doc.Add(AddEmptyLines(1));
        AddNumberedIntent("ΑΙΜΟΔΟΣΙΕΣ ΠΡΟΣΩΠΙΚΟΥ");
        doc.Add(AddEmptyLines(3));
        AddNumberedIntent("ΣΥΜΜΕΤΟΧΗ ΑΝΤΙΠΡΟΣΩΠΕΙΩΝ ΣΕ ΤΟΠΙΚΕΣ ΕΚΔΗΛΩΣΕΙΣ");
        doc.Add(AddEmptyLines(3));
        AddNumberedIntent("ΔΙΑΘΕΣΗ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ» ΓΙΑ ΚΟΙΝΩΝΙΚΕΣ ΕΚΔΗΛΩΣΕΙΣ");
        doc.Add(AddEmptyLines(3));
        AddNumberedIntent("ΠΟΛΙΤΙΣΤΙΚΕΣ ΔΡΑΣΤΗΡΙΟΤΗΤΕΣ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ»");
        doc.Add(AddEmptyLines(3));
        AddNumberedIntent("ΓΕΝΙΚΗ ΕΠΙΘΕΩΡΗΣΗ ΘΩΡΗΚΤΟΥ «Γ.ΑΒΕΡΩΦ»");
        doc.Close();
    }
}
