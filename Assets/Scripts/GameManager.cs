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
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, SettingsManager.ReadFloat("FullScreen") == 1);
        headerObj.SetActive(SettingsManager.ReadFloat("FullScreen") == 1);
        SetLanguage((int)SettingsManager.ReadFloat("Language"));

        if(currentDate.Month == 1 && currentDate.Day == 1 && SettingsManager.Read("LastIdReset") != TimeConversions.DateTimeToString(currentDate))
            ResetIDs();

        if (SettingsManager.ReadFloat("LastBackUp") != currentDate.Month)
        {
            SettingsManager.Write("LastBackUp", currentDate.Month);
            RearrangeData();
            DataReader.BackUp(DATA_PATH, EXPORT_PATH + "/CalendarDataBackUp", true);
        }
    }

    void InitialStartUp()
    {
        // check if first startup - initialise required variables
        if (PlayerPrefs.GetInt("Start") == 0 || true)
        {
            PlayerPrefs.SetInt("Start", 1);
            DATA_PATH = Application.dataPath + @"/Calendar Data";
            SettingsManager.Write("DataPath", DATA_PATH);
            EXPORT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SettingsManager.Write("ExportPath", EXPORT_PATH);
            IMPORT_PATH = "";
            SettingsManager.Write("ImportPath", IMPORT_PATH);

            ResetIDs();
        }
        else
        {
            DATA_PATH = SettingsManager.Read("DataPath");
            EXPORT_PATH = SettingsManager.Read("ExportPath");
            IMPORT_PATH = SettingsManager.Read("ImportPath");
        }
    }

    public void ReloadScene()
    {
        if (OnReloadScene != null)
            OnReloadScene();

        PlayerPrefs.SetInt("LoadLastState", 1);
        
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
                    if (currentDate.Year * 12 + currentDate.Month - folderMonth >= SettingsManager.ReadFloat("OldDataThreshold"))
                    {
                        DataReader.BackUp(dir, LEGACY_FOLDER + dir.Substring(DATA_FOLDER.Length), false);
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
        SettingsManager.Write("Language", value);
        
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
        SettingsManager.Write("FullScreen", b ? 1 : 0);
        headerObj.SetActive(b);
    }
    
    public void Options()
    {
        CalendarViewController viewController = FindObjectOfType<CalendarViewController>();
        if (viewController)
            viewController.RequestView(CalendarViewController.State.OPTIONS);
    }
    
    void ResetIDs()
    {
        PlayerPrefs.SetInt("Guide", 0);
        PlayerPrefs.SetInt("Event", 0);
        SettingsManager.Write("LastIdReset", TimeConversions.DateTimeToString(currentDate));
    }

    void UpdateTimetable(string id, string value)
    {
        SettingsManager.Write(id, value);
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
                    SettingsManager.Write("MinimumTourTime", spacing);
                break;
            case "LEGACY_THRESHOLD":
                int time = 0;
                if (int.TryParse(s[2], out time))
                    SettingsManager.Write("OldDataThreshold", time);
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
                        DataReader.BackUp(DATA_FOLDER, EXPORT_PATH + "/CalendarDataBackUp/Data", true);
                    else if (s[2] == "LEGACY")
                        DataReader.BackUp(LEGACY_FOLDER, EXPORT_PATH + "/CalendarDataBackUp/Legacy", true);
                }
                else if (s.Length == 2)
                    DataReader.BackUp(DATA_PATH, EXPORT_PATH + "/CalendarDataBackUp", true);

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
                    DataReader.BackUp(path, DATA_PATH, false);
                }
                else {
                    if(IMPORT_PATH != "")
                        DataReader.BackUp(IMPORT_PATH, DATA_PATH, false);
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
            case "Monday":
            case "Tuesday":
            case "Wednesday":
            case "Thursday":
            case "Friday":
            case "Saturday":
            case "Sunday":
                if (s.Length > 2)
                    UpdateTimetable(s[1], s[2]);
                break;
            case "TicketPrice":
                if (s.Length > 2)
                {
                    float f = 0;
                    float.TryParse(s[2], out f);
                    SettingsManager.Write("TicketPrice", f);
                }
                break;
            case "ReducedTicketPrice":
                if (s.Length > 2)
                {
                    float f = 0;
                    float.TryParse(s[2], out f);
                    SettingsManager.Write("ReducedTicketPrice", f);
                }
                break;
            case "OPTIONS":
                Options();
                break;
            case "DataPath":
                if (s.Length > 2)
                {
                    SettingsManager.Write("DataPath", s[2]);
                    DATA_PATH = s[2];
                }
                break;
            case "ExportPath":
                if (s.Length > 2)
                {
                    SettingsManager.Write("ExportPath", s[2]);
                    EXPORT_PATH = s[2];
                }
                break;
            case "ImportPath":
                if (s.Length > 2)
                {
                    SettingsManager.Write("ImportPath", s[2]);
                    IMPORT_PATH = s[2];
                }
                break;
            default:
                break;
        }
    }
}
