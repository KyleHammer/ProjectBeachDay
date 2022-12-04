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
        SetGunDirection();
        SetSpriteDirection();
    }

    public void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        Debug.Log("Bang");
        
        // TODO: Implement bullet movement
        
        // Set the projectile velocity

        // Set the projectile damage
    }

    private void SetGunDirection()
    {
        // TODO: Implement gun rotation
        
        // Find the mouse position on the screen

        // Get the position of the gun

        // Get the direction the bullet needs to go (end point - start point)

        // Set the gun transform's right side to the direction
        // This is because the gun faces right by default
        
    }

    private void SetSpriteDirection()
    {
        if (transform.localRotation.z > 0.7 || transform.localRotation.z < -0.7)
            sr.flipY = true;
        else
            sr.flipY = false;
    }

    public void IncreaseDamage(float increase)
    {
        currentStats.damage += increase;
    }
}