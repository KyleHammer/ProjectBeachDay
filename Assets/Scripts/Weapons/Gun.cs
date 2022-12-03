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

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePosition - weaponPosition;
        transform.right = direction;
        
        if (transform.localRotation.z > 0.7 || transform.localRotation.z < -0.7)
            sr.flipY = true;
        else
            sr.flipY = false;
    }

    public void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDamage(currentStats.damage);
    }

    public void IncreaseDamage(float increase)
    {
        currentStats.damage += increase;
    }
}