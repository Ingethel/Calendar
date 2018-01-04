using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SettingsManager{

    static XmlDocument doc;
    static Dictionary<string, string> timetable;
    static List<ColorGroup> colorGroups;

    private static void LoadDoc()
    {
        if (doc == null)
            doc = new XmlDocument();
        doc.Load(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
    }
    
    public static string Read(string id)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(id);
        return entry.InnerText;
    }

    public static float Read_i(string id)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(id);
        float i = 0;
        float.TryParse(entry.InnerText, out i);
        return i;
    }

    public static void Write(string id, string data)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(id);
        entry.InnerText = data;
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
    }

    public static void Write(string id, float data)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(id);
        entry.InnerText = data.ToString();
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
    }
    
    private static void ReadColourGroups()
    {
        LoadDoc();
        XmlNodeList list = doc.GetElementsByTagName("ColorGroup");
        colorGroups = new List<ColorGroup>();
        foreach (XmlElement element in list)
        {
            string[] parts = element.InnerText.Split(' ');
            if (parts.Length >= 4)
            {
                float r = 0, g = 0, b = 0;
                float.TryParse(parts[parts.Length - 1], out b);
                float.TryParse(parts[parts.Length - 2], out g);
                float.TryParse(parts[parts.Length - 3], out r);
                colorGroups.Add(new ColorGroup(new Color(r, g, b), parts[0], element.GetAttribute("id")));
            }
        }
    }

    public static ColorGroup[] GetColorGroups()
    {
        if (colorGroups == null)
            ReadColourGroups();
        return colorGroups.ToArray();
    }

    public static ColorGroup CreateColorGroup(Color c, string s)
    {
        ColorGroup cG = new ColorGroup(c, s);
        LoadDoc();
        XmlElement xmlCG = doc.CreateElement("ColorGroup");
        xmlCG.SetAttribute("id", cG.Id);
        xmlCG.InnerText = cG.Name + " " + cG.Colour.r.ToString() + " " + cG.Colour.g.ToString() + " " + cG.Colour.b.ToString();
        XmlElement root = doc.DocumentElement;
        root.AppendChild(xmlCG);
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        colorGroups.Add(cG);
        return cG;
    }

    public static void DeleteColourGroup(ColorGroup cG)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(cG.Id);
        doc.DocumentElement.RemoveChild(entry);
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        colorGroups.Remove(cG);
    }

    private static void ReadTimetable()
    {
        LoadDoc();
        XmlNodeList list = doc.GetElementsByTagName("Timetable");
        timetable = new Dictionary<string, string>();
        foreach (XmlElement element in list)
            timetable.Add(element.GetAttribute("id"), element.InnerText);
    }

    public static string GetTimetable(string day)
    {
        if (timetable == null)
            ReadTimetable();
        return timetable[day];
    }
}

public class ColorGroup
{
    public Color Colour { private set; get; }
    public string Name { private set; get; }
    public string Id { private set; get; }

    public ColorGroup(Color c, string s)
    {
        Colour = c;
        Name = s;
        int _id = PlayerPrefs.GetInt("colorGroupId") + 1;
        Id = "_colorGroup." + _id.ToString();
        PlayerPrefs.SetInt("colorGroupId", _id);
    }

    public ColorGroup(Color c, string s, string id)
    {
        Colour = c;
        Name = s;
        Id = id;
    }

}