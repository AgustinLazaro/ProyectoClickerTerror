using System;
using UnityEngine;

public class GameManagerMarian : MonoBehaviour
{
    public enum GameState { Playing, Paused, GameOver, Victory }

    [Header("Configuration")]
    [SerializeField] private int targetScore = 50;

    [SerializeField] private GameState currentState;
    public GameState CurrentGameState => currentState;

    //eventos de estado
    public event Action<GameState> OnStateChanged;
    public event Action OnGameOver;
    public event Action OnVictory;
    public event Action<int> OnScoreChanged;

    private int currentScore = 0;
    private bool isGameActive = false;

    private GameTimer gameTimer;

    private void Awake()
    {
        gameTimer = FindAnyObjectByType<GameTimer>();
    }

    private void Start()
    {
        SetGameState(GameState.Playing);
        isGameActive = true;
    }

    private void SetGameState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("Nuevo estado: " + currentState);
        OnStateChanged?.Invoke(currentState);

        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1;
                break;

            case GameState.Paused:
                Time.timeScale = 0;
                break;

            case GameState.GameOver:
                isGameActive = false;
                Time.timeScale = 0;
                gameTimer.StopTimer();
                OnGameOver?.Invoke();
                break;

            case GameState.Victory:
                isGameActive = false;
                Time.timeScale = 0;
                gameTimer.StopTimer();
                OnVictory?.Invoke();
                break;
        }

    }

    public void GameOver()
    {
        if (!isGameActive) return;
        SetGameState(GameState.GameOver);
    }

    public void Victory()
    {
        if (!isGameActive) return;
        SetGameState(GameState.Victory);
    }

    public void Pause() => SetGameState(GameState.Paused);
    public void Resume() => SetGameState(GameState.Playing);

    //llamado por ParanoiaManager cuando estamina llega a 0
    public void DepletedStamina()
    {
        if (!isGameActive) return;
        GameOver();
    }

    //llamado por enemigo
    public void EnemyCatchsPlayer()
    {
        if (!isGameActive) return;
        GameOver();
    }

    //llamado por GamerTimer
    public void EvaluateDepletedTime()
    {
        if (!isGameActive) return;

        if (currentScore >= targetScore)
            Victory();
        else
            GameOver();
    }

    //llamado por zona de escape
    public void TryToEscape()
    {
        if (!isGameActive) return;

        if (currentScore >= targetScore)
            Victory();
        else
            Debug.Log($"Puntaje insuficiente: {currentScore} / {targetScore}");
    }

    //llamado por ScoreManager
    public void UpdateScore(int newScore)
    {
        currentScore = newScore;
        OnScoreChanged?.Invoke(currentScore);
    }

    public int GetScore() => currentScore;
    public int GetTargetScore() => targetScore;
}
