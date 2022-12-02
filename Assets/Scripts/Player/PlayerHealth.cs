using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invulnerabilityFrames = 2.5f;
    private float currentInvulnerabilityFrames;
    [SerializeField] private float maxHealth = 5.0f;
    private float currentHealth;

    private bool isInvulnerable = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        SetInvulnerability(true);
    }

    // Update is called once per frame
    void Update()
    {
        InvulFrameCheck();
    }

    private void InvulFrameCheck()
    {
        if (!isInvulnerable) return;
        
        currentInvulnerabilityFrames -= Time.deltaTime;
        
        if (currentInvulnerabilityFrames <= 0)
            SetInvulnerability(false);
    }
    
    public void DealDamage(float damage)
    {
        if (isInvulnerable) return;
        
        currentHealth -= damage;
        Debug.Log("Damage Dealt");

        if (currentHealth <= 0)
            GameOver();
        else
            SetInvulnerability(true);
    }

    private void GameOver()
    {
        Debug.Log("You Died");
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
