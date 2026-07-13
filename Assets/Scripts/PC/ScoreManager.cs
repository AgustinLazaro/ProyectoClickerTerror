using TMPro;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private GameManagerMarian gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManagerMarian>();
        ActualizarUI();
    }

    public void AddPoints(int cantidad)
    {
        score += cantidad;
        ActualizarUI();
        gameManager.UpdateScore(score);
    }

    public void CheatMaxScore()
    {
        score = 9999;
        ActualizarUI();
        gameManager.UpdateScore(score);
    }

    private void ActualizarUI()
    {
        scoreText.text = $"Score: {score}";
    }
}
