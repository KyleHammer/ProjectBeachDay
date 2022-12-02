using UnityEngine;

public class WalkingUnit : MonoBehaviour
{
    private Transform playerTransfrom;

    [Header("Enemy Stats")]
    [SerializeField] private float moveSpeed = 3;

    // Start is called before the first frame update
    private void Start()
    {
        playerTransfrom = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    private void FixedUpdate()
    {
        float step =  moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, playerTransfrom.position, step);
    }
}
