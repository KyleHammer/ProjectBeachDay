using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isPlayerProjectile = true;
    
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public ParticleSystem trailParticle;

    private float damage = 1f;
    private float bulletLifetime = 2.0f;

    private float destroyDelay = 2.0f;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        bulletLifetime -= Time.deltaTime;
        if(bulletLifetime < 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Hole")) return;
        
        if (col.transform.CompareTag("Player"))
        {
            if(!isPlayerProjectile)
                col.GetComponent<PlayerHealth>().TakeDamage(damage);
            else
                return;
        }
        else if (col.transform.CompareTag("Enemy"))
        {
            if(isPlayerProjectile)
                col.GetComponent<IEnemyDamagable>().TakeDamage(damage);
            else
                return;
        }
        
        RemoveProjectile();
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    
    private void RemoveProjectile()
    {
        sr.enabled = false;
        boxCollider.enabled = false;

        if(trailParticle != null)
        {
            trailParticle.Stop();
        }

        StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(destroyDelay);
        
        Destroy(this.gameObject);
    }
}
