using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Weapon: MonoBehaviour
{
    #region SINGLETON
    private static Weapon _instance = new Weapon();
    public static Weapon Instance { get { return _instance; } }
    #endregion

    #region FIELDS
    /// <summary>Damage inflicted on target.</summary>
    public int damage { get { return 200 + (int)wpnLvl * 50; } }

    /// <summary>Fire rate per second.</summary>
    public int fireRate;

    ///<summary>Reload time in milliseconds.</summary>
    public float reloadTime = 2.0f;

    /// <summary>Initializes weapon level.</summary>
    public Weapon_Level wpnLvl = Weapon_Level.One;
    public int wpnEXP = 0;

    public Vector3 direction;
    public GameObject cam;
    public TMPro.TextMeshProUGUI wpnTxt;
    public BulletPool bp;
    public AudioManager AudioManager;
    
    public enum Weapon_Level : int
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8
    }
    #endregion

    #region DELEGATES

    public delegate void OnExpGain(int EXP);
    public OnExpGain onExpGain;

    #endregion

    #region FUNCTIONS

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
        AudioManager = AudioManager.Instance;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        wpnTxt = GameObject.FindGameObjectWithTag("WeaponUI").GetComponent<TMPro.TextMeshProUGUI>();
        Physics.IgnoreLayerCollision(0, 1, true);

        onExpGain = GainEXP;
    }

    private void Update()
    {
        UpdateWeapon();

        Fire();
    }
    
    public void Fire()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (Input.GetKeyDown(KeyCode.V) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            BulletPool.BulletFire(cam);
            AudioManager.BulletPlayer.Play();
        }
        else
        {
            return;
        }
        
    }

    private void UpdateWeapon()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        transform.position = cam.transform.position;
        transform.forward = cam.transform.forward;
        
        LevelUp();
    }

    public Vector3 GetCurrentDirection()
    {
        return direction;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void GainEXP(int EXP)
    {
        wpnEXP += EXP;
    }

    private void LevelUp()
    {
        if((float)wpnEXP / 100 >= (float)wpnLvl && wpnLvl < Weapon_Level.Four)
        {
            wpnLvl++;
            UpdateInfo();
        }
        else if((float)wpnEXP / 150 >= (float)wpnLvl && wpnLvl < Weapon_Level.Six)
        {
            wpnLvl++;
            UpdateInfo();
        }
        else if ((float)wpnEXP / 200 >= (float)wpnLvl && wpnLvl < Weapon_Level.Eight)
        {
            wpnLvl++;
            UpdateInfo();
        }
    }
    
    private void UpdateInfo()
    {
        if(wpnLvl < Weapon_Level.Eight)
        {
            wpnTxt.text = ((int)wpnLvl).ToString() + "(" + damage + ")";
        }
        else
        {
            wpnTxt.text = "8 (MAX)" + "(" + damage + ")";
        }
    }
    #endregion
}
