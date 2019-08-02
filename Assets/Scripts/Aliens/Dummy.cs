using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 100;
    void Start()
    {
        Debug.Log("Dummy spawned.");
        gameObject.layer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= 100;
            Debug.Log("Dummy hit!");
        }
    }
}
