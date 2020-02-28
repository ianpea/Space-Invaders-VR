using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // Singleton
    private static WaveSystem _instance;
    public static WaveSystem Instance { get { return _instance; } }


    public GameObject AlienCargoShip;
    public GameObject AlienFighter;
    public GameObject AlienBoss;

    private List<Wave> Tutorial = new List<Wave>();

    public delegate void OnSpawn(int waveIndex);
    public OnSpawn onSpawn;

    private const int WAVE_COOLDOWN = 13;
    private float lastSpawnTime = -5;
    private bool bossSpawned = false;
    private AudioManager AudioManager;

    void Awake()
    {
        // Singleton
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        AlienBoss = GameObject.FindGameObjectWithTag("Boss");
    }

    private void Start()
    {
        Initialize();
        AssignDelegate();

        AudioManager = AudioManager.Instance;

    }
    void Update()
    {
        SpawnWaveHandler();
    }

    void AssignDelegate()
    {
        onSpawn = SpawnWave;
    }

    void Initialize()
    {
        Spawner.Start();
        AlienCargoShip = Resources.Load("Prefabs/AlienCargoShip") as GameObject;
        AlienFighter = Resources.Load("Prefabs/AlienFighter") as GameObject;
        SetWave();
    }

    void SpawnWave(int waveIndex)
    {
        for (int j = 0; j < Tutorial[waveIndex].unit.Count; j++)
        {
            Instantiate(Tutorial[waveIndex].unit[j].unitGet, Tutorial[waveIndex].unit[j].position, Tutorial[waveIndex].unit[j].rotation);
        }
        Tutorial[waveIndex].IsSpawned = true;
        AudioManager.AlienSpawn.Play();
    }

    void SpawnWaveHandler()
    {
        for (int i = 0; i < Tutorial.Count; i++)
        {
            if(Time.time - lastSpawnTime >= WAVE_COOLDOWN)
            {
                if (!Tutorial[i].IsSpawned)
                {
                    onSpawn.Invoke(i);
                    lastSpawnTime = Time.time;
                }
                else
                {
                    //Debug.Log("Not spawning, in cooldown.");
                }
            }
        }

        if (Time.time - lastSpawnTime >= (WAVE_COOLDOWN + 5) && 
            Tutorial[Tutorial.Count - 1].IsSpawned && !bossSpawned)
        {
            bossSpawned = true;
            AlienBoss.SetActive(true);
            lastSpawnTime = Time.time;
        }
    }

    void SetWave()
    {
        // TutorialWave1
        List<Units> tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 3 && j < 3; i++, j++)
        {
            tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            if (j == 2) j = 0;
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });
        // TutorialWave2
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 3 && j < 3; i++, j++)
        {
            tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            if (j == 2) j = 0;
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });
        // TutorialWave3
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 3 && j < 3; i++, j++)
        {
            if(j == 0)
            {
                tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if (j == 1)
            {
                tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if(j == 2)
            {
                tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
                j = 0;
            }
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });
        // TutorialWave4
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 6 && j < 3; i++, j++)
        {
            tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            if (j == 2)
            {
                j = 0;
            }
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });
        // TutorialWave5
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 6 && j < 3; i++, j++)
        {
            tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            if (j == 2)
            {
                j = 0;
            }
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });

        // TutorialWave6
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 12 && j < 3; i++, j++)
        {
            if (j == 0)
            {
                tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if (j == 1)
            {
                tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if (j == 2)
            {
                tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
                j = 0;
            }
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });

        // TutorialWave7
        tempUnits = new List<Units>();
        for (int i = 0, j = 0; i < 15 && j < 3; i++, j++)
        {
            if (j == 0)
            {
                tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if (j == 1)
            {
                tempUnits.Add(new Units { unitGet = AlienCargoShip, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
            }
            if (j == 2)
            {
                tempUnits.Add(new Units { unitGet = AlienFighter, position = Spawner.SpawnPoints[j], rotation = Quaternion.identity });
                j = 0;
            }
        }
        Tutorial.Add(new Wave { unit = tempUnits, IsSpawned = false });
    }
}
