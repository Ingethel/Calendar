using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {

    public GameObject headerObj;

    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        headerObj.SetActive(true);
    }

	public void ExitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        headerObj.SetActive(!headerObj.activeSelf);
    }

    public void Print()
    {
        Application.CaptureScreenshot(Application.persistentDataPath+"Calendar.png");
    }

    public void Command(string s)
    {
        switch (s)
        {
            case "$_FULLSCREEN":
                SetFullScreen();
                break;
            case "$_CLEAR_DATA":
                break;
            case "$_CLEAR_LEGACY":
                break;
            case "$_SEARCH_LEGACY":
                break;
            default:
                break;
        }
    }
}
