using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("All Stats")]
    [SerializeField] private PlayerStatsObject startingStats;
    [SerializeField] private PlayerStatsObject currentStats;
    [SerializeField] private EnemyStatsObject defaultEnemyStats;
    [SerializeField] private EnemyStatsObject currentEnemyStats;
    [SerializeField] private EnemyStatsObject enemyScalingFactor;
    [Space]
    
    [SerializeField] private GameObject pickUpReward;
    public UpgradeTypeObject[] upgradePool;
    public UpgradeTypeObject currentRoomUpgrade;
    
    public string[] scenePool;
    private List<GameObject> nextRoomInteractables = new List<GameObject>();

    private int score = 0;
    [SerializeField] private int scoreIncrease = 100;

    private GameObject gameOverScreen;
    
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
            
            ResetAllStats();
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

        gameOverScreen = gameUI.GetComponentInChildren<GameOverUI>().gameObject;
        gameOverScreen.SetActive(false);
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

    public void IncreaseDifficulty()
    {
        currentEnemyStats.damageScaling += enemyScalingFactor.damageScaling;
        currentEnemyStats.healthScaling += enemyScalingFactor.healthScaling;
        currentEnemyStats.speedScaling += enemyScalingFactor.speedScaling;
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
        ResetAllStats();
        DisablePlayer();
        enemies.Clear();
        
        gameOverScreen.SetActive(true);
    }

    private void DisablePlayer()
    {
        //player.SetActive(false);
        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<SpriteRenderer>().enabled = false;
        
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.DisableMovement();
        controller.DisableGun();
    }

    private void ResetAllStats()
    {
        currentStats.damage = startingStats.damage;
        currentStats.health = startingStats.health;
        currentStats.speed = startingStats.speed;
        currentStats.dashDuration = startingStats.dashDuration;
        currentStats.dashCooldown = startingStats.dashCooldown;
        currentStats.maxHealth = startingStats.maxHealth;

        currentEnemyStats.damageScaling = defaultEnemyStats.damageScaling;
        currentEnemyStats.healthScaling = defaultEnemyStats.healthScaling;
        currentEnemyStats.speedScaling = defaultEnemyStats.speedScaling;
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

    public void Restart()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene("StartScene");
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
