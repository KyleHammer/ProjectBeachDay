using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NextRoomInteractable : MonoBehaviour
{
    private UpgradeTypeObject upgradeTypeObject;
    private String sceneName;
    private TextMeshProUGUI flavorText;
    
    // Start is called before the first frame update
    void Start()
    {
        upgradeTypeObject = GameManager.Instance.upgradePool[Random.Range(0, GameManager.Instance.upgradePool.Length)];
        sceneName = GameManager.Instance.scenePool[Random.Range(0, GameManager.Instance.scenePool.Length)];

        flavorText = GetComponentInChildren<TextMeshProUGUI>();

        if(upgradeTypeObject.upgradeName == "health")
            flavorText.text = upgradeTypeObject.flavorText + " " + upgradeTypeObject.upgradeIncrease + " HP";
        else
            flavorText.text = upgradeTypeObject.flavorText + " " + upgradeTypeObject.upgradeIncrease;
        
        GameManager.Instance.AddNextRoomInteractable(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.Instance.ClearRoomInteractables();
            GameManager.Instance.currentRoomUpgrade = upgradeTypeObject;
            SceneManager.LoadScene(sceneName);
        }
    }
}
