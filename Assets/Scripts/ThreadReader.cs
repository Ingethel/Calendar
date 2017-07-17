using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;

public class ThreadReader /*: ThreadJob*/ {
    
    private NewEntryList GetDayInfo(XmlElement day) {
        NewEntryList dayInfo = new NewEntryList();
        XmlNodeList entries = day.GetElementsByTagName(Strings.NewEntry);
        foreach (XmlNode entry in entries)
        {
            NewEntry newGuide = new NewEntry();
            XmlNodeList entryInfo = entry.ChildNodes;
            foreach (XmlElement element in entryInfo)
            {
                for (int i = 0; i < newGuide.attributes.Length; i++) {
                    if (element.Name == newGuide.labels[i])
                    {
                        newGuide.attributes[i] = element.InnerText;
                        break;
                    }
                }
            }
            newGuide.filler = false;
            dayInfo.Add(newGuide);
        }
        return dayInfo;
    }

    // Read By Day
    public NewEntryList Read(string filename, string ID) {
        NewEntryList dayInfo = new NewEntryList();
        if (File.Exists(filename)) {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement day = doc.GetElementById(ID);
            dayInfo = GetDayInfo(day);
        }
        return dayInfo;
    }

    // Read By Month
    public Dictionary<string, NewEntryList> Read(string filename)
    {
        Dictionary<string, NewEntryList> monthInfo = new Dictionary<string, NewEntryList>();
        if (File.Exists(filename))
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList entries = doc.GetElementsByTagName(Strings.Day);
            foreach(XmlElement day in entries)
            {
                string id = day.GetAttribute("id");
                monthInfo.Add(id, GetDayInfo(day));
            }
        }
        return monthInfo;
    }

    private string InitialiseDoc(string filePath)
    {
        filePath = Application.dataPath + @"/Calendar Data/Data/" + filePath;
        string filename;
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        filename = filePath + "/" + Strings.file;

        XmlDocument doc = new XmlDocument();

        if (!File.Exists(filename))
        {
            doc.Save(filename);
            XmlTextWriter writer = new XmlTextWriter(filename, null);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("Entries", null, null, "<!ELEMENT Entries ANY ><!ELEMENT Day ANY><!ELEMENT NewEntry ANY ><!ATTLIST Day id ID #REQUIRED>");
            writer.WriteStartElement("Entries");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
        return filename;
    }

    // write element
    public void Write(string filePath, string tag, NewEntry item) {
        string filename = InitialiseDoc(filePath);

        XmlDocument doc = new XmlDocument();

        doc.Load(filename);
        XmlElement day = doc.GetElementById(tag);
        if (day == null)
        {
            day = doc.CreateElement(Strings.Day);
            day.SetAttribute("id", tag);

            XmlElement root = doc.DocumentElement;
            root.AppendChild(day);
        }
        if (!item.filler)
        {
            XmlElement NE = doc.CreateElement(Strings.NewEntry);
            for (int i = 0; i < item.attributes.Length; i++)
            {
                XmlElement e = doc.CreateElement(item.labels[i]);
                e.InnerText = item.attributes[i];
                NE.AppendChild(e);
            }
            day.AppendChild(NE);
        }
        doc.Save(filename);
    }

    // write list
    public void Write(string filePath, string tag, NewEntryList list)
    {
        string filename = InitialiseDoc(filePath);

        XmlDocument doc = new XmlDocument();

        doc.Load(filename);
        XmlElement day = doc.GetElementById(tag);
        if (day == null)
        {
            day = doc.CreateElement(Strings.Day);
            day.SetAttribute("id", tag);

            XmlElement root = doc.DocumentElement;
            root.AppendChild(day);
        }

        for (int y = 0; y < list.Count(); y++)
        {
            NewEntry n;
            if (list.TryGet(y, out n))
            {
                if (!n.filler) {
                    XmlElement NE = doc.CreateElement(Strings.NewEntry);
                    for (int i = 0; i < n.attributes.Length; i++)
                    {
                        XmlElement e = doc.CreateElement(n.labels[i]);
                        e.InnerText = n.attributes[i];
                        NE.AppendChild(e);
                    }
                    day.AppendChild(NE);
                }
            }
            doc.Save(filename);
        }
    }
}
