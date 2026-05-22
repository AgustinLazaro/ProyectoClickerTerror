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
    [SerializeField] private GameManagerMarian gameManager;

    private void Awake()
    {
        SetCanvasGroup(gameoverPanel, false);
        SetCanvasGroup(victoryPanel, false);
        AddButtonsListeners();
    }

    private void OnEnable()
    {
        // Suscribe los eventos del GameManager.
        if (gameManager != null)
        {
            gameManager.OnGameOver += ShowGameOverScreen;
            gameManager.OnVictory += ShowVictoryScreen;
        }
    }

    private void OnDisable()
    {
        // Desuscribe los eventos para evitar errores.
        gameManager.OnGameOver -= ShowGameOverScreen;
        gameManager.OnVictory -= ShowVictoryScreen;
    }

    private void OnDestroy()
    {
        RemoveButtonsListeners();
    }

    private void AddButtonsListeners()
    {
        // Asigna funciones a los botones de ambos paneles.
        gameOverPlayAgainButton.onClick.AddListener(OnPlayAgainClicked);
        gameOverMainMenuButton.onClick.AddListener(OnExitGameClicked);
        victoryPlayAgainButton.onClick.AddListener(OnPlayAgainClicked);
        victoryMainMenuButton.onClick.AddListener(OnExitGameClicked);
    }
    private void RemoveButtonsListeners()
    {
        // Elimina los listeners para prevenir referencias colgantes.
        gameOverPlayAgainButton.onClick.RemoveAllListeners();
        gameOverMainMenuButton.onClick.RemoveAllListeners();
        victoryPlayAgainButton.onClick.RemoveAllListeners();
        victoryMainMenuButton.onClick.RemoveAllListeners();
    }
    private void SetCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        // Activa o desactiva la visibilidad e interacción de un panel.
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }
    public void ShowGameOverScreen()
    {
        //muestra el panel de Game Over
        Cursor.lockState = CursorLockMode.None;
        gameoverPanel.alpha = 1;
        gameoverPanel.interactable = true;
        gameoverPanel.blocksRaycasts = true;
    }
    public void ShowVictoryScreen()
    {
        //muestra el panel de Victoria
        Cursor.lockState = CursorLockMode.None;
        victoryPanel.alpha = 1;
        victoryPanel.interactable = true;
        victoryPanel.blocksRaycasts = true;
    }
    private void OnPlayAgainClicked()
    {
        // Reinicia el juego cargando la escena principal.
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    private void OnExitGameClicked()
    {
        // Vuelve al menú principal.
        SceneManager.LoadScene(1);
    }
}
