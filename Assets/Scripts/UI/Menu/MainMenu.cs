using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.Shift;

public class MainMenu : MonoBehaviour
{
    private string saveFilePath;
    private DirectoryInfo directoryPath;
    private FileInfo[] saveFileArray;
    private List<FileInfo> saveFileNames;
    private List<FileInfo> saveFilesOrdered;

    private bool firstLoad = true;

    [SerializeField]
    private GameObject ContinueButtonObject;
    private ChapterButton contBtn;

    public GameObject SaveFileScroll;
    public GameObject SaveFileScrollContent;
    public GameObject SaveFilePrefab;

    private void Awake()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        saveFilePath = Application.persistentDataPath + "/Saves";
        directoryPath = new DirectoryInfo(saveFilePath);

        if (File.Exists(Application.persistentDataPath + "/Saves/" + "1-Save.sls"))
        {
            ContinueButtonObject.SetActive(true);
            contBtn = ContinueButtonObject.GetComponent<ChapterButton>();
        }
        else
        {
            DataManager.LoadSaveGame = false;
            ContinueButtonObject.SetActive(false);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        
    }

    private void Start()
    {
        saveFileArray = directoryPath.GetFiles("*.sls");
        saveFileNames = saveFileArray.ToList();
        sortSaveFiles();
        if (saveFileArray.Length!=0)
        {
            string lastSave = saveFilesOrdered.Last().Name;
            string[] strings = lastSave.Split("-");
            DataManager.SaveCount = int.Parse(strings[0]);
        }

    }

    public void NewGameButton()
    {
        DataManager.LoadSaveGame = false;
        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {
        DataManager.LoadSaveGame = true;
        DataManager.SaveFileName = DataManager.SaveCount + "-Save" + ".sls";
        SceneManager.LoadScene(1);
    }

    public void LoadButton()
    {
        SceneManager.LoadScene(1);
    }
    
    public void OpenLoadButton()
    {
        if (firstLoad)
        {
            firstLoad = false;
            List<Button> btnList = new List<Button>();
            int e = 0;
            for (int i = saveFilesOrdered.Count - 1; i >= 0; i--)
            {
                GameObject btn = GameObject.Instantiate(SaveFilePrefab, SaveFileScrollContent.transform);
                Button btnObject = btn.GetComponent<Button>();
                btnList.Add(btnObject);
                ChapterButton btnText = btn.GetComponentInChildren<ChapterButton>();
                btnText.buttonTitle = saveFilesOrdered[i].Name;
                btnText.buttonDescription = saveFilesOrdered[i].CreationTime.ToString();
                btnObject.onClick.AddListener(LoadButton);
                e++;
                if (e > 21)
                {
                    break;
                }
            }
        }
        
    }
    
    private void sortSaveFiles()
    {
        saveFilesOrdered = saveFileNames.OrderBy(x => x.CreationTime).ToList();
        if (saveFilesOrdered.Count > 0)
        {
            contBtn.buttonDescription = saveFilesOrdered.Last().CreationTime.ToString();
        }
        
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
