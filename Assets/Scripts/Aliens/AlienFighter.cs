using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienFighter : Alien
{
    // Start is called before the first frame update
    public int hp = 550;
    public const int EXP = 50;
    public const float SPEED = 205f;
    
    // Shoot at player
    private const float FIRE_CD = 3f;
    private float lastFireTime = 0;

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
        LookAtPlayer();
        FireAtPlayer();
    }


    public void OnCollisionEnter(Collision collision)
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
    
    /// <summary>
    /// Aim for player if player too close.
    /// </summary>
    public override void MoveToTarget()
    {
        if ((transform.position - player.transform.position).magnitude < 5f)
        {
            MoveToBuilding();
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

    /// <summary>
    /// If player near portal, will MoveToPlayer,
    /// else aim for MoveToPortal.
    /// </summary>
    /// <returns></returns>
    public override void MoveToBuilding()
    {
        MeshRenderer buildingMR = BuildingManager.GetCurrentBuilding().GetMeshRenderer();
        if (BuildingManager.GetCurrentBuildingIndex() != -1)
        {
            Vector3 distToBuilding = buildingMR.bounds.center + new Vector3(0, buildingMR.bounds.extents.y, 0) - transform.position;
            rb.velocity = distToBuilding.normalized * Time.deltaTime * SPEED * Mathf.Min(distToBuilding.magnitude / Player.ENEMY_SLOW_RADIUS, 1);
        }
        else
        {
            Vector3 distToBuilding = buildingMR.bounds.center + new Vector3(0, buildingMR.bounds.extents.y / 2, 0) - transform.position;
            rb.velocity = distToBuilding.normalized * Time.deltaTime * SPEED * Mathf.Min(distToBuilding.magnitude / Player.ENEMY_SLOW_RADIUS, 1);
        }
    }

    public override void MoveToPlayer()
    {
        rb.velocity = (player.transform.position - transform.position) * Time.deltaTime * 10f;
    }

    public override void FireAtPlayer()
    {
        if(Time.time - lastFireTime >= FIRE_CD)
        {
            lastFireTime = Time.time;
            BulletPool.FireAtPlayer(gameObject, player);
            if (AudioManager.BulletEnemy.isPlaying)
            {
                AudioManager.BulletEnemy.Stop();
                AudioManager.BulletEnemy.Play();
            }
            else
            {
                AudioManager.BulletEnemy.Play();
            }
        }
    }

    public override void LookAtPlayer()
    {
        gameObject.transform.LookAt(player.transform);
    }
}
