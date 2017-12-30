using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SettingsManager{

    static XmlDocument doc;
    
    public static string Read(string id)
    {
        if (doc == null)
            doc = new XmlDocument();
        doc.Load(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        XmlElement entry = doc.GetElementById(id);
        return entry.InnerText;
    }

    public static float Read_i(string id)
    {
        if (doc == null)
            doc = new XmlDocument();
        doc.Load(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        XmlElement entry = doc.GetElementById(id);
        float i = 0;
        float.TryParse(entry.InnerText, out i);
        return i;
    }

    public static void Write(string id, string data)
    {
        if (doc == null)
            doc = new XmlDocument();
        doc.Load(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        XmlElement entry = doc.GetElementById(id);
        entry.InnerText = data;
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
    }

    public static void Write(string id, float data)
    {
        if (doc == null)
            doc = new XmlDocument();
        doc.Load(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
        XmlElement entry = doc.GetElementById(id);
        entry.InnerText = data.ToString();
        doc.Save(Path.Combine(Application.streamingAssetsPath, "Settings.xml"));
    }
    
}

public class ColorGroup
{
    Color colour;
    string name;
    string id;

    public ColorGroup(Color c, string s)
    {
        colour = c;
        name = s;
    }

}