using UnityEngine;

public class WalkingUnit : IEnemyDamagable
{
    private Transform playerTransfrom;

    [Header("Enemy Stats")]
    [SerializeField] private float moveSpeed = 3;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerTransfrom = GameManager.Instance.GetPlayer().transform;
    }
    
    private void FixedUpdate()
    {
        float step =  moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, playerTransfrom.position, step);
    }
}
