using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource crabDeathSFX;

    private GameObject player;
    private GameObject gameUI;
    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    
    public GameObject GetGameUI()
    {
        return gameUI;
    }
    
    // Set from PlayerController Start()
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        ;
    }

    // Set from HealthUI Start()
    public void SetGameUI(GameObject newGameUI)
    {
        gameUI = newGameUI;
    }
    
    // Set in each enemies Start()
    public void AddEnemy(GameObject newEnemy)
    {
        enemies.Add(newEnemy);
        StartCoroutine(SetEnemyTarget(newEnemy));
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    private IEnumerator SetEnemyTarget(GameObject newEnemy)
    {
        // Wait until the next frame
        // That way each enemy and player has been assigned to the game manager
        yield return 0;

        newEnemy.GetComponent<IEnemyDamagable>().SetTargetPlayer(player.transform);
    }

    public void PlayAudio(string audioName)
    {
        switch (audioName)
        {
            case "BasicHit":
                hitSFX.Play();
                break;
            case "CrabDeath":
                crabDeathSFX.Play();
                break;
            default:
                Debug.LogError("No sound effect for " + audioName);
                return;
        }
    }
}