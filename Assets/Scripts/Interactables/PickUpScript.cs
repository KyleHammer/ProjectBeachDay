using System;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] private UpgradeTypeObject upgradeType;
    private SpriteRenderer sr;
    
    public void SetValues(UpgradeTypeObject newUpgrade)
    {
        upgradeType = newUpgrade;
        
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = upgradeType.upgradeSprite;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerUpgrades>().Upgrade(upgradeType);
            Destroy(this.gameObject);
        }
    }
}
