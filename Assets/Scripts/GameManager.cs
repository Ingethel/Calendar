using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {

    public GameObject headerObj;

    void Awake()
    {
        int spacing = PlayerPrefs.GetInt("TimeThreshold");
        if (spacing == 0)
            PlayerPrefs.SetInt("TimeThreshold", 45);
    }

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

    public void SetFullScreen(bool b)
    {
        Screen.fullScreen = b;
        headerObj.SetActive(b);
    }

    public void Print()
    {
        Application.CaptureScreenshot(Application.persistentDataPath+"Calendar.png");
    }

    public void Command(string[] s)
    {
        switch (s[1])
        {
            case "FULLSCREEN":
                SetFullScreen(s[2] == "ON" ? true : false);
                break;
            case "CLEAR_DATA":
                break;
            case "CLEAR_LEGACY":
                break;
            case "SEARCH_LEGACY":
                break;
            case "TIME_THRESHOLD":
                int spacing = 0;
                if(int.TryParse(s[2], out spacing))
                    PlayerPrefs.SetInt("TimeThreshold", spacing);
                break;
            case "EXIT":
                ExitApplication();
                break;
            case "RESET_IDS":
                PlayerPrefs.SetInt("Guide", 0);
                PlayerPrefs.SetInt("Event", 0);
                break;
            default:
                break;
        }
    }
}
