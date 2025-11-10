using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WalkingUnit : IEnemyDamagable
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float contactDamage = 1.0f;

    private void FixedUpdate()
    {
        if (playerTransfrom != null)
        {
            float step = GetSpeed() * Time.deltaTime;

            Vector2 direction = Vector2.ClampMagnitude(playerTransfrom.position - transform.position, 1);
            
            rb.MovePosition((Vector2) transform.position + direction * step);
        }
        else
        {
            // Currently no player assigned
        }
    }

    private float GetSpeed()
    {
        return moveSpeed + (speedScaling * GameManager.Instance.GetDifficulty());
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(contactDamage + (damageScaling * GameManager.Instance.GetDifficulty()));
        }
    }
}
