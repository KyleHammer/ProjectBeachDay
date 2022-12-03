using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeTypeObject", menuName = "Upgrades/UpgradeTypeObject")]
public class UpgradeTypeObject : ScriptableObject
{
    public string upgradeName;
    public float upgradeIncrease;
    public Sprite upgradeSprite;
}
