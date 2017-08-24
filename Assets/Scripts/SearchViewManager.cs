using UnityEngine;

public class SearchViewManager : IViewManager {

    string searchTerm;
    
    protected override void SetHeader()
    {
        header.text = "Search Result for: " + searchTerm;
    }

    protected override void AssignInfo(GameObject o, NewEntry n)
    {
        IItemListView<NewEntry> o_view = o.GetComponent<IItemListView<NewEntry>>();
        if (o_view != null)
            o_view.Allocate(n);
    }

    public void SetView(DAY day, string result)
    {
        Refresh();
        searchTerm = result;
        info = day;
        SetHeader();
        DisplayInfo();
    }
}
