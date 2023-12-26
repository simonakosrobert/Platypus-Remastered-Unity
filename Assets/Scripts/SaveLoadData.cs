using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine.Android;
using System;
using System.Collections.Generic;
using TMPro;
public class SaveLoadData : MonoBehaviour
{   

    private static string path;
    private static string key = "b14ca5898a4e4133bbce2ea2315a1916";

    private int tick = 0;

    float lvlUpStartTime = 0;
    float lvlUpFontSize = 1;
    byte lvlUpOpacity = 255;

    public void Awake()
    {   
        Input.backButtonLeavesApp = true;
        path = Application.persistentDataPath + "/PlayerData.gec";
        Load();
        LevelingSystem.CalculateLevel(true);
    }

    [SerializeField] public GameObject levelUpText;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject levelUpSound;
    public static bool levelUpSoundPlaying;
    
    public void Update()
    {   
        tick += 1;

        if (tick >= 600)
        {
            Save();
            tick = 0;
        }

        if (LevelingSystem.leveledUp)
        {   

            if (!levelUpSoundPlaying)
            {
                AudioSource levelUpSoundClone = Instantiate(levelUpSound.GetComponent<AudioSource>());
                levelUpSoundClone.transform.SetParent(GameObject.Find("Effects").transform);
                levelUpSoundClone.Play();
                Destroy(levelUpSoundClone.gameObject, levelUpSoundClone.clip.length);
                levelUpSoundPlaying = true;
            }

            levelUpText.SetActive(true);
            if (lvlUpStartTime < 0.3f)
            {   
                lvlUpStartTime += Time.deltaTime;
                lvlUpFontSize += 2.5f;
                levelUpText.GetComponent<TMP_Text>().fontSize = lvlUpFontSize;
            }
            else if (lvlUpStartTime >= 0.3f && lvlUpStartTime <= 2)
            {
                lvlUpStartTime += Time.deltaTime;
            }
            else if(lvlUpStartTime > 2 && lvlUpOpacity > 0)
            {   
                lvlUpStartTime += Time.deltaTime;
                lvlUpOpacity -= 5;
                levelUpText.GetComponent<TMP_Text>().color = new Color32(255, 240, 0, lvlUpOpacity);
            }
            else
            {
                LevelingSystem.leveledUp = false;
                lvlUpOpacity = 255;
            }
        }
        else
        {
            levelUpText.SetActive(false);
            lvlUpStartTime = 0;
            lvlUpFontSize = 1;
            levelUpText.GetComponent<TMP_Text>().fontSize = lvlUpFontSize;
            levelUpText.GetComponent<TMP_Text>().color = new Color32(255, 240, 0, lvlUpOpacity);
        }

        LevelingSystem.CalculateLevel(false); 
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

[Serializable]
public class PlayerData
{
    public int totalXP = 0;
    public int skillPoint = 0;
    public int highScore = 0;
}

[Serializable]
public class LevelingSystem
{   

    public static List<int> levels = new List<int> {0, 20, 50, 100, 250, 500, 100000, 250000, 700000, 1500000};
    public static int levelCounter = 1;
    public static int currentLevel = 1;
    public static int nextLevelXp = 0;
    public static int currentLevelXp = 0; 
    public static bool leveledUp = false;

    public static void CalculateLevel(bool initialCalc)
    {   
      
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] <= SaveLoadData._PlayerData.totalXP)
            {
                levelCounter = i + 1;
                if (i + 1 == levels.Count)
                {
                    levelCounter = i + 1;
                    currentLevelXp = levels[i];
                    nextLevelXp = levels[i];
                }
                else
                {
                    levelCounter = i + 1;
                    currentLevelXp = levels[i];
                    nextLevelXp = levels[i + 1];
                }
                
            } 
        }

        if (levelCounter != currentLevel && !initialCalc)
        {
            currentLevel = levelCounter;
            leveledUp = true;
            SaveLoadData.levelUpSoundPlaying = false;
            SaveLoadData._PlayerData.skillPoint += 1;
        }
        else if (levelCounter != currentLevel && initialCalc)
        {
            currentLevel = levelCounter;
        }
    }

    public static float CalculateLevelProgress()
    {   
        // 500 - 0 = 500 needed
        float neededXP = nextLevelXp - currentLevelXp;
        // 492 - 0 = 492 current progress
        float progressXP = SaveLoadData._PlayerData.totalXP - currentLevelXp;

        float barProgress = progressXP/neededXP;

        return barProgress;

    }

}