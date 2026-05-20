using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int puntaje = 0;

    //private static ScoreManager instance;
    //public static ScoreManager Instance => instance;

    private void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //    Destroy(gameObject);

        ActualizarUI();
    }

    public void AddPoints(int cantidad)
    {
        puntaje += cantidad;
        ActualizarUI();
        Debug.Log($"Puntaje actual: {puntaje}");
    }

    private void ActualizarUI()
    {
        scoreText.text = $"Score: {puntaje}";
    }
}
