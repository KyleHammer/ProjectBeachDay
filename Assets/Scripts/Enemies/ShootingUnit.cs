using UnityEngine;

public class ShootingUnit : IEnemyDamagable
{
    [Header("Enemy Stats")]
    [SerializeField] private float shootingCooldown = 2.0f;
    private float currentShootingCooldown;
    
    [Header("Assign in Inspector")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    
    [SerializeField] private float projectileSpeed = 20;
    
    [SerializeField] private float startingDamage = 1.0f;
    
    protected override void Start()
    {
        base.Start();
        
        currentShootingCooldown = shootingCooldown / stats.speedScaling;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        SetSpriteDirection();

        if (playerTransfrom != null)
        {
            transform.right = playerTransfrom.position - transform.position;
            
            if (currentShootingCooldown > 0)
            {
                currentShootingCooldown -= Time.deltaTime;

                if (currentShootingCooldown <= 0)
                {
                    Shoot();
                }
            }
        }
        else
        {
            // Currently no player assigned
        }
    }

    private void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDamage(startingDamage * stats.damageScaling);
        
        currentShootingCooldown = shootingCooldown;
    }

    private void SetSpriteDirection()
    {
        if (transform.localRotation.z > 0.7 || transform.localRotation.z < -0.7)
            spriteRenderer.flipY = true;
        else
            spriteRenderer.flipY = false;
    }
}
