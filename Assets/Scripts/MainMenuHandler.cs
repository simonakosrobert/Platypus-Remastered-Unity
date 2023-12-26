using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{   
    [SerializeField] private GameObject M1S1;
    [SerializeField] private RawImage _image;
    [SerializeField] private GameObject _imageEarth;
    [SerializeField] private float _x, _y;
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource M1Music;
    [SerializeField] private TMP_Text currentLevel;
    [SerializeField] private TMP_Text skillPointsText;
    private bool fontSizeIncrease = true;
    [SerializeField] private Slider levelSlider;

    private Vector3 eulerAngles;

    public void SkillPointTextUpdater()
    {
        if (SaveLoadData._PlayerData.skillPoint == 1)
            {   
                skillPointsText.gameObject.SetActive(true);
                skillPointsText.text = SaveLoadData._PlayerData.skillPoint.ToString() + " skill point available";
            }
            else if (SaveLoadData._PlayerData.skillPoint > 1)
            {
                skillPointsText.gameObject.SetActive(true);
                skillPointsText.text = SaveLoadData._PlayerData.skillPoint.ToString() + " skill points available";
            }
            else
            {
                skillPointsText.gameObject.SetActive(false);
            }
    }

    public void SkillPointTextFontSize()
    {
        if (SaveLoadData._PlayerData.skillPoint > 0)
        {   
            if (skillPointsText.fontSize <= 40)
            {
                fontSizeIncrease = true;
            }
            else if (skillPointsText.fontSize >= 43)
            {
                fontSizeIncrease = false;
            }

            if (fontSizeIncrease)
            {
                skillPointsText.fontSize += 0.1f;
            }
            else
            {
                skillPointsText.fontSize -= 0.1f;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        eulerAngles = _imageEarth.transform.eulerAngles;
        GameHandler.currentStage = M1S1;
        GameHandler.currentStage.SetActive(false);
        levelSlider.value = LevelingSystem.CalculateLevelProgress();
        currentLevel.text = "Level " + LevelingSystem.currentLevel.ToString();
        SkillPointTextUpdater();
        GameHandler.pause = true;
        Time.timeScale = 0.0f;
        mainMenuMusic.Play();
    }

    void OnEnable()
    {   
        LevelingSystem.CalculateLevel(true);
        levelSlider.value = LevelingSystem.CalculateLevelProgress();
        currentLevel.text = "Level " + LevelingSystem.currentLevel.ToString();
        SkillPointTextUpdater();
        GameHandler.pause = true;
        Time.timeScale = 0.0f;
        mainMenuMusic.Play();
    }

    public void NewGame()
    {
        GameHandler.currentStage = M1S1;
        GameHandler.currentStage.SetActive(true);
        GameHandler.pause = false;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        mainMenuMusic.Stop();
        M1Music.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        _image.uvRect = new Rect(_image.uvRect.position + new Vector2(_x, _y) * Time.unscaledDeltaTime, _image.uvRect.size);
        _imageEarth.transform.rotation *= Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0.1f);
        SkillPointTextFontSize();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SaveLoadData._PlayerData.totalXP = 0;
            InGameStats.bufferXP = 0;
            SaveLoadData._PlayerData.skillPoint = 0;
            LevelingSystem.CalculateLevel(true);
            levelSlider.value = LevelingSystem.CalculateLevelProgress();
            currentLevel.text = "Level " + LevelingSystem.currentLevel.ToString();
            SkillPointTextUpdater();
        }
    }
}
