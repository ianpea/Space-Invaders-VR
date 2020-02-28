using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDeviceManager : MonoBehaviour
{
    private const int TOTAL_DEVICE = 2;
    public GameObject[] devices;

    private GameObject player;
    private GameObject cam;

    private bool firstDevice = false;
    private bool secondDevice = false;

    public delegate void OnFirstDevice();
    public OnFirstDevice onFirstDevice;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        InitTPDevices();
    }

    // Update is called once per frame
    void Update()
    {
        PlaceDevice();
    }

    private void InitTPDevices()
    {
        devices = new GameObject[2];
        devices = GameObject.FindGameObjectsWithTag("TeleportDevice");
        foreach(GameObject device in devices)
        {
            device.SetActive(false);
        }
        Debug.Log("Teleport Devices initialized");
    }

    private void PlaceDevice()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire3"))
        {
            Physics.Raycast(player.transform.position, cam.transform.forward, out hit);
            if (!firstDevice)
            {
                if(hit.rigidbody != null)
                {
                    devices[0].SetActive(true);
                    devices[0].transform.position = hit.point;
                    firstDevice = true;
                }
            }
            else if(firstDevice && !secondDevice)
            {
                if(hit.rigidbody != null)
                {
                    devices[1].SetActive(true);
                    devices[1].transform.position = hit.point;
                    secondDevice = true;
                }
            }
        }
    }

}
