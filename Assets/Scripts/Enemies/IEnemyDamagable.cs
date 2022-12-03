using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemyDamagable : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private float flashDuration = 0.1f;
    private float currentFlashDuration = 0;
    
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth = 0;

    protected Rigidbody2D rb;
    
    protected Transform playerTransfrom;

    private AudioSource hitSFX;

    protected virtual void Start()
    {
        GameManager.Instance.AddEnemy(this.gameObject);

        rb = GetComponent<Rigidbody2D>();
        hitSFX = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        FlashUpdate();
    }
    
    public void SetTargetPlayer(Transform newPlayerTransform)
    {
        playerTransfrom = newPlayerTransform;
    }

    public void TakeDamage(float damageTaken)
    {
        hitSFX.Play();
        
        currentHealth -= damageTaken;
        
        if (currentHealth > 0)
        {
            BeginFlash();
        }
        else
        {
            GameManager.Instance.RemoveEnemy(this.gameObject);
            GameManager.Instance.PlayAudio("BasicHit");
            GameManager.Instance.PlayAudio("Death");
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
