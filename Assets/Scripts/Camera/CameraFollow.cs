using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool followEnemies = false;

    private List<Transform> targetTransforms = new List<Transform>();
    private CinemachineVirtualCamera cam;
    
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        
        targetTransforms.Add(GameObject.FindWithTag("Player").transform);
    }
}
