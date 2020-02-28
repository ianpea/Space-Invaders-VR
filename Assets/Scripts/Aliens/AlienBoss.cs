using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : Alien
{
    public int hp = 9000;
    public const int xpPerHit = 10;
    public bool isMovingToPlayer;

    // Shoot
     const float FIRECD_INIT = 1f;
     const float FIRECD_CYCLE = 3f; // Increase fire rate every 3 seconds.
     const float FIRECD_MAX = 0.5f;
     const float FIRE_CD_INCREMENT = 0.05f;
     float fireCDCurrent = FIRECD_INIT;
     float lastFireTime = 0;
     float lastFireCDIncrement = 0;

    // Hover
    const float HOVER_INIT = 0.3f;
    const float HOVER_CYCLE = 2.5f;
    const float HOVER_MAX = 10.0f;
    const float HOVER_INCREMENT = 0.5f;
    float hoverCurrent = HOVER_INIT;
    float lastHover = 0;
    float lastHoverIncrement = 0;

     Rigidbody rb;
     GameObject player;
     GameObject portal;
    public ParticleSystem particle;
     AudioManager AudioManager;

    void Start()
    {
        AudioManager = AudioManager.Instance;
        gameObject.SetActive(false);
        gameObject.layer = 2;
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        portal = GameObject.FindGameObjectWithTag("Portal");
        gameObject.transform.position = new Vector3(0, 3, -12);
    }

    void Update()
    {
        IsAlive();
        LookAtPlayer();
        FireAtPlayer();
    }

     void FixedUpdate()
    {
        Hover();
        IncreaseFireRateOvertime();
        IncreaseHoverRateOvertime();
    }


     void OnCollisionEnter(Collision collision)
    {
        TakeDamage(collision);
    }

    
    public override void FireAtPlayer()
    {
        if (Time.time - lastFireTime >= fireCDCurrent)
        {
            lastFireTime = Time.time;
            BulletPool.FireAtPlayer(gameObject, player);
            if (AudioManager.BulletBoss.isPlaying)
            {
                AudioManager.BulletBoss.Stop();
                AudioManager.BulletBoss.Play();
            }
            else
            {
                AudioManager.BulletBoss.Play();
            }
        }
    }

    public override void IsAlive()
    {
        if (hp <= 0)
        {
            Gameplay.Instance.onBossDead?.Invoke(true);
            Destroy(gameObject);
            Instantiate(particle, transform.position, Quaternion.identity);
            AudioManager.AlienExplode.Play();
        }
    }

    public override void TakeDamage(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= Weapon.Instance.damage;
            BulletPool.Instance.RecycleBullet(collision.gameObject);
            ScoreSystem.Instance.onAlienKilled?.Invoke();
            Weapon.Instance.onExpGain?.Invoke(xpPerHit);
        }
    }

    void IncreaseFireRateOvertime()
    {
        if(fireCDCurrent < FIRECD_MAX && Time.time - lastFireCDIncrement >= FIRECD_CYCLE)
        {
            lastFireCDIncrement = Time.time;
            fireCDCurrent -= FIRE_CD_INCREMENT;
        }
    }

    void IncreaseHoverRateOvertime()
    {
        if (hoverCurrent < HOVER_MAX && Time.time - lastHoverIncrement >= HOVER_CYCLE)
        {
            lastHoverIncrement = Time.time;
            hoverCurrent += HOVER_INCREMENT;
        }
    }
    public override void Hover()
    {
        gameObject.transform.position += (new Vector3(0, hoverCurrent, 0) * Mathf.Cos(Time.time)) / 60;
    }

    public override void LookAtPlayer()
    {
        gameObject.transform.LookAt(player.transform);
    }
}
