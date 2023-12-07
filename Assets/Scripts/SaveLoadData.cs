using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine.Android;
using System;



public class SaveLoadData : MonoBehaviour
{   

    private static string path;
    private static string key = "b14ca5898a4e4133bbce2ea2315a1916";

    private int tick = 0;

    public void Awake()
    {   
        Input.backButtonLeavesApp = true;
        path = Application.persistentDataPath + "/PlayerData.gec";        
    }
    
    public void Update()
    {   
        tick += 1;

        if (tick >= 600)
        {
            Save();
            tick = 0;
        }
    }

    public static PlayerData _PlayerData = new PlayerData();

    public static void Save()
    {
        if (File.Exists(path))
        {   
            try
            {
            string player = JsonConvert.SerializeObject(_PlayerData);
            var encryptedString = AesOperation.EncryptString(key, player); 
            File.WriteAllText(path, encryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't save file! due to this error: {e}");
            }
        }
    }

    public static void Load(){
        
        if (!File.Exists(path))
        {   
            try
            {
            string player = JsonConvert.SerializeObject(_PlayerData);
            var encryptedString = AesOperation.EncryptString(key, player); 
            File.WriteAllText(path, encryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't create file! due to this error: {e}");
            }
        }
        else
        {   
            try
            {
            string loadPlayerData = File.ReadAllText(path);
            var decryptedString = AesOperation.DecryptString(key, loadPlayerData);
            _PlayerData = JsonConvert.DeserializeObject<PlayerData>(decryptedString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't load file! due to this error: {e}");
            }
        }
        
    }
}

[System.Serializable]
public class PlayerData
{
    public int totalXP = 0;
}