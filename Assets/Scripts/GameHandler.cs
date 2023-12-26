using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public static bool mainMenu = true;
    public static bool pause = false;
    public static bool StageIntro = true;
    
    public static GameObject currentStage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class InGameStats
{
    public static int totalPoints = 0;
    public static int bufferPoints = 0;
    public static float pointCounterTimer = 0;

    public static int bufferXP = 0;
    public static float xpCounterTimer = 0;

    public static float sliderBuffer = 0;
    public static float sliderBufferTimer = 0;
}
