using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject startingStats;
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [SerializeField] private GameObject pickUpReward;
    [SerializeField] private UpgradeTypeObject[] upgradePool;
    private UpgradeTypeObject currentRoomUpgrade;
    
    [SerializeField] private AudioSource music;
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
            
            ResetPlayerStats();
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject GetPlayer() => player;

    public List<GameObject> GetEnemies() => enemies;

    public GameObject GetGameUI() => gameUI;

    // Set from PlayerController Start()
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
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
        
        if(enemies.Count == 0)
            RoomCleared();
    }

    private IEnumerator SetEnemyTarget(GameObject newEnemy)
    {
        // Wait until the next frame
        // That way each enemy and player has been assigned to the game manager
        yield return 0;

        newEnemy.GetComponent<IEnemyDamagable>().SetTargetPlayer(player.transform);
    }

    private void RoomCleared()
    {
        if (currentRoomUpgrade != null)
        {
            GameObject newPickUp = Instantiate(pickUpReward, Vector2.zero, Quaternion.identity);
            newPickUp.GetComponent<PickUpScript>().SetValues(currentRoomUpgrade);
        }
        
        Debug.Log("Spawn in exits");
    }

    public void GameOver()
    {
        ResetPlayerStats();
        player.SetActive(false);
    }

    private void ResetPlayerStats()
    {
        currentStats.damage = startingStats.damage;
        currentStats.health = startingStats.health;
        currentStats.speed = startingStats.speed;
        currentStats.dashDuration = startingStats.dashDuration;
        currentStats.dashCooldown = startingStats.dashCooldown;
        currentStats.maxHealth = startingStats.maxHealth;
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
