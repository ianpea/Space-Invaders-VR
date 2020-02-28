using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_MVC : MonoBehaviour
{
    private static Portal_Database _database = new Portal_Database();
    public static Portal_Database Database { get { return _database; } }

    private static Portal_Controller _controller = new Portal_Controller();
    public static Portal_Controller Controller { get { return _controller; } }

    private static Portal_MVC _instance;
    public static Portal_MVC Instance { get { return _instance; } }

    void Awake()
    {
        Database.PortalGO = gameObject;
    }

    void Update()
    {
        Controller.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Alien")
        {
            Controller.PortalEnter(collision.gameObject);
        }
    }
}
