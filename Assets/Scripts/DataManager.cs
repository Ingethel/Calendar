using System.Collections.Generic;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, DAY> entries;
    ThreadReader reader;

    void Awake()
    {
        entries = new Dictionary<string, DAY>();
        reader = new ThreadReader();
    }

    private void Append(Dictionary<string, DAY> temp)
    {
        foreach(string s in temp.Keys)
        {
            entries[s] = temp[s];
        }
    }

    private string DateToPath(DateTime date) {
        return Application.dataPath + @"/Calendar Data/Data/" + date.Year.ToString() + "/" + date.Month.ToString() + "/" + Strings.file;
    }

    private string TagToPath(string s)
    {
        string[] split = s.Split('.');
        return Application.dataPath + @"/Calendar Data/Data/" + split[2] + "/" + split[1] + "/" + Strings.file;
    }


    public void RequestReadMonth(DateTime date)
    {
        string filepath = DateToPath(date);
        Append(reader.Read(filepath));
    }
    
    public void RequestReadDay(string id)
    {
        string filepath = TagToPath(id);
        DAY list = reader.Read(filepath, id);
        if(list != null)
        {
            entries[id] = list;
        }
    }

    public void RequestWrite(NewEntry e)
    {
        string filename = TagToPath(e.Date);
        DAY day_info;
        if (!entries.TryGetValue(e.Date, out day_info))
            day_info = new DAY();

        day_info.AddGuide(e);

        entries[e.Date] = day_info;
        reader.Write(filename, e);
    }

    public void RequestDelete(NewEntry e)
    {
        string filename = TagToPath(e.Date);
        Debug.Log(e.Date);
        DAY day_info;
        if (entries.TryGetValue(e.Date, out day_info))
        {
            day_info.guides.Remove(e);
            entries[e.Date] = day_info;
        }
        reader.DeleteItem(filename, e);
    }

    public SearchResult TryGetEntries(string id)
    {
        SearchResult res = new SearchResult();
        DAY day_info;
        res.value = entries.TryGetValue(id, out day_info);
        if (!res.value)
        {
            RequestReadDay(id);
            res.value = entries.TryGetValue(id, out day_info);
        }
        if (res.value)
            res.info = day_info;
        return res;
    }

    public void SearchTerm(string text)
    {

    }

}

public class SearchResult
{
    public bool value;
    public DAY info;
}