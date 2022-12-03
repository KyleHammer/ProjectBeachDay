using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private float damage = 1f;

    private float destroyDelay = 2.0f;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.transform.CompareTag("Player"))
        {
            if (col.transform.CompareTag("Enemy"))
            {
                col.GetComponent<IEnemyDamagable>().TakeDamage(damage);
            }

            RemoveProjectile();
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    
    private void RemoveProjectile()
    {
        sr.enabled = false;
        boxCollider.enabled = false;

        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }

        StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(destroyDelay);
        
        Destroy(this.gameObject);
    }
}
