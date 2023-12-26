using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{   

    [SerializeField] private Slider levelSlider;
    [SerializeField] private TMP_Text currentXPText;
    [SerializeField] private TMP_Text currentPointText;
    [SerializeField] private GameObject fill;

    public void pointCounterHandler()
    {
        if (InGameStats.bufferPoints != InGameStats.totalPoints)
        {   
            float currentPoint = Mathf.Lerp(InGameStats.bufferPoints, InGameStats.totalPoints, InGameStats.pointCounterTimer / 0.5f);
            currentPoint = Mathf.Ceil(currentPoint);
            InGameStats.bufferPoints = (int)currentPoint;
            InGameStats.pointCounterTimer += Time.deltaTime;
            currentPointText.text = InGameStats.bufferPoints.ToString("00000000");
        }
        else
        {
            InGameStats.pointCounterTimer = 0;
        }
    }

    public void XPCounterHandler()
    {
        if (InGameStats.bufferXP != SaveLoadData._PlayerData.totalXP)
        {   
            float currentPoint = Mathf.Lerp(InGameStats.bufferXP, SaveLoadData._PlayerData.totalXP, InGameStats.xpCounterTimer / 0.5f);
            currentPoint = Mathf.Ceil(currentPoint);
            InGameStats.bufferXP = (int)currentPoint;
            InGameStats.xpCounterTimer += Time.deltaTime;
            currentXPText.text = InGameStats.bufferXP.ToString() + " / " + LevelingSystem.nextLevelXp.ToString();
        }
        else
        {
            InGameStats.xpCounterTimer = 0;
            currentXPText.text = SaveLoadData._PlayerData.totalXP.ToString() + " / " + LevelingSystem.nextLevelXp.ToString();
        }
    }

    public void levelBarHandler()
    {
        InGameStats.sliderBuffer = LevelingSystem.CalculateLevelProgress();

        if (levelSlider.value != InGameStats.sliderBuffer)
        {   
            levelSlider.value = Mathf.Lerp(levelSlider.value, InGameStats.sliderBuffer, InGameStats.sliderBufferTimer / 1f);
            InGameStats.sliderBufferTimer += Time.deltaTime;
        }
        else
        {
            InGameStats.sliderBufferTimer = 0;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentXPText.text = SaveLoadData._PlayerData.totalXP.ToString() + " / " + LevelingSystem.nextLevelXp.ToString();
        currentPointText.text = InGameStats.bufferPoints.ToString("00000000");
    }

    void Start()
    {
        levelSlider.value = LevelingSystem.CalculateLevelProgress();
    }

    // Update is called once per frame
    void Update()
    {
        pointCounterHandler();
        levelBarHandler();
        XPCounterHandler();
    }
}
