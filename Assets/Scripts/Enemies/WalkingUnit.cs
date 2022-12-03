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
            transform.position = Vector2.MoveTowards(transform.position, playerTransfrom.position, step);
        }
        else
        {
            // Currently no player assigned
        }
    }
}
