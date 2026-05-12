using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        DisplayScore(); // Updating the visual display whenever the score changes
    }
}
