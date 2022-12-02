using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invulnerabilityFrames = 2.5f;
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
        healthUI = GameManager.Instance.GetGameUI().GetComponentInChildren<HealthUI>();
        playerSprite = GetComponent<SpriteRenderer>();
        
        currentHealth = maxHealth;
        healthUI.SetMaxHealth(maxHealth);
        healthUI.SetHealth(currentHealth);
        
        SetInvulnerability(true);
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
            SetInvulnerability(false);
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
            SetInvulnerability(true);
    }

    private void GameOver()
    {
        SetInvulnerability(true);
        isGameOver = true;
        
        Debug.Log("Game Over");
    }

    private void SetInvulnerability(bool newInvulnerabilityFrames)
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
