using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [SerializeField] private float hitInvulnerabilityFrames = 1.5f;
    private float currentInvulnerabilityFrames;
    private SpriteRenderer playerSprite;
    private float flashSpeed = 0.1f;
    private bool isInvulnerable = false;

    private bool isGameOver = false;
    
    private HealthUI healthUI;

    [SerializeField] private AudioSource hitSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        
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
        healthUI.SetMaxHealth(currentStats.maxHealth);
        healthUI.SetHealth(currentStats.health);
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
        
        currentStats.health -= damage;
        hitSFX.Play();
        healthUI.SetHealth(currentStats.health);

        if (currentStats.health <= 0)
            GameOver();
        else
            SetInvulnerability(true, hitInvulnerabilityFrames);
    }

    public void IncreaseMaxHealth(float increase)
    {
        currentStats.maxHealth += increase;

        healthUI.SetMaxHealth(currentStats.maxHealth);
        healthUI.SetHealth(currentStats.health);
    }
    
    public void IncreaseHealth(float increase)
    {
        if (currentStats.health + increase > currentStats.maxHealth) return;
        
        currentStats.health += increase;
        
        healthUI.SetHealth(currentStats.health);
    }

    private void GameOver()
    {
        healthUI.SetHealth(0);
        SetInvulnerability(true, 0);
        isGameOver = true;
        
        GameManager.Instance.GameOver();
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
