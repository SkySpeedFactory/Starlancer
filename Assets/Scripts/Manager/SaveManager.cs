using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
     
    public static SaveManager Instance { get { return _instance; } }

    private GameObject HUD;

    private Serializer serializer;
    private SaveData saveData;
    private int sceneIndex;
    private PlayerStats playerStats;
    private GameObject pauseMenu = null;
    private GameObject pauseMenuContainer;
    public bool isPaused = false;

    [SerializeField]
    private GameObject pauseMenuPrefab;
    [SerializeField]
    public string RootSavePath = string.Empty;
    [SerializeField]
    private static string buildVersion = "0.1";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        serializer = new Serializer(RootSavePath);
        SceneLoaderManager.OnNewSceneLoaded += OnNewSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void OnNewSceneLoaded(int newSceneID)
    {
        LoadData();
    }

    public void TogglePauseMenu()
    {
        if (!isPaused && !SceneLoaderManager.Instance.IsInWarp)
        {
            isPaused = true;
            HUD = GameObject.Find("Canvas");
            HUD.SetActive(false);
            pauseMenuContainer = GameObject.Find("PauseMenuContainer");

            if (HUD != null)
            {
                pauseMenu = GameObject.Instantiate(pauseMenuPrefab, pauseMenuContainer.transform);
                pauseMenuContainer.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Debug.Log("no canvas found");
            }

        }
        else if (!SceneLoaderManager.Instance.IsInWarp)
        {
            isPaused = false;
            Destroy(pauseMenu);
            HUD.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void SaveGame()
    {
        playerStats = PlayerStats.Instance;
        GameObject Player = PlayerStats.Instance.gameObject;
        SceneLoaderManager.Instance.SceneData.SetData();
        SceneData scenedata = SceneLoaderManager.Instance.SceneData;
        SceneLoaderManager.Instance.QuestData.SetData();
        QuestData questData =  SceneLoaderManager.Instance.QuestData;
        saveData = new SaveData(playerStats, scenedata, questData);
        serializer.SaveData(saveData);
        //serializer.SavePersistanceData(saveData);
        saveData = null;
    }

    public void SetFirstLoad(bool firstLoad)
    {
        DataManager.FirstLoad = firstLoad;
    }

    public void LoadGame()
    {
        saveData = serializer.LoadData();
        
        if (saveData.Version == buildVersion)
        {
            SceneData SceneData = saveData.GetSceneData();
            TradingManager.Instance.SetCurrentLandedStationID(SceneData.CurrentScene.CurrentTradingID);
            SceneLoaderManager.Instance.ChangeScene(SceneData.CurrentScene.SceneIndex);
            SceneLoaderManager.Instance.SceneData = SceneData;
           
        }
        else
        {
            Debug.LogError("Build Version and Save File version Not the Same");
        }

    }

    private void LoadData()
    {
        playerStats = PlayerStats.Instance;
        if (saveData != null)
        {
            PlayerData playerData = saveData.GetPlayerData();
            playerData.SetData();
            QuestData questData = saveData.GetQuestData();
            questData.LoadData();

        }
    }

    public void SceneChangeSave()
    {
        playerStats = PlayerStats.Instance;
        GameObject Player = PlayerStats.Instance.gameObject;
        SceneLoaderManager.Instance.SceneData.LoadAiData();
        saveData = new SaveData(playerStats, SceneLoaderManager.Instance.SceneData);
        serializer.SavePersistanceData(saveData);
        saveData = null;
    }

    public void SceneChangeLoad()
    {
        SaveData PersSaveData = serializer.LoadPersistanceData();
        SceneData PersSceneData = saveData.GetSceneData();
        AIManager.Instance.AiDataList = PersSceneData.PrevScene.AiDataList;
    }
}
