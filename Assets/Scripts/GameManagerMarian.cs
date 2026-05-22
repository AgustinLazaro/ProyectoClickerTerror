using System;
using UnityEngine;

public class GameManagerMarian : MonoBehaviour
{
    public enum GameState { Playing, Paused, GameOver, Victory }
    [SerializeField] private GameState currentState;
    public GameState CurrentGameState => currentState;

    public event Action<GameState> OnStateChanged;
    public event Action OnGameOver;
    public event Action OnVictory;

    private void Start()
    {
        SetGameState(GameState.Playing);
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
                Time.timeScale = 0;
                OnGameOver?.Invoke();
                break;

            case GameState.Victory:
                Time.timeScale = 0;
                OnVictory?.Invoke();
                break;
        }

    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void Victory()
    {
        SetGameState(GameState.Victory);
    }

    public void Pause()
    {
        SetGameState(GameState.Paused);
    }

    public void Resume()
    {
        SetGameState(GameState.Playing);
    }
}
