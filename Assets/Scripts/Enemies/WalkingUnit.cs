using UnityEngine;

public class WalkingUnit : IEnemyDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] private float moveSpeed = 3;

    private void FixedUpdate()
    {
        if (playerTransfrom != null)
        {
            float step =  moveSpeed * Time.deltaTime;

            Vector2 direction = Vector2.ClampMagnitude(playerTransfrom.position - transform.position, 1);
            
            rb.MovePosition((Vector2) transform.position + direction * step);
        }
        else
        {
            // Currently no player assigned
        }
    }
}
