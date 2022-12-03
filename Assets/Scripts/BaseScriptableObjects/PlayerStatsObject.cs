using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatsObject", menuName = "Stats/PlayerStatsObject")]
public class PlayerStatsObject : ScriptableObject
{
    public float maxHealth = 5f;
    public float health = 5f;
    public float damage = 1f;
    public float dashCooldown = 3f;
    public float dashDuration = 0.1f;
    public float speed = 5f;
}
