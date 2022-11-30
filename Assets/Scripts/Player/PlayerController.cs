using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 7f;
    
    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 3.0f;
    [SerializeField] private List<TrailRenderer> trailRenderers = new List<TrailRenderer>();
    [SerializeField] private AudioSource dashSFX;
    [SerializeField] private AudioSource walkSFX;

    private bool canDash = true;
    private float currentDashCooldown;
    private float dashTime = 0;
    private Vector2 dashDirection = Vector2.zero;

    private bool movementEnabled = true;
    private Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        DisableTrail();
    }

    private void FixedUpdate()
    {
        rb.velocity = movementInput * moveSpeed;
        
        animator.SetFloat("moveSpeed", rb.velocity.magnitude / 2);
        
        SetSpriteDirection();
        DashUpdate();
        PlayWalkSound();
    }

    public void ForceRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Reset logic here
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
        currentDashCooldown = dashCooldown;
        
        EnableTrail();
            
        dashDirection = movementInput.normalized;
        dashTime = dashDuration;
            
        if(!dashSFX.isPlaying)
        {
            dashSFX.Play();
        }
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }
    
    public void DisableMovement()
    {
        rb.velocity = Vector2.zero;
        movementInput = Vector2.zero;
        
        movementEnabled = false;
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
            Debug.Log(currentDashCooldown);
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
