using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Cargo_Ship : MonoBehaviour
{
    public int hp = 1200;
    public int onHitDamage = 200;
    public int movementSpeeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawned Alien_Cargo_Ship.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetPlayerPos()
    {
        //get player position

        return Vector3.zero;
    }

    private void MoveToPlayer()
    {
        //move to player position
    }
}
