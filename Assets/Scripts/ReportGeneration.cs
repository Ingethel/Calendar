using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public static class ReportGeneration {
    static Document doc;
#if UNITY_EDITOR
    static string combineStr = Path.Combine(Application.dataPath + "/StreamingAssets", "Arial.ttf");
#else
    static string combineStr = Path.Combine(Application.streamingAssetsPath, "Arial.ttf");
#endif
    static BaseFont bf = BaseFont.CreateFont(combineStr, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

    public static iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 16f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public static iTextSharp.text.Font normalFont = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    public static iTextSharp.text.Font boldFont = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public static iTextSharp.text.Font tableFont = new iTextSharp.text.Font(bf, 8f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    public static iTextSharp.text.Font tableBoldFont = new iTextSharp.text.Font(bf, 10f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

    static int numberedIntent = 0;
    
    public static void AddEmptyLines(int n_lines)
    {
        Paragraph empty = new Paragraph();
        for (int i = 0; i < n_lines; i++)
        {
            empty.Add(new Paragraph(" "));
        }
        doc.Add(empty);
    }

    public static void AddElement(IElement e)
    {
        doc.Add(e);
    }

    public static void AddTitle(string s)
    {
        Paragraph p = new Paragraph(new Paragraph(s, titleFont))
        {
            Alignment = Element.ALIGN_CENTER,
        };
        doc.Add(p);
    }

    public static void AddNumberedIntent(string s)
    {
        numberedIntent++;
        Paragraph p = new Paragraph(new Paragraph(numberedIntent.ToString() + ". " + s, boldFont));
        doc.Add(p);
    }

    public static PdfPCell AddCell(string s, int colSpan, int align, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(s, font))
        {
            Colspan = colSpan,
            HorizontalAlignment = align,
            VerticalAlignment = Element.ALIGN_MIDDLE
        };
        return cell;
    }

    public static PdfPCell AddCell(string s, int colSpan, int rowSpan, int align, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(s, font))
        {
            Colspan = colSpan,
            Rowspan = rowSpan,
            HorizontalAlignment = align,
            VerticalAlignment = Element.ALIGN_MIDDLE
        };
        return cell;
    }
    
    public static void OpenDoc()
    {
        doc.Open();
        doc.NewPage();
    }

    public static void CloseDoc()
    {
        doc.Close();
    }

    public static void CreatePDF(string filepath, bool landscape)
    {
        numberedIntent = 0;
        doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
        if(landscape)
            doc.SetPageSize(PageSize.A4.Rotate());
    }

}