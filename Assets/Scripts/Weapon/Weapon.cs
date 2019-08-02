using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon: MonoBehaviour
{
    #region STATS
    /// <summary>Damage inflicted on target.</summary>
    public int damage { get { return 200 + (int)wpnLvl * 30; } }

    /// <summary>Fire rate per second.</summary>
    public int fireRate;

    ///<summary>Reload time in milliseconds.</summary>
    public float reloadTime = 2.0f;

    /// <summary>Initializes weapon level.</summary>
    public Weapon_Level wpnLvl;
    
    public float bulletSpeed = 10.0f;

    public int magazineSize { get { return 50 + (int)wpnLvl * 5; } }

    public GameObject cam;

    public GameObject bullet;

    //test
    public Rigidbody bullet_rb;
    public enum Weapon_Level : int
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
    #endregion


    #region FUNCTIONS
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        Physics.IgnoreLayerCollision(0, 1, true);
    }

    private void Update()
    {
        // update weapon
        UpdateWeapon();

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject bulletClone = Instantiate(bullet, transform.position, transform.rotation);
            bulletClone.SetActive(true);
            bulletClone.layer = 1;
            bulletClone.GetComponent<Rigidbody>().velocity = cam.transform.forward * bulletSpeed * 100;
            //Debug.Log("bullet dir: " + Camera.main.transform.forward + " " + cam.transform.forward);
        }
    }
    
    private void UpdateWeapon()
    {
        transform.position = cam.transform.position;
    }
    #endregion
}
