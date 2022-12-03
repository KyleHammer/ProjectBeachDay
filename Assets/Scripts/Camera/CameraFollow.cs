using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Play mode restart required")]
    [SerializeField] private bool followEnemies = false;
    
    private CinemachineVirtualCamera cam;
    
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return 0;
        
        cam.Follow = GameManager.Instance.GetPlayer().transform;

        if (followEnemies)
        {
            CinemachineTargetGroup tg = gameObject.AddComponent<CinemachineTargetGroup>();
            tg.AddMember(GameManager.Instance.GetPlayer().transform, 1, 3);
            
            foreach(GameObject enemy in GameManager.Instance.GetEnemies())
            {
                tg.AddMember(enemy.transform, 0.2f, 1);
            }

            cam.Follow = tg.transform;
        }
    }
}
