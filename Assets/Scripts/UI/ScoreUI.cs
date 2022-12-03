using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateScore(GameManager.Instance.GetScore());
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
}
