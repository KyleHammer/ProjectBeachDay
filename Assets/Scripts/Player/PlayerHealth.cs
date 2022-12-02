using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invulnerabilityFrames = 2.5f;
    private float currentInvulnerabilityFrames;
    [SerializeField] private float maxHealth = 5.0f;
    private float currentHealth;

    private bool isGameOver = false;
    private bool isInvulnerable = false;
    private HealthUI healthUI;
    
    // Start is called before the first frame update
    void Start()
    {
        healthUI = GameManager.Instance.GetGameUI().GetComponentInChildren<HealthUI>();
        
        currentHealth = maxHealth;
        SetInvulnerability(true);
        
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
        
        currentInvulnerabilityFrames -= Time.deltaTime;
        
        if (currentInvulnerabilityFrames <= 0)
            SetInvulnerability(false);
    }
    
    public void DealDamage(float damage)
    {
        if (isInvulnerable || isGameOver) return;
        
        currentHealth -= damage;
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
        }
    }
}
