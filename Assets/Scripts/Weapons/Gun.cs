using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    
    [Header("Projectile Stats")]
    [SerializeField] private float projectileSpeed = 20;
    [SerializeField] private float damage = 1;
    
    private void Update()
    {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePosition - weaponPosition;
        transform.right = direction;
    }

    public void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileSpeed;
        newProjectile.GetComponent<Projectile>().SetDamage(damage);
    }
}