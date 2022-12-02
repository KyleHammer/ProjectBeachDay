using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemyDamagable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private float flashDuration = 0.1f;
    private float currentFlashDuration = 0;
    
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth = 0;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        FlashUpdate();
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        
        if (currentHealth > 0)
        {
            BeginFlash();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BeginFlash()
    {
        spriteRenderer.material = flashMaterial;
        currentFlashDuration = flashDuration;
    }
    
    private void FlashUpdate()
    {
        if (currentFlashDuration > 0)
        {
            currentFlashDuration -= Time.deltaTime;
            if (currentFlashDuration <= 0)
            {
                spriteRenderer.material = defaultMaterial;
            }
        }
    }
}
