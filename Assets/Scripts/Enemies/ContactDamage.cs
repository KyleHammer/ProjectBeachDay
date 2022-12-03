using System;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private EnemyStatsObject stats;
    [SerializeField] private float damage = 1.0f;

    private void Start()
    {
        damage *= stats.damageScaling;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
