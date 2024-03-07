using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
    private static string savePath = string.Empty;
    private string RootPath = string.Empty;
    private SaveData saveData;

    public Serializer(string roothPath = "")
    {
        RootPath = roothPath;
        SetPath();
    }

    private void SetPath()
    {
        if (RootPath == String.Empty)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            savePath = Application.persistentDataPath + "/Saves/";
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            savePath = Application.persistentDataPath + "/Saves/";
        }

    }

    public void SetRootPath(string rootpath)
    {
        RootPath = rootpath;
    }

    private void SerializeData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + $"{++DataManager.SaveCount}-Save" + ".sls", FileMode.Create);
        formatter.Serialize(fs, saveData);
        fs.Close();
    }

    private void SerializePersistanceData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + $"PersistanceData" + ".sps", FileMode.Create);
        formatter.Serialize(fs, saveData);
        fs.Close();
    }

    private void DeserializeData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(savePath + DataManager.SaveFileName, FileMode.Open);
        saveData = formatter.Deserialize(fs) as SaveData;
        fs.Close();
    }

    private void DeserializePersistanceData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            FileStream fs = new FileStream(savePath + $"PersistanceData" + ".sps", FileMode.Open);
            saveData = formatter.Deserialize(fs) as SaveData;
            fs.Close();
        }
        catch (Exception)
        {

        }
    }

    public void SaveData(SaveData data)
    {
        saveData = data;
        SerializeData();
    }

    public void SavePersistanceData(SaveData data)
    {
        saveData = data;
        SerializePersistanceData();
    }


    public SaveData LoadData()
    {
        DeserializeData();
        return saveData;
    }

    public SaveData LoadPersistanceData()
    {
        DeserializePersistanceData();
        return saveData;
    }
}
