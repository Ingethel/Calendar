using System.Collections.Generic;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, NewEntryList> entries;
    ThreadReader reader;

    void Awake()
    {
        entries = new Dictionary<string, NewEntryList>();
        reader = new ThreadReader();
    }

    private void Append(Dictionary<string, NewEntryList> temp)
    {
        foreach(string s in temp.Keys)
        {
            entries[s] = temp[s];
        }
    }


    public void RequestReadMonth(DateTime date)
    {
        string filepath = Application.dataPath + @"/Calendar Data/Data/" + date.Year.ToString() + "/" + date.Month.ToString() + "/" + Strings.file;
        Append(reader.Read(filepath));
    }
    
    public void RequestReadDay(String id)
    {
        string[] split = id.Split('.');
        string filepath = Application.dataPath + @"/Calendar Data/Data/" + split[2] + "/" + split[1] + "/" + Strings.file;
        NewEntryList list = reader.Read(filepath, id);
        if(list != null)
        {
            entries[id] = list;
        }
    }

    public void RequestWrite(NewEntry e)
    {
        string filename = e.year + "/" + e.month;
        string tag = e.day + "." + e.month + "." + e.year;
        ThreadReader reader = new ThreadReader();
        NewEntryList list;
        if (!entries.TryGetValue(tag, out list))
            list = new NewEntryList();

        list.Add(e);

        entries[tag] = list;
        reader.Write(filename, tag, e);
    }

    public SearchResult TryGetEntries(string id)
    {
        SearchResult res = new SearchResult();
        NewEntryList list;
        res.value = entries.TryGetValue(id, out list);
        if (!res.value)
        {
            RequestReadDay(id);
            res.value = entries.TryGetValue(id, out list);
        }
        if (res.value)
            res.info = list;
        return res;
    }

    public void SearchTerm(String text)
    {

    }

}

public class SearchResult
{
    public bool value;
    public NewEntryList info;
}