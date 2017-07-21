using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {
    
    void Start()
    {
        Screen.SetResolution(1280, 720, true);
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
    }

    public void Print()
    {
        Application.CaptureScreenshot(Application.persistentDataPath+"Calendar.png");
    }
}
