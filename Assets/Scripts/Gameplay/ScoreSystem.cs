using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreSystem : MonoBehaviour
{
    #region DELEGATE

    public delegate void OnAlienKilled();
    public OnAlienKilled onAlienKilled;

    public delegate void OnBuildingCollide();
    public OnBuildingCollide onBuildingCollide;

    public delegate void OnGameEnd();
    public OnGameEnd onGameOver;
    
    public delegate void OnTeleport();
    public OnTeleport onTeleport;

    public delegate void OnBuildingHit();
    public OnBuildingHit onBuildingHit;

    public delegate void OnPlayerHit();
    public OnPlayerHit onPlayerHit;
    #endregion
    
    #region DATA

    public TMPro.TextMeshProUGUI CurrentScore;
    public TMPro.TextMeshProUGUI plusPopUp;
    public TMPro.TextMeshProUGUI minusPopUp;
    private BuildingManager BuildingManager;
    public List<int> Highscores = new List<int>();

    private static ScoreSystem _instance;
    public static ScoreSystem Instance { get { return _instance; } }

    #endregion

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        onBuildingCollide += BuildingHit;
        onAlienKilled += AlienKilled;
        onTeleport += TeleportSuccess;
        onPlayerHit += PlayerHit;

        CurrentScore = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<TMPro.TextMeshProUGUI>();
        plusPopUp = GameObject.Find("PlusText").GetComponent<TMPro.TextMeshProUGUI>();
        minusPopUp = GameObject.Find("MinusText").GetComponent<TMPro.TextMeshProUGUI>();

        onGameOver += SaveHighscore;
    }

    private void UpdateScore()
    {
        int score = int.Parse(CurrentScore.text);
        CurrentScore.text = score.ToString();
    }
    
    public void AlienKilled()
    {
        PlusScore(200);
        UpdateScore();
    }

    private void PlayerHit()
    {
        MinusScore(100);
        UpdateScore();
    }

    private void BuildingHit()
    {
        MinusScore(150);
        UpdateScore();
    }

    private void TeleportSuccess()
    {
        PlusScore(50);
        UpdateScore();
    }

    public void SaveHighscore()
    {
        int currentScore = int.Parse(CurrentScore.text);
        if (PlayerPrefs.HasKey("Highscore"))
        {
            if(PlayerPrefs.GetInt("Highscore") < currentScore)
                PlayerPrefs.SetInt("Highscore", currentScore);
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
        }
        PlayerPrefs.Save();
    }

    public void PlusScore(int s)
    {
        int score = int.Parse(CurrentScore.text);
        score += s;
        CurrentScore.text = score.ToString();
        plusPopUp.text = "+" + s.ToString();
        Color transparentColor = new Color(0, 0, 0, 0);
        plusPopUp.color = transparentColor;
        StartCoroutine(FadeScore(true));
    }

    public void MinusScore(int s)
    {
        int score = int.Parse(CurrentScore.text);
        score -= s;
        CurrentScore.text = score.ToString();
        minusPopUp.text = "-" + s.ToString();
        Color transparentColor = new Color(0, 0, 0, 0);
        minusPopUp.color = transparentColor;
        StartCoroutine(FadeScore(false));
    }

    private IEnumerator FadeScore(bool isPlus)
    {
        float alpha = 1;
        if (isPlus)
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
            {
                Color newColor = new Color(0, 255, 39, Mathf.Lerp(alpha, 0, t));
                plusPopUp.color = newColor;
                yield return null;
            }
        }
        else
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
            {
                Color newColor = new Color(255, 0, 25, Mathf.Lerp(alpha, 0, t));
                minusPopUp.color = newColor;
                yield return null;
            }
        }
    }
}
