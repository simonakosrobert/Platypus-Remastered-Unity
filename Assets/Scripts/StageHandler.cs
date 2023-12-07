using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StageHandler : MonoBehaviour
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject _object;
    [SerializeField] private string stageName;

    private TMP_Text textMP;
    private bool introOver = false;

    private IEnumerator FadeOut(TMP_Text textDisplay, float duration)
    {
        float currentTime = 0f;

        //Fade in
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0f;

        //Hold
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        currentTime = 0f;

        //Fade out
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        introOver = true;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {   
        introOver = false;
        textMP = _object.GetComponent<TMP_Text>();
        textMP.text = stageName;
        StartCoroutine(FadeOut(textMP, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (introOver)
        {
            GameHandler.StageIntro = false;
            canvas.GetComponent<Canvas> ().enabled = false;
        }
    }
}
