using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioSource TeleportStart;
    public AudioSource TeleportEnd;
    public AudioSource PlayerFall;
    public AudioSource BulletPlayer;
    public AudioSource BulletEnemy;
    public AudioSource BulletBoss;
    public AudioSource PlayerHit;
    public AudioSource BuildingDestruct;
    public AudioSource AlienExplode;
    public AudioSource AlienSpawn;
    public AudioSource Warning;
    public AudioSource GameplayBGM;

    // UI_Manager only, in MAIN_MENU scene only
    public AudioSource ButtonPressed;
    public AudioSource MainMenuBGM;

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
        
    }
}
