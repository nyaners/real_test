using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel, optionsPanel
    [SerializeField]
    private text bestScoreText
    [SerializeField]
    private Slider volumeSlider
    publice static int colore, bestScore

    private enum Colors 
    {
        White = 1,
        Yellow = 2,
        Green = 3 

    }


    void Start()
    {
        menuPanel.SetActive(true);
        optonsPanel.SetActive(false);
        volumeSlider.value = PlayrsPrefs.GetFloat('Volume',1f);
        coloreNum = PlayerPrefs.GetInt('ColoreNum', 1);
        bestScore = PlayerPrefs.GetInt('BestScore', 0);
    }

    void Update()
    {
        bestScoreText.text = 'Best Score' + bestScore.Tostring();
        AudioListener.volume = volumeSlider.value; 
    }
    public void SetColor(string colorSpriteName)
    {
        switch (colorSpriteName)
        {
            case 'White':
                coloreNum = (int)Colors.White
                break;
            case 'Yellow':
                coloreNum = (int)Colors.Yellow
                break;
            case 'Green':
                coloreNum = (int)Colors.Yellow
                break;
        }
        PlayerPrefs.SetInt('ColorNum', colorNum);
    }

    public void OpenGameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1),
    }

}
