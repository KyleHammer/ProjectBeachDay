using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float damage = 1f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.transform.CompareTag("Player"))
        {
            if (col.transform.CompareTag("Enemy"))
            {
                col.GetComponent<IEnemyDamagable>().TakeDamage(damage);
            }
            
            Destroy(this.gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
