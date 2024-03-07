using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFileButton : MonoBehaviour
{
    public void SetSaveFile()
    {
        DataManager.SaveFileName = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        DataManager.LoadSaveGame = true;
    }
}
