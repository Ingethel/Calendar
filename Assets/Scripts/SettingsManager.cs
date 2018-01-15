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

    public static float ReadFloat(string id)
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
            string[] parts = element.InnerText.Split(',');
            if (parts.Length == 3)
            {
                float r = 0, g = 0, b = 0;
                float.TryParse(parts[0], out r);
                float.TryParse(parts[1], out g);
                float.TryParse(parts[2], out b);
                colorGroups.Add(new ColorGroup(new Color(r, g, b), element.GetAttribute("name"), element.GetAttribute("id")));
            }
        }
    }

    public static ColorGroup[] GetColorGroups()
    {
        if (colorGroups == null)
            ReadColourGroups();
        return colorGroups.ToArray();
    }

    public static ColorGroup GetColourGroup(string name)
    {
        if (colorGroups == null)
            ReadColourGroups();
        if (colorGroups.Count == 0)
            CreateColorGroup(new Color(0, 39, 255), "default");
        return colorGroups.Where(x => x.Name == "name").DefaultIfEmpty(colorGroups[0]).First();
    }

    public static ColorGroup CreateColorGroup(Color c, string s)
    {
        ColorGroup cG = new ColorGroup(c, s);
        LoadDoc();
        XmlElement xmlCG = doc.CreateElement("ColorGroup");
        xmlCG.SetAttribute("id", cG.Id);
        xmlCG.SetAttribute("name", cG.Name);
        xmlCG.InnerText = cG.Colour.r.ToString() + "," + cG.Colour.g.ToString() + "," + cG.Colour.b.ToString();
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

    public static void ReadDataGroups()
    {

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
        Id = "ColorGroup." + _id.ToString();
        PlayerPrefs.SetInt("colorGroupId", _id);
    }

    public ColorGroup(Color c, string s, string id)
    {
        Colour = c;
        Name = s;
        Id = id;
    }

}

public class DataGroup
{
    public enum DataType { EVENT, ALARM };
    public DataType type;
    public string Name { private set; get; }
    public string Id { private set; get; }
    public string Attributes { private set; get; }

    protected DataGroup(string name, string attributeList, int value)
    {
        int _id = PlayerPrefs.GetInt("dataGroupId") + 1;
        PlayerPrefs.SetInt("dataGroupId", _id);
        if(value == 0)
        {
            type = DataType.EVENT;
            Id = "EventGroup." + _id.ToString();
        }
        else if(value == 1)
        {
            type = DataType.ALARM;
            Id = "AlarmGroup." + _id.ToString();
        }
        Name = name;
        Attributes = attributeList;
    }

    public static DataGroup CreateEventData(string name, string attributeList)
    {
        return new DataGroup(name, attributeList, 0);
    }

    public static DataGroup CreateAlarmData(string name, string attributeList)
    {
        return new DataGroup(name, attributeList, 1);
    }
}