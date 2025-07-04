using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [SerializeField] private AudioSource upgradeSFX;
    
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Upgrade(UpgradeTypeObject upgradeType)
    {
        upgradeSFX.Play();
        
        switch (upgradeType.upgradeName)
        {
            case "maxHealth":
                IncreaseMaxHealth(upgradeType.upgradeIncrease);
                break;
            case "health":
                RegenHealth(upgradeType.upgradeIncrease);
                break;
            case "damage":
                IncreaseDamage(upgradeType.upgradeIncrease);
                break;
            case "dashCooldown":
                DecreaseDashCooldown(upgradeType.upgradeIncrease);
                break;
            case "dashDuration":
                IncreaseDashDuration(upgradeType.upgradeIncrease);
                break;
            case "speed":
                IncreaseSpeed(upgradeType.upgradeIncrease);
                break;
            default:
                Debug.LogWarning("Upgrade name " + upgradeType.upgradeName + " has not been implemented");
                return;
        }
    }
    
    private void IncreaseMaxHealth(float increase)
    {
        playerHealth.IncreaseMaxHealth(increase);
        playerHealth.IncreaseHealth(increase);
    }

    private void RegenHealth(float increase)
    {
        playerHealth.IncreaseHealth(increase);
    }

    private void IncreaseDamage(float increase)
    {
        currentStats.damage += increase;
    }

    private void DecreaseDashCooldown(float decrease)
    {
        currentStats.dashCooldown /= decrease;
    }

    private void IncreaseDashDuration(float increase)
    {
        currentStats.dashDuration += increase;
    }

    private void IncreaseSpeed(float increase)
    {
        currentStats.speed += increase;
    }
}
