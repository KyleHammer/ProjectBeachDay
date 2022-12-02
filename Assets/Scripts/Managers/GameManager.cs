using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource crabDeathSFX;

    private GameObject player;
    private GameObject gameUI;

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
    
    // Set from PlayerController Awake()
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    // Set from HealthUI Awake()
    public void SetGameUI(GameObject newGameUI)
    {
        gameUI = newGameUI;
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
