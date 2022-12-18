using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [Header("Assign in Inspector")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    
    [SerializeField] private float projectileSpeed = 20;
    
    [SerializeField] private float fireRateCooldown = 0.4f;
    private float currentCooldown = 0f;
    private bool canShoot = true;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetGunDirection();
        SetSpriteDirection();
        CooldownUpdate();
    }

    private void CooldownUpdate()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                canShoot = true;
            }
        }
    }

    private void SetGunDirection()
    {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePosition - weaponPosition;
        transform.right = direction;
    }
    
    private void SetSpriteDirection()
    {
        if (transform.localRotation.z > 0.7 || transform.localRotation.z < -0.7)
            sr.flipY = true;
        else
            sr.flipY = false;
    }

    public void Shoot()
    {
        if (!canShoot) return;
        
        canShoot = false;
        currentCooldown = fireRateCooldown;
        
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDamage(currentStats.damage);
    }

    public void IncreaseDamage(float increase)
    {
        currentStats.damage += increase;
    }
}