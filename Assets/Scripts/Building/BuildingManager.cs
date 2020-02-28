using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager _instance;
    public static BuildingManager Instance { get { return _instance; } }

    /// <summary>
    /// List of buildings.
    /// </summary>
    public List<Building> Buildings;
    private GameObject[] temp;
    public bool isMainBuildingExist = true;

    /// <summary>
    /// Current building player is on.
    /// </summary>
    public int currentBuildingIndex = -1;

    /// <summary>
    /// Current building player is on.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Main building.
    /// </summary>
    public Building mainBuilding;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    int index = 0;
    void Start()
    {
        Instance.temp = GameObject.FindGameObjectsWithTag("Building"); // List of buildings in scene
        player = GameObject.FindGameObjectWithTag("Player");
        mainBuilding = GameObject.Find("HighriseBuilding").GetComponent<Building>();
        
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < Instance.temp.Length; j++)
            {
                index = int.Parse(Instance.temp[j].name.Substring(Instance.temp[j].name.Length - 1)) - 1;
                if (index == i)
                {
                    Instance.Buildings.Add(Instance.temp[i].GetComponent<Building>());
                    
                }
            }
        }
    }

    void Update()
    {
        //Debug.Log(GetCurrentBuildingIndex());
    }
    
    public Building GetCurrentBuilding()
    {
        if (Instance.GetCurrentBuildingIndex() == -1) // Main building is -1
        {
            return Instance.mainBuilding;
        }
        else
        {
            return Instance.Buildings[Instance.GetCurrentBuildingIndex()];
        }
    }
    
    public int GetCurrentBuildingIndex()
    {
        return Instance.currentBuildingIndex;
    }

    public void SetCurrentBuildingIndex(int index)
    {
        Instance.currentBuildingIndex = index;
    }

    public Building GetMainBuilding()
    {
        return Instance.mainBuilding;
    }
}



public class Building_Database
{
    // Building Game Object
    public GameObject buildingGO;
    public Rigidbody buildingRB;
    public MeshRenderer buildingMR;

    public const int BUILDING_HP_MAX = 6;
    public const int BUILDING_HP_MAX_EXTRA = 10;

    // Building stats
    public string buildingName;
    public int buildingIndex;
    public int currentHP;


    private bool _isDestroyed = false;
    public bool IsDestroyed { get { return _isDestroyed; } set { _isDestroyed = value; } }

    private bool _isSetHP = false;
    public bool IsSetHP { get { return _isSetHP; } set { _isSetHP = value; } }
}
