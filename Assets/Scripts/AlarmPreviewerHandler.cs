using System.Collections.Generic;
using UnityEngine;

public class AlarmPreviewerHandler : Panel {

    public GameObject ViewList, ViewItem;

    void Update()
    {
        KeybordInputHandler();
    }

    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void SetView(List<Alarm> list)
    {
        foreach(Alarm a in list)
        {
            GameObject o = Instantiate(ViewItem);
            o.transform.SetParent(ViewList.transform);
            o.transform.localScale = Vector3.one;
            IItemListView<Alarm> o_view = o.GetComponent<IItemListView<Alarm>>();
            if (o_view)
                o_view.Allocate(a);
        }
    }

}
