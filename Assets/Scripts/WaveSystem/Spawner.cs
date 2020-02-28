using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    public static List<Vector3> SpawnPoints;
    public static void Start()
    {
        SpawnPoints = new List<Vector3>();
        SpawnPoints.Add(new Vector3(-22.0f, 9.749947f, -23.448345f));
        SpawnPoints.Add(new Vector3(-0.3999996f, 17.18995f, -21.34834f));
        SpawnPoints.Add(new Vector3(22.1f, 9.749947f, -23.448345f));
    }
}
