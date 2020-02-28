using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCargoShip : Alien
{
    public int hp = 1600;
    public const int EXP = 35;
    public const float SPEED = 190f;
    
    private Rigidbody rb;
    private GameObject player;
    private BuildingManager BuildingManager;
    private AudioManager AudioManager;
    public ParticleSystem particle;

    void Start()
    {
        gameObject.layer = 2;
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        BuildingManager = BuildingManager.Instance;
        AudioManager = AudioManager.Instance;
    }
    
    void Update()
    {
        IsAlive();
        MoveToTarget();
    }


    private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(collision);
    }

    public override void IsAlive()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            AudioManager.AlienExplode.Play();
            ScoreSystem.Instance.onAlienKilled?.Invoke();
            Weapon.Instance.onExpGain?.Invoke(EXP);
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }

    public override void MoveToTarget()
    {
        Building currentBuilding = BuildingManager.GetCurrentBuilding();
        if (currentBuilding.Info.currentHP <= 2)
        {
            MoveToPlayer();
        }
        else
        {
            MoveToBuilding();
        }
    }

    public override void TakeDamage(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= Weapon.Instance.damage;
            BulletPool.Instance.RecycleBullet(collision.gameObject);
        }
    }
    
    public override void MoveToBuilding()
    {
        MeshRenderer buildingMR = BuildingManager.GetCurrentBuilding().GetMeshRenderer();
        if (BuildingManager.GetCurrentBuildingIndex() != -1)
        {
            Vector3 distToBuilding = buildingMR.bounds.center + new Vector3(0, buildingMR.bounds.extents.y, 0) - transform.position;
            rb.velocity = distToBuilding.normalized * Time.deltaTime * SPEED * Mathf.Min(distToBuilding.magnitude/Player.ENEMY_SLOW_RADIUS, 1);
        }
        else
        {
            Vector3 distToBuilding = buildingMR.bounds.center + new Vector3(0, buildingMR.bounds.extents.y/2, 0) - transform.position;
            rb.velocity = distToBuilding.normalized * Time.deltaTime * SPEED * Mathf.Min(distToBuilding.magnitude / Player.ENEMY_SLOW_RADIUS, 1);
        }
    }

    public override void MoveToPlayer()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * Time.deltaTime * 250f;
    }

    public override void SelfDestruct()
    {
        Destroy(gameObject);
        AudioManager.AlienExplode.Play();
        ScoreSystem.Instance.onBuildingCollide.Invoke();
        Instantiate(particle, transform.position, Quaternion.identity);
    }

    public override void LookAtPlayer()
    {
        gameObject.transform.LookAt(player.transform);
    }
}
