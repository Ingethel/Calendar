using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SettingsManager{

    static XmlDocument doc;
    static Dictionary<string, string> timetable;
    static List<ColorGroup> colorGroups;
    static List<List<DataGroup>> dataGroups;

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
        if (name == "")
            return colorGroups[0];
        else
            return colorGroups.Where(x => x.Name == name).DefaultIfEmpty(colorGroups[0]).First();
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

    private static List<DataGroup> ReadDataGroup(int i)
    {
        List<DataGroup> temp = new List<DataGroup>();
        LoadDoc();
        XmlNodeList list = doc.GetElementsByTagName(DataGroup.Groups[i]);
        foreach (XmlElement element in list)
        {
            temp.Add(new DataGroup(element.GetAttribute("name"), element.InnerText, element.GetAttribute("id"), i));
        }
        return temp;
    }

    public static void ReadDataGroups()
    {
        dataGroups = new List<List<DataGroup>>();
        for (int i = 0; i < DataGroup.Groups.Length; i++)
            dataGroups.Add(ReadDataGroup(i));
    }

    public static List<List<DataGroup>> GetDataGroups()
    {
        if (dataGroups == null)
            ReadDataGroups();
        return dataGroups;
    }

    public static List<DataGroup> GetDataGroup(int i)
    {
        if (dataGroups == null)
            ReadDataGroups();
        return dataGroups[i];
    }

    public static DataGroup GetDataGroupID(DataGroup.DataGroups group, string name)
    {
        if (dataGroups == null)
            ReadDataGroups();
        if (dataGroups[(int)group].Count == 0)
            CreateDataGroup("default", "Notes", (int)group);
        if (name == "")
            return dataGroups[(int)group][0];
        else
            return dataGroups[(int)group].Where(x => x.Name == name).DefaultIfEmpty(dataGroups[(int)group][0]).First();
    }

    public static DataGroup CreateDataGroup(string name, string attributeList, int value)
    {
        DataGroup dG = new DataGroup(name, attributeList, value);
        LoadDoc();
        XmlElement xmlDG = doc.CreateElement(DataGroup.Groups[value]);
        xmlDG.SetAttribute("id", dG.Id);
        xmlDG.SetAttribute("name", dG.Name);
        xmlDG.InnerText = dG.Attributes;
        XmlElement root = doc.DocumentElement;
        root.AppendChild(xmlDG);
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        dataGroups[value].Add(dG);
        return dG;
    }

    public static void DeleteDataGroup(DataGroup dG)
    {
        LoadDoc();
        XmlElement entry = doc.GetElementById(dG.Id);
        doc.DocumentElement.RemoveChild(entry);
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        dataGroups[dG.type].Remove(dG);
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

    public static string GenerateNewId()
    {
        int id = (int)ReadFloat("IDCounter");
        id += 1;
        Write("IDCounter", id);
        return "_entryNo." + id.ToString();
    }

}

public class ColorGroup
{
    public Color Colour { private set; get; }
    public string Name { private set; get; }
    public string Id { private set; get; }

    public ColorGroup(Color c, string s)
    {
        c.a = .7f;
        Colour = c;
        Name = s;
        Id = SettingsManager.GenerateNewId();
    }

    public ColorGroup(Color c, string s, string id)
    {
        c.a = .7f;
        Colour = c;
        Name = s;
        Id = id;
    }

}

public class DataGroup
{
    public enum DataGroups { Event, Alarm };
    public static string[] Groups = new string[] { "EventGroup", "AlarmGroup" };
    public int type;
    public string Name { private set; get; }
    public string Id { private set; get; }
    public string Attributes { private set; get; }

    public DataGroup(string name, string attributeList, int value)
    {
        Id = SettingsManager.GenerateNewId();
        Name = name;
        Attributes = attributeList;
        type = value;
    }

    public DataGroup(string name, string attributeList, string id, int value)
    {
        Id = id;
        Name = name;
        Attributes = attributeList;
        type = value;
    }

}