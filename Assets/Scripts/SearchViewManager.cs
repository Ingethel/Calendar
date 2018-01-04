using UnityEngine;

public class SearchViewManager : IViewManager {

    string searchTerm;
    
    protected override void SetHeader()
    {
        header.text = gManager.language.SearchResult + searchTerm;
    }

    public override void SetLanguage()
    {
        SetHeader();
    }

    protected override void AssignInfo(GameObject o, Event n)
    {
        IItemListView<Event> o_view = o.GetComponent<IItemListView<Event>>();
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
