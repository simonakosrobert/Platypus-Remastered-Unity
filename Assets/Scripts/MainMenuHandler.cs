using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{   
    [SerializeField] private GameObject M1S1;
    [SerializeField] private RawImage _image;
    [SerializeField] private GameObject _imageEarth;
    [SerializeField] private float _x, _y;
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource M1Music;

    private Vector3 eulerAngles;


    // Start is called before the first frame update
    void Start()
    {   
        eulerAngles = _imageEarth.transform.eulerAngles;
        GameHandler.currentStage = M1S1;
        GameHandler.currentStage.SetActive(false);
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
    }
}
