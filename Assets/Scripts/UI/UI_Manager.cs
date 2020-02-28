using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public bool isEntered;
    float elapsedTime;
    string button = "";

    #region MAIN_MENU

    private GameObject MainMenu;
    private GameObject Instruction;
    private GameObject Highscore;
    private TMPro.TextMeshProUGUI HighscoreTMP;
    private TMPro.TextMeshProUGUI VisualHelperTMP;
    private bool isVHelperBlinking = false;
    private AudioManager AudioManager;

    private enum MenuIndex{
        MainMenu = 0,
        Instruction = 1,
        Highscore = 2
    }
    #endregion
    private void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnPointerEnter(string b)
    {
        isEntered = true;
        button = b;
    }

    public void OnPointerExit()
    {
        isEntered = false;
    }

    
    public void Quit()
    {
        Application.Quit();
    }

    private void ShowInstruction()
    {
        MainMenu.SetActive(false);
        Instruction.SetActive(true);
    }

    public void ShowHighscore()
    {
        MainMenu.SetActive(false);
        Instruction.SetActive(false);
        Highscore.SetActive(true);
    }

    private void BackToMain()
    {
        MainMenu.SetActive(true);
        Instruction.SetActive(false);
        Highscore.SetActive(false);
    }
    
    private void Update()
    {
        if (isEntered)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 2f)
            {
                switch (button)
                {
                    case "start":
                        StartGame();
                        isEntered = false;
                        AudioManager.ButtonPressed.Play();
                        break;
                    case "highScore":
                        ShowHighscore();
                        isEntered = false;
                        AudioManager.ButtonPressed.Play();
                        break;
                    case "instruction":
                        ShowInstruction();
                        isEntered = false;
                        AudioManager.ButtonPressed.Play();
                        break;
                    case "quit":
                        Quit();
                        isEntered = false;
                        AudioManager.ButtonPressed.Play();
                        break;
                    case "backToMain":
                        BackToMain();
                        isEntered = false;
                        AudioManager.ButtonPressed.Play();
                        break;
                }
            }
        }
        else
        {
            elapsedTime = 0;
        }

        // Blink visual helper
        BlinkVisualHelperCo();
    }

    public void LoadHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            HighscoreTMP.text = PlayerPrefs.GetInt("Highscore").ToString();
        }
        else
        {
            HighscoreTMP.text = "No highscore, play the game!";
        }
    }

    private void Start()
    {
        MainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        MainMenu.SetActive(true);
        Instruction = GameObject.FindGameObjectWithTag("Instruction");
        Instruction.SetActive(false);
        Highscore = GameObject.FindGameObjectWithTag("Highscore");
        HighscoreTMP = GameObject.Find("Highscore").GetComponent<TMPro.TextMeshProUGUI>();
        LoadHighscore();
        Highscore.SetActive(false);
        VisualHelperTMP = GameObject.Find("VisualHelper").GetComponent<TMPro.TextMeshProUGUI>();

        AudioManager = AudioManager.Instance;
    }

    private void BlinkVisualHelperCo()
    {
        if (!isVHelperBlinking)
        {
            StartCoroutine(BlinkVisualHelper());
        }
    }

    private IEnumerator BlinkVisualHelper()
    {
        isVHelperBlinking = true;
        yield return StartCoroutine(Fade.FadeTo(1.0f, 1.0f, VisualHelperTMP));
        yield return StartCoroutine(Fade.FadeTo(0.0f, 1.0f, VisualHelperTMP));
        isVHelperBlinking = false;
    }
}
