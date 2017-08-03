using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;

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
            foreach (XmlElement entry in entries)
            {
                NewEntry newGuide = new NewEntry();
                XmlNodeList entryInfo = entry.ChildNodes;
                newGuide = ReadItem(entryInfo, newGuide);
                newGuide.filler = false;
                newGuide.SetDate(GetElementID(day));
                newGuide.id = GetElementID(entry);
                dayInfo.AddGuide(newGuide);
            }
        }
        {
            XmlNodeList entries = day.GetElementsByTagName(Strings.Event);
            foreach (XmlElement entry in entries)
            {
                Alarm alarm = new Alarm();
                XmlNodeList entryInfo = entry.ChildNodes;
                alarm = ReadItem(entryInfo, alarm);
                alarm.SetDate(GetElementID(day));
                alarm.id = GetElementID(entry);
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
    public void Write(string filePath, Item item) {
        string filename = InitialiseDoc(filePath);

        doc = new XmlDocument();

        doc.Load(filename);
        XmlElement day = GetElementById(item.Date);
        if (day == null)
        {
            day = doc.CreateElement(Strings.Day);
            day.SetAttribute("id", "_" + item.Date);

            XmlElement root = doc.DocumentElement;
            root.AppendChild(day);
        }
        if (!item.filler)
        {
            XmlNodeList NList = day.GetElementsByTagName(item.tag);
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
                NE = doc.CreateElement(item.tag);
                NE.SetAttribute("id", item.id);
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
    
    public void DeleteItem(string filePath, Item item)
    {
        if (File.Exists(filePath))
        {
            doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement element = GetElementById(item.id);
            if (element != null)
            {
                XmlNode parent = element.ParentNode;
                parent.RemoveChild(element);
            }
            doc.Save(filePath);
        }
    }
    
    public DAY SearchItem(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        DAY result = new DAY();
        string dataPath = Application.dataPath + @"/Calendar Data/Data";

        string[] years = Directory.GetDirectories(dataPath);
        foreach(string year in years)
        {
            string[] months = Directory.GetDirectories(year);
            foreach(string month in months)
            {
                string[] files = Directory.GetFiles(month);
                foreach(string file in files)
                {
                    if(file.EndsWith(".xml"))
                        if (File.Exists(file))
                        {
                            doc = new XmlDocument();
                            doc.Load(file);
                            XmlNodeList days = doc.GetElementsByTagName(Strings.Day);
                            foreach(XmlElement day in days)
                            {
                                XmlNodeList teams = day.GetElementsByTagName(Strings.NameOfTeam);
                                XmlNodeList guides = day.GetElementsByTagName(Strings.Guide);

                                foreach (XmlElement entry in teams)
                                {
                                    if (entry.InnerText.ToLower().Contains(searchTerm))
                                    {
                                        XmlNodeList entryInfo = entry.ParentNode.ChildNodes;
                                        NewEntry guide = new NewEntry();
                                        guide = ReadItem(entryInfo, guide);
                                        guide.filler = false;
                                        guide.SetDate(GetElementID(day));
                                        guide.id = GetElementID(entry.ParentNode as XmlElement);
                                        result.AddGuide(guide);
                                    }
                                }

                                foreach (XmlElement entry in guides)
                                {
                                    if (entry.InnerText.ToLower().Contains(searchTerm))
                                    {
                                        XmlNodeList entryInfo = entry.ParentNode.ChildNodes;
                                        NewEntry guide = new NewEntry();
                                        guide = ReadItem(entryInfo, guide);
                                        guide.filler = false;
                                        guide.SetDate(GetElementID(day));
                                        guide.id = GetElementID(entry.ParentNode as XmlElement);
                                        result.AddGuide(guide);
                                    }
                                }
                            }
                        }
                }
            }
        }

        return result;
    }

    public static void BackUp(string source, string destination)
    {
        foreach (string dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dir.Replace(source, destination));
        foreach (string file in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")))
            File.Copy(file, file.Replace(source, destination), true);
    }
}
