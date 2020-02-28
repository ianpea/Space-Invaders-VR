using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    public int hp = 100000;
    public const float ENEMY_SLOW_RADIUS = 12.0f;

    private GameObject cam;
    private AudioManager AudioManager;

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
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        AudioManager = AudioManager.Instance;
    }

    private void Update()
    {
        IsPlayerOnRoof();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            Instance.hp -= 50;
            AudioManager.PlayerHit.Play();
            BulletPool.Instance.RecycleBullet(collision.gameObject);
            ScoreSystem.Instance.onPlayerHit?.Invoke();
            //Handheld.Vibrate();
        }
        if(collision.gameObject.tag == "Building")
        {
            AudioManager.PlayerFall.Play();
        }
    }

    public int GetHP()
    {
        return hp;
    }

    /// <summary>
    /// Check if player is above roof of building. Works by raycasting downwards of player.
    /// </summary>
    public void IsPlayerOnRoof()
    {
        for(int i = 1; i <= 6; i++)
        {
            Physics.Raycast(gameObject.transform.position, -gameObject.transform.up, out RaycastHit hit);
            if (hit.collider.gameObject.name == ("Building_" + i))
            {
                BuildingManager.Instance.SetCurrentBuildingIndex(i - 1);
                break;
            }else if(hit.collider.gameObject.name == "HighriseBuilding")
            {
                BuildingManager.Instance.SetCurrentBuildingIndex(-1);
                break;
            }
        }
    }
}