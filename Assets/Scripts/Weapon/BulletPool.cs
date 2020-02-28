using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject enemyBulletPrefab;

    public static List<GameObject> bulletPool;
    public static List<GameObject> enemyBulletPool;
    public const int MAX_BULLET = 35;
    public const int ENEMY_MAX_BULLET = 35;
    private const float BULLET_SPEED = 100f;
    private const float ENEMY_BULLET_SPEED = 5.5f;
    private const float BOSS_BULLET_SPEED = 12.0f;
    private GameObject player;
    private float countActive;

    private static BulletPool _instance;
    public static BulletPool Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        InstantiateBullets();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update()
    {
        UpdateBullets();
    }
    private void InstantiateBullets()
    {
        bulletPool = new List<GameObject>(MAX_BULLET);
        enemyBulletPool = new List<GameObject>(ENEMY_MAX_BULLET);
        for (int i = 0; i < MAX_BULLET; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.layer = 1;
            bulletPool.Add(bullet);
        }

        for (int i = 0; i < ENEMY_MAX_BULLET; i++)
        {
            GameObject eb = Instantiate(enemyBulletPrefab);
            eb.SetActive(false);
            enemyBulletPool.Add(eb);
        }
    }

    public static void BulletFire(GameObject cam)
    {
        Instance.countActive = bulletPool.Where(x => x.activeInHierarchy == true).Count();
        GameObject bullet = _instance.GetBullet("player");
        if (!bullet.activeInHierarchy)
        {
            bullet.SetActive(true);
            bullet.transform.position = cam.transform.position;
            bullet.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x + 85, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
            // Bullet size powerup
            if (Instance.countActive == 0)
            {
                bullet.transform.localScale = new Vector3(0.03432425f, 0.1290827f, 0.034292f);
            }
            else
            {
                bullet.transform.localScale *= Instance.countActive * 0.18f;
            }
            bullet.GetComponent<Rigidbody>().velocity = (cam.transform.forward * BULLET_SPEED);
        }
        else
        {
            Debug.LogError("Bullet already active!");
        }
    }


    public static void FireAtPlayer(GameObject enemy, GameObject player)
    {
        GameObject bullet = _instance.GetBullet("fighter");
        if (!bullet.activeInHierarchy && enemy.tag == "Alien")
        {
            bullet.SetActive(true);
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.position = enemy.transform.position + enemy.transform.forward * 3.0f;
            bullet.transform.rotation = Quaternion.Euler(enemy.transform.eulerAngles.x + 90, enemy.transform.eulerAngles.y, enemy.transform.eulerAngles.z);

            bullet.GetComponent<Rigidbody>().velocity = (player.transform.position + Vector3.up*1.5f - enemy.transform.position).normalized * ENEMY_BULLET_SPEED;
        }
        else if (!bullet.activeInHierarchy && enemy.name == "AlienBoss")
        {
            bullet.SetActive(true);
            bullet.transform.localScale *= 2.5f;
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.position = enemy.transform.position + enemy.transform.forward * 4.0f;
            bullet.transform.rotation = Quaternion.Euler(enemy.transform.eulerAngles.x + 90, enemy.transform.eulerAngles.y, enemy.transform.eulerAngles.z);

            bullet.GetComponent<Rigidbody>().velocity = (player.transform.position + Vector3.up * 1.35f - enemy.transform.position).normalized * BOSS_BULLET_SPEED;
        }
    }

    private GameObject GetBullet(string type)
    {
        if(type == "player")
        {
            for (int i = 0; i < MAX_BULLET; i++)
            {
                if (!bulletPool[i].activeInHierarchy)
                {
                    return bulletPool[i];
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            for (int i = 0; i < ENEMY_MAX_BULLET; i++)
            {
                if (!enemyBulletPool[i].activeInHierarchy)
                {
                    return enemyBulletPool[i];
                }
                else
                {
                    continue;
                }
            }
        }
        return null;
    }

    private void UpdateBullets()
    {
        for(int i = 0; i < MAX_BULLET; i++)
        {
            if (bulletPool[i] == null) continue;
            if((bulletPool[i].transform.position - player.transform.position).magnitude > 250)
            {
                RecycleBullet(bulletPool[i]);
            }
        }

        for (int i = 0; i < ENEMY_MAX_BULLET; i++)
        {
            if (enemyBulletPool[i] == null) continue;
            if ((enemyBulletPool[i].transform.position - player.transform.position).magnitude > 100)
            {
                RecycleBullet(enemyBulletPool[i]);
            }
        }
    }

    public void RecycleBullet(GameObject bulletToRemove)
    {
        if(bulletToRemove.tag == "Bullet")
        {
            bulletToRemove.transform.localScale = new Vector3(0.03432425f, 0.1290827f, 0.034292f);
            bulletToRemove.transform.rotation = Quaternion.identity;
            bulletToRemove.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bulletToRemove.SetActive(false);
        }
        else if(bulletToRemove.tag == "EnemyBullet")
        {
            bulletToRemove.transform.localScale = new Vector3(0.04102232f, 0.4613841f, 0.04367467f);
            bulletToRemove.transform.rotation = Quaternion.identity;
            bulletToRemove.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bulletToRemove.SetActive(false);
        }
    }

}
