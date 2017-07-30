using UnityEngine;
using UnityEngine.UI;

public class ViewHeaderHandler : MonoBehaviour {

    public GameObject[] buttons;

    GameManager gManager;

    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
        gManager.printMode += PrintMode;
    }

    public void PrintMode()
    {
        foreach (GameObject b in buttons)
            b.SetActive(!b.activeSelf);
    }
}
