using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;

public class ThreadReader /*: ThreadJob*/ {
    
    private List<NewEntry> GetDayInfo(XmlElement day) {
        List<NewEntry> dayInfo = new List<NewEntry>();
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
            dayInfo.Add(newGuide);
        }
        return dayInfo;
    }

    // Read By Day
    public List<NewEntry> Read(string filename, string ID) {
        List<NewEntry> dayInfo = new List<NewEntry>();
        if (File.Exists(filename)) {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlElement day = doc.GetElementById(ID);
            dayInfo = GetDayInfo(day);
        }
        return dayInfo;
    }

    // Read By Month
    public Dictionary<string, List<NewEntry>> Read(string filename)
    {
        Dictionary<string, List<NewEntry>> monthInfo = new Dictionary<string, List<NewEntry>>();
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


    public void Write(string filename, string tag, List<NewEntry> list)
    {
        XmlDocument doc = new XmlDocument();
        if (!File.Exists(filename))
        {
            XmlElement root = doc.DocumentElement;
            XmlElement e = doc.CreateElement(Strings.Entries);
            root.AppendChild(e);
            doc.Save(filename);
        }
        doc.Load(filename);
        XmlElement day = doc.GetElementById(tag);
        if(day == null)
        {
            day = doc.CreateElement(Strings.Day);
            day.SetAttribute("id", tag);
            doc.AppendChild(day);
        }

        foreach(NewEntry n in list)
        {
            XmlElement NE = doc.CreateElement(Strings.NewEntry);
            for(int i = 0; i < n.attributes.Length; i++)
            {
                XmlElement e = doc.CreateElement(n.labels[i]);
                e.InnerText = n.attributes[i];
                NE.AppendChild(e);
            }
            day.AppendChild(NE);
        }
        doc.Save(filename);
    }
}
