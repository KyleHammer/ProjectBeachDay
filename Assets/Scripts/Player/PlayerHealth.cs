using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float hitInvulnerabilityFrames = 2.5f;
    private float currentInvulnerabilityFrames;
    private SpriteRenderer playerSprite;
    private float flashSpeed = 0.1f;
    private bool isInvulnerable = false;

    [SerializeField] private float maxHealth = 5.0f;
    private float currentHealth;

    private bool isGameOver = false;
    
    private HealthUI healthUI;

    [SerializeField] private AudioSource hitSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        
        currentHealth = maxHealth;
        
        SetInvulnerability(true, hitInvulnerabilityFrames);
        
        StartCoroutine(LateStart());
    }

    // gameUI is assigned in start
    // So healthUI needs to get gameUI after it has been assigned (through LateStart)
    private IEnumerator LateStart()
    {
        // Wait 1 frame after being called
        yield return 0;
        
        healthUI = GameManager.Instance.GetGameUI().GetComponentInChildren<HealthUI>();
        healthUI.SetMaxHealth(maxHealth);
        healthUI.SetHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        InvulFrameCheck();
    }

    private void InvulFrameCheck()
    {
        if (!isInvulnerable || isGameOver) return;

        PlayerFlash();

        currentInvulnerabilityFrames -= Time.deltaTime;
        
        if (currentInvulnerabilityFrames <= 0)
            SetInvulnerability(false, 0);
    }

    private void PlayerFlash()
    {
        if (Time.timeSinceLevelLoad % flashSpeed > flashSpeed / 2)
            playerSprite.enabled = false;
        else
            playerSprite.enabled = true;
    }
    
    public void TakeDamage(float damage)
    {
        if (isInvulnerable || isGameOver) return;
        
        currentHealth -= damage;
        hitSFX.Play();
        healthUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
            GameOver();
        else
            SetInvulnerability(true, hitInvulnerabilityFrames);
    }

    private void GameOver()
    {
        SetInvulnerability(true, 0);
        isGameOver = true;
        
        Debug.Log("Game Over");
    }

    public void SetInvulnerability(bool newInvulnerabilityFrames, float invulnerabilityFrames)
    {
        if (newInvulnerabilityFrames)
        {
            isInvulnerable = true;
            currentInvulnerabilityFrames = invulnerabilityFrames;
        }
        else
        {
            isInvulnerable = false;
            currentInvulnerabilityFrames = 0;
            playerSprite.enabled = true;
        }
    }
}
