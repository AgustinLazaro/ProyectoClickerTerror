using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private GameManagerMarian managerMarian;

    private void Awake()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();
        ActualizarUI();
    }

    public void AddPoints(int cantidad)
    {
        score += cantidad;
        ActualizarUI();
        managerMarian.UpdateScore(score);   
        Debug.Log($"Puntaje actual: {score}");
    }

    private void ActualizarUI()
    {
        scoreText.text = $"Score: {score}";
    }
}
