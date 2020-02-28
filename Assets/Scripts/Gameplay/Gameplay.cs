using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    private static Gameplay _instance;
    public static Gameplay Instance { get{ return _instance; } }  

    public bool isWin = false;

    public delegate void OnWin();
    public OnWin onWin;
    public delegate void OnLose();
    public OnLose onLose;
    public delegate void OnBossDead(bool dead);
    public OnBossDead onBossDead;
    
    public TMPro.TextMeshProUGUI HPText;
    public TMPro.TextMeshProUGUI BuildingHPText;
    public GameObject winText;
    public GameObject loseText;
    public GameObject alienBoss;
    public const float GRAVITY = -2.7f;
    public BuildingManager BuildingManager;

    // Debug
    public TMPro.TextMeshProUGUI debugTMP;
    public string debugTxt;

    private bool _isEnd = false;
    public bool IsEnd { get { return _isEnd; } set { _isEnd = value; } }

    void Awake()
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
    void Start()
    {
        // Player's HP Text
        HPText = GameObject.FindGameObjectWithTag("HealthPoints").GetComponent<TMPro.TextMeshProUGUI>();

        // Win/lose text
        winText = GameObject.FindGameObjectWithTag("Win");
        winText.SetActive(false);
        loseText = GameObject.FindGameObjectWithTag("Lose");
        loseText.SetActive(false);

        // Spawn boss
        alienBoss = GameObject.FindGameObjectWithTag("Boss");

        // Check boss dead
        onBossDead = CheckWin;

        // Building Manager
        BuildingManager = BuildingManager.Instance;
        BuildingHPText = GameObject.FindGameObjectWithTag("BuildingHPText").GetComponent<TMPro.TextMeshProUGUI>();

        // World physics
        SetGravity();

        // Debug
        //debugTMP = GameObject.Find("Debug").GetComponent<TMPro.TextMeshProUGUI>();
        //debugTxt = debugTMP.text;
    }
    
    void Update()
    {
        CheckPlayerHeight();
        CheckLose();
        UpdateHPText();
        UpdateCurrentBuildingHPText();
        Restart();
        //debugTMP.text = BuildingManager.GetCurrentBuilding().Info.buildingName + " " + BuildingManager.GetCurrentBuildingIndex();
    }

    public void UpdateHPText()
    {
        HPText.text = Player.Instance.hp.ToString();
    }

    public void Restart()
    {
        if(winText != null && loseText != null)
        {
            if (Input.GetButtonDown("Jump") && (winText.activeInHierarchy || loseText.activeInHierarchy))
            {
                ResetWinLoseGO();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }else if(Input.GetButtonDown("Main") && (winText.activeInHierarchy || loseText.activeInHierarchy))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void UpdateCurrentBuildingHPText()
    {
        BuildingHPText.text = BuildingManager.GetCurrentBuilding().Info.currentHP.ToString();
    }

    private void ResetWinLoseGO()
    {
        winText.SetActive(false);
        loseText.SetActive(false);
    }

    private void CheckWin(bool dead)
    {
        if (dead && winText != null)
        {
            winText.SetActive(true);
            ScoreSystem.Instance.onGameOver?.Invoke();
        }
    }

    public void CheckPlayerHeight()
    {
        if (Player.Instance)
        {
            if (Player.Instance.gameObject.transform.position.y < -48.0f)
            {
                Player.Instance.hp = 0;
            }
        }
    }
    private void SetGravity()
    {
        Physics.gravity = new Vector3(0, GRAVITY, 0);
    }

    private void CheckLose()
    {
        if(Player.Instance.hp <= 0)
        {
            loseText.SetActive(true);
            ScoreSystem.Instance.onGameOver?.Invoke();
            return;
        }
    }
}
