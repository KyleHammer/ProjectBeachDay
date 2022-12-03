using UnityEngine;

public class DashingUnit : IEnemyDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] private float dashCooldown = 2.0f;
    [SerializeField] private float dashForce = 200.0f;
    private float currentDashCooldown;

    protected override void Start()
    {
        base.Start();
        currentDashCooldown = dashCooldown;
    }

    protected override void Update()
    {
        base.Update();
        
        if (playerTransfrom != null)
        {
            if (currentDashCooldown > 0)
            {
                currentDashCooldown -= Time.deltaTime;

                if (currentDashCooldown <= 0)
                {
                    PerformDash();
                }
            }
        }
        else
        {
            // Currently no player assigned
        }
    }

    private void PerformDash()
    {
        Vector2 direction = Vector2.ClampMagnitude(playerTransfrom.position - transform.position, 1);

        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        rb.AddForce(direction * dashForce);
        
        currentDashCooldown = dashCooldown;
    }
}
