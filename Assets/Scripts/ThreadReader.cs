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
                if (element.Name == Strings.StartTime)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.EndTime)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.NameOfTeam)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.NumberOfPeople)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.PersonInCharge)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.Telephone)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.ConfirmationDate)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.Guide)
                    newGuide.startTime = element.InnerText;
                else if (element.Name == Strings.Notes)
                    newGuide.startTime = element.InnerText;
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
        Debug.Log(File.Exists(filename));
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


    public void Write(string filename)
    {
        XmlDocument doc = new XmlDocument();
        if (!File.Exists(filename))
        {
            doc.Save(filename);
        }
        doc.Load(filename);
    }

}
