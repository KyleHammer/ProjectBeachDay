using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyScalingObject", menuName = "Stats/EnemyScalingObject")]
public class EnemyStatsObject : ScriptableObject
{
    public float speedScaling = 2f;
    public float healthScaling = 2f;
    public float damageScaling = 2f;
}
