using System;
using System.Collections;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {
    
    public GameObject headerObj;
    [HideInInspector]
    public string DATA_FOLDER, LEGACY_FOLDER, DATA_PATH, EXPORT_PATH, IMPORT_PATH;
    [HideInInspector]
    public string DESKTOP;

    public event Action PrintMode;
    public event Action OnLanguageChange;
    public event Action OnReloadScene;
    public event Action OnUpdateWeekTimes;

    public Language language;

    public DateTime currentDate;

    void Awake()
    {
        currentDate = DateTime.Now;
        InitialStartUp();
        DATA_FOLDER = DATA_PATH + "/Data";
        LEGACY_FOLDER = DATA_PATH + "/Legacy";
    }

    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, PlayerPrefs.GetInt("FullScreen") == 1);
        headerObj.SetActive(PlayerPrefs.GetInt("FullScreen") == 1);
        SetLanguage(PlayerPrefs.GetInt("Language"));

        if(currentDate.Month == 1 && currentDate.Day == 1 && PlayerPrefs.GetString("LastIdReset") != TimeConversions.DateTimeToString(currentDate))
            ResetIDs();

        if (PlayerPrefs.GetInt("LastBackUp") != currentDate.Month)
        {
            PlayerPrefs.SetInt("LastBackUp", currentDate.Month);
            RearrangeData();
            ThreadReader.BackUp(DATA_PATH, EXPORT_PATH + "/CalendarDataBackUp", true);
        }
    }

    void InitialStartUp()
    {
        // check if first startup - initialise required variables
        if (PlayerPrefs.GetInt("Start") == 0)
        {
            PlayerPrefs.SetInt("Start", 1);
            DATA_PATH = Application.dataPath + @"/Calendar Data";
            PlayerPrefs.SetString("DataPath", DATA_PATH);
            EXPORT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PlayerPrefs.SetString("ExportPath", EXPORT_PATH);
            IMPORT_PATH = "";
            PlayerPrefs.SetString("ImportPath", IMPORT_PATH);
            
            ResetIDs();
            PlayerPrefs.SetInt("OldDataThreshold", 2);

            PlayerPrefs.SetFloat("TicketPrice", 3);
            PlayerPrefs.SetFloat("ReducedTicketPrice", 1.5f);

            PlayerPrefs.SetInt("MinimumTourTime", 40);

            PlayerPrefs.SetString("WeekTimes", "09:00,10:30,12:00,13:30");
            PlayerPrefs.SetString("WeekendTimes", "10:30,12:00,13:30,15:00,16:30");
        }
    }

    public void ReloadScene()
    {
        if (OnReloadScene != null)
            OnReloadScene();

        PlayerPrefs.SetInt("LoadLastState", 1);
        PlayerPrefs.SetInt("FullScreen", Screen.fullScreen ? 1 : 0);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void RearrangeData()
    {
        foreach (string dir in Directory.GetDirectories(DATA_FOLDER, "*", SearchOption.AllDirectories))
        {
            int folderMonth = 0;
            int folderYear = 0;
            if (int.TryParse(dir.Substring(dir.LastIndexOf('\\') - 5, 4), out folderYear))
            {
                if (int.TryParse(dir.Substring(dir.LastIndexOf('\\') + 1), out folderMonth))
                {
                    folderMonth = folderYear * 12 + folderMonth;
                    if (currentDate.Year * 12 + currentDate.Month - folderMonth >= PlayerPrefs.GetInt("OldDataThreshold"))
                    {
                        ThreadReader.BackUp(dir, LEGACY_FOLDER + dir.Substring(DATA_FOLDER.Length), false);
                        Directory.Delete(dir, true);
                    }
                }
            }
        }
    }

    public void SetLanguage(int value)
    {
        if (value == 0)
            language = new English();
        else if (value == 1)
            language = new Greek();
        PlayerPrefs.SetInt("Language", value);

        if (OnLanguageChange != null)
            OnLanguageChange();
        headerObj.GetComponentInChildren<UnityEngine.UI.Text>().text = language.Title;
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
        StartCoroutine(PrintProcess());
    }

    public void Options()
    {
        CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
        if (viewController)
            viewController.RequestView(CalendarViewController.State.OPTIONS);
    }

    IEnumerator PrintProcess()
    {
        bool headerFlag = headerObj.activeSelf;
        if (PrintMode != null)
            PrintMode();
        if (headerFlag)
            headerObj.SetActive(false);
        yield return new WaitForSeconds(1);
        Application.CaptureScreenshot(EXPORT_PATH + "/Calendar.png");
        yield return 0;
        if (PrintMode != null)
            PrintMode();
        if (headerFlag)
            headerObj.SetActive(true);
        yield return 0;
    }

    void ResetIDs()
    {
        PlayerPrefs.SetInt("Guide", 0);
        PlayerPrefs.SetInt("Event", 0);
        PlayerPrefs.SetString("LastIdReset", TimeConversions.DateTimeToString(currentDate));
    }

    void UpdateTimetable(string id, string value)
    {
        PlayerPrefs.SetString(id, value);
        if (OnUpdateWeekTimes != null)
            OnUpdateWeekTimes();
        CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
        if (viewController)
            viewController.RefreshView();
    }

    public void Command(string[] s)
    {
        switch (s[1])
        {
            case "FULLSCREEN":
                SetFullScreen(s[2] == "ON" ? true : false);
                break;
            case "CLEAR_LEGACY":
                if(s.Length > 2 && s[2] == "TRUE")
                    Directory.Delete(LEGACY_FOLDER, true);
                break;
            case "SEARCH_LEGACY":
                if(s.Length > 2)
                GetComponent<DataManager>().SearchLegacy(s[2]);
                break;
            case "MinimumTourTime":
                int spacing = 0;
                if(int.TryParse(s[2], out spacing))
                    PlayerPrefs.SetInt("MinimumTourTime", spacing);
                break;
            case "LEGACY_THRESHOLD":
                int time = 0;
                if (int.TryParse(s[2], out time))
                    PlayerPrefs.SetInt("OldDataThreshold", time);
                break;
            case "EXIT":
                ExitApplication();
                break;
            case "RESET_IDS":
                ResetIDs();
                break;
            case "BACKUP":
                if (s.Length == 3)
                {
                    if (s[2] == "DATA")
                        ThreadReader.BackUp(DATA_FOLDER, EXPORT_PATH + "/CalendarDataBackUp/Data", true);
                    else if (s[2] == "LEGACY")
                        ThreadReader.BackUp(LEGACY_FOLDER, EXPORT_PATH + "/CalendarDataBackUp/Legacy", true);
                }
                else if (s.Length == 2)
                    ThreadReader.BackUp(DATA_PATH, EXPORT_PATH + "/CalendarDataBackUp", true);

                break;
            case "REARRANGE":
                RearrangeData();
                break;
            case "IMPORT":
                if (s.Length > 2)
                {
                    string path = s[2];
                    for (int i = 3; i < s.Length; i++)
                        path = path + " " + s[i];
                    ThreadReader.BackUp(path, DATA_PATH, false);
                }
                else {
                    if(IMPORT_PATH != "")
                        ThreadReader.BackUp(IMPORT_PATH, DATA_PATH, false);
                }
                ReloadScene();
                break;
            case "REPORT":
                CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
                if (viewController)
                    viewController.RequestView(CalendarViewController.State.REPORT);
                break;
            case "HELP":
                ExtrasViewController extras = FindObjectOfType<ExtrasViewController>();
                if (extras)
                    extras.RequestView(ExtrasViewController.State.HELP);
                break;
            case "LANGUAGE":
                if(s.Length > 2)
                {
                    int i = 0;
                    int.TryParse(s[2], out i);
                    SetLanguage(i);
                }
                break;
            case "WeekTimes":
                if (s.Length > 2)
                    UpdateTimetable("WeekTimes", s[2]);
                break;
            case "WeekendTimes":
                if (s.Length > 2)
                    UpdateTimetable("WeekendTimes", s[2]);
                break;
            case "TicketPrice":
                if (s.Length > 2)
                {
                    float f = 0;
                    float.TryParse(s[2], out f);
                    PlayerPrefs.SetFloat("TicketPrice", f);
                }
                break;
            case "ReducedTicketPrice":
                if (s.Length > 2)
                {
                    float f = 0;
                    float.TryParse(s[2], out f);
                    PlayerPrefs.SetFloat("ReducedTicketPrice", f);
                }
                break;
            case "OPTIONS":
                Options();
                break;
            case "DataPath":
                if (s.Length > 2)
                {
                    PlayerPrefs.SetString("DataPath", s[2]);
                    DATA_PATH = s[2];
                }
                break;
            case "ExportPath":
                if (s.Length > 2)
                {
                    PlayerPrefs.SetString("ExportPath", s[2]);
                    EXPORT_PATH = s[2];
                }
                break;
            case "ImportPath":
                if (s.Length > 2)
                {
                    PlayerPrefs.SetString("ImportPath", s[2]);
                    IMPORT_PATH = s[2];
                }
                break;
            default:
                break;
        }
    }
}
