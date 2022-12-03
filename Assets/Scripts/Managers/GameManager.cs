using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private PlayerStatsObject startingStats;
    [SerializeField] private PlayerStatsObject currentStats;
    [Space]
    
    [SerializeField] private GameObject pickUpReward;
    public UpgradeTypeObject[] upgradePool;
    public UpgradeTypeObject currentRoomUpgrade;
    public string[] scenePool;
    private List<GameObject> nextRoomInteractables = new List<GameObject>();
    
    private int score = 0;
    [SerializeField] private int scoreIncrease = 100;
    
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource enemyDeathSFX;
    [SerializeField] private AudioSource playerDeathSFX;

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
            Destroy(this.gameObject);
        }
    }

    public GameObject GetPlayer() => player;

    public List<GameObject> GetEnemies() => enemies;

    public GameObject GetGameUI() => gameUI;

    public int GetScore() => score;

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
    
    // Set in each NextRoomInteractable Start()
    public void AddNextRoomInteractable(GameObject newExit)
    {
        nextRoomInteractables.Add(newExit);
        
        // Hide the exit
        newExit.SetActive(false);
    }

    public void ClearRoomInteractables()
    {
        nextRoomInteractables.Clear();
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
        // Give score reward
        score += scoreIncrease;
        gameUI.GetComponentInChildren<ScoreUI>().UpdateScore(score);
        
        // Give upgrade reward
        if (currentRoomUpgrade != null)
        {
            GameObject newPickUp = Instantiate(pickUpReward, Vector2.zero, Quaternion.identity);
            newPickUp.GetComponent<PickUpScript>().SetValues(currentRoomUpgrade);
        }
        
        // Show next rooms
        foreach (GameObject exit in nextRoomInteractables)
        {
            exit.SetActive(true);
        }
    }

    public void GameOver()
    {
        playerDeathSFX.Play();
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
            case "Death":
                enemyDeathSFX.Play();
                break;
            default:
                Debug.LogError("No sound effect for " + audioName);
                return;
        }
    }
}
