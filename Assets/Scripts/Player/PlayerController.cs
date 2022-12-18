using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private float dashSpeed = 15f;
    
    [SerializeField] private List<TrailRenderer> trailRenderers = new List<TrailRenderer>();
    
    [Header("Sound Effects")]
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private AudioSource walkSFX;

    private DashUI dashUI;
    private bool canDash = true;
    private float currentDashCooldown;
    private float dashTime = 0;
    private Vector2 dashDirection = Vector2.zero;
    private PlayerHealth playerHealth;

    private bool movementEnabled = true;
    private bool shootingEnabled = true;
    private Vector2 movementInput = Vector2.zero;
    private Gun gun;
    
    private bool shootButtonHeld = false;
    
    private void Start()
    {
        GameManager.Instance.SetPlayer(this.gameObject);
        
        DisableTrail();
        
        gun = GetComponentInChildren<Gun>();
        playerHealth = GetComponent<PlayerHealth>();
        
        StartCoroutine(LateStart());
    }
    
    // gameUI is assigned in start
    // So PlayerController needs to get gameUI after it has been assigned (through LateStart)
    private IEnumerator LateStart()
    {
        // Wait 1 frame after being called
        yield return 0;
        
        dashUI = GameManager.Instance.GetGameUI().GetComponentInChildren<DashUI>();
        dashUI.SetMaxDashValue(currentStats.dashCooldown);
        dashUI.SetDashValue(currentDashCooldown);
    }
    
    private void Update()
    {
        CheckShootButton();
    }

    private void FixedUpdate()
    {
        rb.velocity = movementInput * currentStats.speed;
        
        animator.SetFloat("moveSpeed", rb.velocity.magnitude / 2);
        
        SetSpriteDirection();
        DashUpdate();
        PlayWalkSound();
    }
    
    public void Quit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.Quit();
        }
    }

    public void ForceRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.Restart();
        }
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        if (!movementEnabled)
            return;
    
        movementInput = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }
    
    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started || !movementEnabled || !canDash)
            return;

        canDash = false;
        currentDashCooldown = currentStats.dashCooldown;
        playerHealth.SetInvulnerability(true, currentStats.dashDuration + 0.5f);
        
        EnableTrail();
            
        dashDirection = movementInput.normalized;
        dashTime = currentStats.dashDuration;
            
        if(!dashSFX.isPlaying)
        {
            dashSFX.Play();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!shootingEnabled)
            return;
        
        if (context.performed)
            shootButtonHeld = true;
        if (context.canceled)
            shootButtonHeld = false;
    }
    
    private void CheckShootButton()
    {
        if (shootButtonHeld)
            gun.Shoot();
    }
    
    public void EnableMovement()
    {
        movementEnabled = true;
    }
    
    public void DisableMovementShootingAndSprite()
    {
        rb.velocity = Vector2.zero;
        movementInput = Vector2.zero;

        sprite.enabled = false;
        
        movementEnabled = false;
        shootingEnabled = false;
    }

    public void EnableTrail()
    {
        foreach (var trail in trailRenderers)
        {
            trail.emitting = true;
        }
    }

    public void DisableTrail()
    {
        foreach (var trail in trailRenderers)
        {
            trail.emitting = false;
        }
    }

    private void DashUpdate()
    {
        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
            dashUI.SetDashValue(currentDashCooldown);
            
            if (currentDashCooldown < 0)
            {
                currentDashCooldown = 0;
                canDash = true;
            }
        }
        
        if (dashDirection == Vector2.zero)
            return;
        
        if (dashTime <= 0)
        {
            DisableTrail();
        
            dashDirection = Vector2.zero;
            dashTime = 0;
            rb.velocity = Vector2.zero;
        }
        else
        {
            dashTime -= Time.deltaTime;
            
            dashDirection = movementInput.normalized;
            
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    private void SetSpriteDirection()
    {
        if (rb.velocity.x > 0)
            sprite.flipX = false;
        else if(rb.velocity.x < 0)
            sprite.flipX = true;
    }

    private void PlayWalkSound()
    {
        if (movementInput != Vector2.zero && !walkSFX.isPlaying && !dashSFX.isPlaying)
        {
            walkSFX.pitch = Random.Range(0.9f, 1.1f);
            walkSFX.Play();
        }
    }
}
