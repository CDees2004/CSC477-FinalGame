using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

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
