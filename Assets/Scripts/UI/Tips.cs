using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    private const string SHOOT_TIP = "A: Shoot";
    //private const string JUMP_TIP = "Y: Jump";
    private const string TELEPORT_TIP = "X: Teleport to building";
    private const string CANCEL_TIP = "PRESS L2 to enable/disable tips.";

    private static List<string> TIPS;
    
    private GameObject mainTips;
    private GameObject gameTips;
    private TMPro.TextMeshProUGUI mainTipsText;
    private TMPro.TextMeshProUGUI gameTipsText;

    private const float COOLDOWN = 5;
    private float lastCycleTime = 0;
    private int lastIndex = 0;
    private Color InitColor = new Color(124, 154, 255); // blueish 
    private Color FinalColor = new Color(225, 154, 255); // purplish

    // Managr references
    public BuildingManager BuildingManager;
    public AudioManager AudioManager;

    // Check if game tips is showing
    bool isGameTipsShowing = false;

    void Start()
    {
        TIPS = new List<string>();
        AssignTips();

        // Main tips (at the top)
        mainTips = GameObject.FindGameObjectWithTag("Tips");
        mainTipsText = mainTips.GetComponent<TMPro.TextMeshProUGUI>();

        // Game tips (display when needed)
        gameTips = GameObject.Find("GameTips");
        gameTipsText = gameTips.GetComponent<TMPro.TextMeshProUGUI>();
        gameTips.SetActive(false);

        lastIndex = (int)Random.Range(0, TIPS.Count);
        //text.text = TIPS[lastIndex];
        lastCycleTime = Time.time;

        // Building manager instance
        BuildingManager = BuildingManager.Instance;

        // Audio manager instance
        AudioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //CycleTips();
        CancelTips();

        DisplayGameTips();
    }

    private void CycleTips()
    {
        if (lastCycleTime <= Time.time)
        {
            lastIndex = lastIndex < TIPS.Count - 1 ? lastIndex+=1 : 0;
            lastCycleTime = Time.time + COOLDOWN;
            mainTipsText.text = TIPS[lastIndex];
        }
    }

    private static void AssignTips()
    {
        TIPS.Add(SHOOT_TIP);
        //TIPS.Add(JUMP_TIP);
        TIPS.Add(TELEPORT_TIP);
        TIPS.Add(CANCEL_TIP);
    }

    public void CancelTips()
    {
        if (Input.GetButtonDown("Cancel1") || Input.GetKeyDown(KeyCode.C))
        {
            if (!mainTips.activeInHierarchy)
            {
                mainTips.SetActive(true);
            }
            else
            {
                mainTips.SetActive(false);
            }
        }
    }

    public void DisplayGameTips()
    {
        if (!isGameTipsShowing)
        {
            if (CurrentBuildingFallingWarning())
            {
                StartCoroutine(DisplayGameTipsCo());
                return;
            }
            else if (IncomingProjectileWarning())
            {
                StartCoroutine(DisplayGameTipsCo());
                return;
            }
        }
    }

    public IEnumerator DisplayGameTipsCo()
    {
        isGameTipsShowing = true;
        gameTips.SetActive(true);
        yield return StartCoroutine(Fade.FadeTo(1.0f, 1.0f, gameTipsText));
        yield return StartCoroutine(Fade.FadeTo(0.0f, 1.0f, gameTipsText));
        gameTips.SetActive(false);
        isGameTipsShowing = false;
    }

    private bool CurrentBuildingFallingWarning()
    {
        bool isFalling = false;
        
        if (BuildingManager.GetCurrentBuilding().Info.currentHP <= 0)
        {
            isFalling = true;
            gameTipsText.text = "Building destroyed! Teleport to other building!";
            AudioManager.Warning.Play();
        }

        return isFalling;
    }

    private bool IncomingProjectileWarning()
    {
        bool isIncomingProjectile = false;
        for (int i = 0; i < BulletPool.ENEMY_MAX_BULLET; i++)
        {
            if (BulletPool.enemyBulletPool[i].activeInHierarchy)
            {
                if ((BulletPool.enemyBulletPool[i].transform.position - Player.Instance.transform.position).magnitude < 6.5f)
                {
                    gameTipsText.text = "Incoming projectile detected, MOVE!";
                    AudioManager.Warning.Play();
                    isIncomingProjectile = true;
                    break;
                }
            }
            else
            {
                continue;
            }
        }
        return isIncomingProjectile;
    }
}
