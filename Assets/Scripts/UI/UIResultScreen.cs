using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIResultScreen : MonoBehaviour
{
    [Header("Game Over panel")]
    [SerializeField] private CanvasGroup gameoverPanel;
    [SerializeField] private Button gameOverPlayAgainButton;
    [SerializeField] private Button gameOverMainMenuButton;

    [Header("Victory panel")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private Button victoryPlayAgainButton;
    [SerializeField] private Button victoryMainMenuButton;

    [Header("Managers")]
    [SerializeField] private GameManagerMarian managerMarian;

    private void Awake()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();
        SetCanvasGroup(gameoverPanel, false);
        SetCanvasGroup(victoryPanel, false);
        AddButtonsListeners();
    }

    private void OnEnable()
    {
       
        if (managerMarian != null)
        {
            managerMarian.OnGameOver += ShowGameOverScreen;
            managerMarian.OnVictory += ShowVictoryScreen;
        }
    }

    private void OnDisable()
    {
        
        managerMarian.OnGameOver -= ShowGameOverScreen;
        managerMarian.OnVictory -= ShowVictoryScreen;
    }

    private void OnDestroy()
    {
        RemoveButtonsListeners();
    }

    private void AddButtonsListeners()
    {
       
        gameOverPlayAgainButton.onClick.AddListener(OnPlayAgainClicked);
        gameOverMainMenuButton.onClick.AddListener(OnExitGameClicked);
        victoryPlayAgainButton.onClick.AddListener(OnPlayAgainClicked);
        victoryMainMenuButton.onClick.AddListener(OnExitGameClicked);
    }
    private void RemoveButtonsListeners()
    {
      
        gameOverPlayAgainButton.onClick.RemoveAllListeners();
        gameOverMainMenuButton.onClick.RemoveAllListeners();
        victoryPlayAgainButton.onClick.RemoveAllListeners();
        victoryMainMenuButton.onClick.RemoveAllListeners();
    }
    private void SetCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
       
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
    public void ShowGameOverScreen()
    {
        
        Cursor.lockState = CursorLockMode.None;
        gameoverPanel.alpha = 1;
        gameoverPanel.interactable = true;
        gameoverPanel.blocksRaycasts = true;
    }
    public void ShowVictoryScreen()
    {
      
        Cursor.lockState = CursorLockMode.None;
        victoryPanel.alpha = 1;
        victoryPanel.interactable = true;
        victoryPanel.blocksRaycasts = true;
    }
    private void OnPlayAgainClicked()
    {
       
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    private void OnExitGameClicked()
    {
        
        SceneManager.LoadScene(0);
    }
}
