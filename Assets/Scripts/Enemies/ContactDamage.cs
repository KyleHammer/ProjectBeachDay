using System;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private EnemyStatsObject stats;
    [SerializeField] private float startingDamage = 1.0f;
    
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(startingDamage * stats.damageScaling);
        }
    }
}
