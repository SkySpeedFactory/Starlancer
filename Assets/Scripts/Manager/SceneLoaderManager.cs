using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    private static SceneLoaderManager _instance;
    public static SceneLoaderManager Instance { get { return _instance; } }

    [SerializeField] private List<ScriptableScene> scenesData;

    public SceneData SceneData;
    public QuestData QuestData;
    public bool IsInWarp = false;

    private float minLoadingSceneDuation = 10f;
    private float loadingCounter;
    private bool startLoadingCounter = false;

    public static event Action<int> OnNewSceneLoaded;

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

    }

    // Start is called before the first frame update
    void Start()
    {
        SceneData = new SceneData(2);

        if (DataManager.LoadSaveGame)
        {
            SaveManager.Instance.LoadGame();
        }
        else
        {
            ChangeScene(2);
        }
        ResetLoadingCounter();
    }

    private void Update()
    {
        if (startLoadingCounter)
        {
            loadingCounter -= Time.deltaTime;
        }
    }

    public void ChangeScene(int currentSceneID, int newSceneID, int loadingSceneID)
    {
        IsInWarp = true;
        SceneData.CurrentScene.SceneIndex = newSceneID;
        SceneData.PrevScene.SceneIndex = currentSceneID;
        SceneManager.LoadScene(loadingSceneID, LoadSceneMode.Additive);
        // poolManager ai
        ObjectPoolManager.Instance.ResetAIShips();
        SceneManager.UnloadSceneAsync(currentSceneID);
        
        StartCoroutine(CheckIfSceneIsLoaded(newSceneID, loadingSceneID));
        //UpdateSkybox(SceneData.SceneIndex);
    }

    public void ChangeScene(int currentSceneID, int newSceneID)
    {
        SceneData.CurrentScene.SceneIndex = newSceneID;
        SceneData.PrevScene.SceneIndex = currentSceneID;
        SceneManager.LoadScene(newSceneID, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentSceneID);
    }
    public void ChangeOutOfHangarScene(int currentSceneID, int newSceneID)
    {
        SceneData.CurrentScene.SceneIndex = newSceneID;
        SceneData.PrevScene.SceneIndex = currentSceneID;
        // poolManager ai
        ObjectPoolManager.Instance.ResetAIShips();
        StartCoroutine(LeaveHangerSceneCoroutine(newSceneID));
        SceneManager.UnloadSceneAsync(currentSceneID);
    }

    public void ChangeScene(int newSceneID)
    {
        SceneData.CurrentScene.SceneIndex = newSceneID;
        StartCoroutine(CheckIfFirstSceneIsLoaded(newSceneID));
    }

    public int GetCurrentSceneIndex()
    {
        return SceneData.CurrentScene.SceneIndex;
    }

    private void UpdateSkybox(int id)
    {
        RenderSettings.skybox = scenesData[id].Skybox;
    }

    public IEnumerator CheckIfFirstSceneIsLoaded(int newSceneID)
    {
        startLoadingCounter = true;
        SceneManager.LoadScene(newSceneID, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneAt(SceneManager.sceneCount -1).name == SceneManager.GetSceneByBuildIndex(newSceneID).name && loadingCounter <= 0);
        OnNewSceneLoaded?.Invoke(newSceneID);
        QuestManager.Instance.StartQuest(1);
    }

    public IEnumerator CheckIfSceneIsLoaded(int newSceneID, int loadinSceneID)
    {
        startLoadingCounter = true;
        yield return new WaitUntil(() => loadingCounter <= 0);
        AsyncOperation operation = SceneManager.LoadSceneAsync(newSceneID, LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation.isDone);
        
        OnNewSceneLoaded?.Invoke(newSceneID);
        
        PlayerStats.Instance.GetComponent<PlayerController>().LeaveWarp();
        SceneManager.UnloadSceneAsync(loadinSceneID);
        yield return new WaitUntil(() => loadingCounter + 2 <= 0);
        if (QuestManager.Instance.GetActiveQuest().GetCurrentQuestGoal().GetQuestType() == EnumQuestTypes.MOVETO)
        {
            SetQuestMarker();
        }
        ResetLoadingCounter();
        IsInWarp = false;

    }

    private void SetQuestMarker()
    {
        QuestManager.Instance.GetActiveQuest().GetCurrentQuestGoal().SetQuestMarker();
    }

    private void ResetLoadingCounter()
    {
        startLoadingCounter = false;
        loadingCounter = minLoadingSceneDuation;
    }

    private IEnumerator LeaveHangerSceneCoroutine(int newSceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(newSceneID, LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation.isDone);
        PlayerStats.Instance.GetComponent<PlayerController>().LeaveHangar();
    }
}
