using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building: MonoBehaviour
{
    #region FIELDS
    
    public Building_Database Info = new Building_Database();
    private BuildingManager BuildingManager;

    private AudioManager AudioManager;

    #endregion

    #region START/UPDATE

    private void Start()
    {
        BuildingManager = BuildingManager.Instance;
        AudioManager = AudioManager.Instance;
        Info.buildingGO = gameObject;
        Info.buildingMR = gameObject.GetComponentInChildren<MeshRenderer>();
        Info.buildingName = gameObject.name;
        Info.currentHP = Building_Database.BUILDING_HP_MAX;
        Info.buildingRB = Info.buildingGO.GetComponent<Rigidbody>();

        // Building Index == gameObject.name == Building_*INDEX*, example "Building_1, index is 1."
        if (Info.buildingName == "HighriseBuilding")
        {
            Info.buildingIndex = -1;
        }
        else // == Building_*INDEX*
        {
            Info.buildingIndex = int.Parse(Info.buildingName.Substring(Info.buildingName.Length - 1)) - 1;
        }
        SetHP();
    }

    private void Update()
    {
        Demolish();
    }

    #endregion

    #region FUNCTIONS

    private void SetHP()
    {
        if (!Info.IsSetHP && Info.buildingName == "HighriseBuilding")
        {
            Info.currentHP = Building_Database.BUILDING_HP_MAX_EXTRA;
            Info.IsSetHP = true;
        }
    }

    public void Demolish()
    {
        if (Info.currentHP <= 0 && !Info.IsDestroyed)
        {
            Info.IsDestroyed = true;
            Info.buildingRB.isKinematic = false;
            Info.buildingRB.useGravity = true;
            if (AudioManager.BuildingDestruct.isPlaying)
            {
                AudioManager.BuildingDestruct.Play();
                Debug.Log("BuildingDestruct audio playing.");
                //Handheld.Vibrate();
            }
            else
            {
                AudioManager.BuildingDestruct.Play();
            }
            if(Info.buildingIndex == -1 && BuildingManager.isMainBuildingExist)
            {
                Debug.Log("Main building destroyed");
                foreach(Building b in BuildingManager.Buildings)
                {
                    b.Info.currentHP -= 2;
                }
                BuildingManager.isMainBuildingExist = false;
                
            }
        }
    }

    public void DestroyBuilding()
    {
        if(Info.buildingGO.transform.position.y < -50)
        {
            Info.buildingGO.SetActive(false);
        }
    }

    public void EnemyHit(GameObject enemy)
    {
        AudioManager.AlienExplode.Play();
        enemy.SetActive(false);
        if(enemy.name == "AlienCargoShip(Clone)")
        {
            Info.currentHP -= 2;
        }
        else
        {
            Info.currentHP--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Alien")
        {
            EnemyHit(collision.gameObject);
            ScoreSystem.Instance.onBuildingCollide?.Invoke();
        }

        if(collision.gameObject.tag == "EnemyBullet")
        {
            //Debug.Log("BULLET HIT BUILDING");
            BulletPool.Instance.RecycleBullet(collision.gameObject);
        }
    }

    public MeshRenderer GetMeshRenderer()
    {
        return Info.buildingMR;
    }
    #endregion
}

