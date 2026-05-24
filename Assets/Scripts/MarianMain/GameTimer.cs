using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float timeLimit = 300f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeLeft;
    private bool isTimerActive = false;

    private GameManagerMarian managerMarian;

    private void Awake()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();
    }

    private void Start()
    {
        timeLeft = timeLimit;
        isTimerActive = true;
        ActualizarUI();
    }

    private void Update()
    {
        if (!isTimerActive) return;

        timeLeft -= Time.deltaTime;
        ActualizarUI();

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isTimerActive = false;
            ActualizarUI();
            managerMarian.EvaluateDepletedTime();
        }
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    private void ActualizarUI()
    {
        int minutos = Mathf.FloorToInt(timeLeft / 60f);
        int segundos = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"{minutos:00}:{segundos:00}";
    }
}