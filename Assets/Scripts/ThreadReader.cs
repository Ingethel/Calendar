using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;

public class ThreadReader /*: ThreadJob*/ {

    XmlDocument doc;
    
    private string GetElementID(XmlElement e)
    {
        string[] temp = e.GetAttribute("id").Split('_');
        return temp[1];
    }

    private XmlElement GetElementById(string id)
    {
        return doc.GetElementById("_" + id);
    }

    private T ReadItem<T>(XmlNodeList list, T item) where T : Item
    {
        foreach (XmlElement element in list)
        {
            for (int i = 0; i < item.attributes.Length; i++)
            {
                if (element.Name == item.labels[i])
                {
                    item.attributes[i] = element.InnerText;
                    break;
                }
            }
        }
        return item;
    }

    private DAY GetDayInfo(XmlElement day) {
        DAY dayInfo = new DAY();
        {
            XmlNodeList entries = day.GetElementsByTagName(Strings.NewEntry);
            foreach (XmlNode entry in entries)
            {
                NewEntry newGuide = new NewEntry();
                XmlNodeList entryInfo = entry.ChildNodes;
                newGuide = ReadItem(entryInfo, newGuide);
                newGuide.filler = false;
                newGuide.SetDate(GetElementID(day));
                dayInfo.AddGuide(newGuide);
            }
        }
        {
            XmlNodeList entries = day.GetElementsByTagName(Strings.Event);
            foreach (XmlNode entry in entries)
            {
                Alarm alarm = new Alarm();
                XmlNodeList entryInfo = entry.ChildNodes;
                alarm = ReadItem(entryInfo, alarm);
                alarm.SetDate(GetElementID(day));
                dayInfo.AddEvent(alarm);
            }
        }
        return dayInfo;
    }

    // Read By Day
    public DAY Read(string filename, string ID) {
        DAY dayInfo = new DAY();
        if (File.Exists(filename)) {
            doc = new XmlDocument();
            doc.Load(filename);
            XmlElement day = GetElementById(ID);
            if(day != null)
                dayInfo = GetDayInfo(day);
        }
        return dayInfo;
    }

    // Read By Month
    public Dictionary<string, DAY> Read(string filename)
    {
        Dictionary<string, DAY> monthInfo = new Dictionary<string, DAY>();
        if (File.Exists(filename))
        {
            
            doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList entries = doc.GetElementsByTagName(Strings.Day);
            foreach(XmlElement day in entries)
            {
                string id = GetElementID(day);
                monthInfo.Add(id, GetDayInfo(day));
            }
        }
        return monthInfo;
    }

    private string InitialiseDoc(string filePath)
    {
        string files = filePath.Substring(0, filePath.LastIndexOf('/'));
        if (!Directory.Exists(files))
            Directory.CreateDirectory(files);

        doc = new XmlDocument();

        if (!File.Exists(filePath))
        {
            doc.Save(filePath);
            XmlTextWriter writer = new XmlTextWriter(filePath, null);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("Entries", null, null, Strings.doctype);
            writer.WriteStartElement("Entries");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
        return filePath;
    }

    // write element
    public void Write(string filePath, NewEntry item) {
        string filename = InitialiseDoc(filePath);

        doc = new XmlDocument();

        doc.Load(filename);
        XmlElement day = GetElementById("_" + item.Date);
        if (day == null)
        {
            day = doc.CreateElement(Strings.Day);
            day.SetAttribute("id", "_" + item.Date);

            XmlElement root = doc.DocumentElement;
            root.AppendChild(day);
        }
        if (!item.filler)
        {
            XmlNodeList NList = day.GetElementsByTagName(Strings.NewEntry);
            XmlElement NE = null;
            bool exists = false;
            if (NList != null)
            {
                foreach (XmlElement element in NList)
                {
                    if (GetElementID(element) == item.id)
                    {
                        NE = element;
                        NE.IsEmpty = true;
                        exists = true;
                        break;
                    }
                }
            }
            if (!exists)
            {
                NE = doc.CreateElement(Strings.NewEntry);
                NE.SetAttribute("id", "_" + item.id);
            }

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
    
    public void DeleteItem(string filePath, NewEntry n)
    {
        Debug.Log("requesting delete");
        Debug.Log(filePath);
        
        Debug.Log(File.Exists(filePath));
        if (File.Exists(filePath))
        {
            doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement element = GetElementById(n.id);
            Debug.Log(n.id);
            if (element != null)
            {
                Debug.Log("deleting");
                XmlNode parent = element.ParentNode;
                parent.RemoveChild(element);
            }
            doc.Save(filePath);
        }
    }

}
