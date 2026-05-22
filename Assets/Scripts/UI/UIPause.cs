using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManagerMarian gameManager;


    [Header("Panels")]
    [SerializeField] private CanvasGroup panelMainPause;
    [SerializeField] private CanvasGroup panelSettings;

    [Header("Buttons")]
    [SerializeField] private Button btnContinue;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnSettingsBack;

    private bool isPause;
    private void Awake()
    {
        AddButtonsListeners();
        SetStateCanvasGroup(panelMainPause, false);
        SetStateCanvasGroup(panelSettings, false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    private void AddButtonsListeners()
    {
        // Asigna las funciones a los botones del menú.
        btnContinue.onClick.AddListener(OnPlayClicked);
        btnSettings.onClick.AddListener(OnOptionsClicked);
        btnExit.onClick.AddListener(OnExitClicked);
        btnSettingsBack.onClick.AddListener(OnSettingsBackClicked);
    }

    private void TogglePause()
    {
        // Cambia el estado de pausa y muestra/oculta el panel principal.
        isPause = !isPause;
        SetStateCanvasGroup(panelMainPause, isPause);
        if (isPause)
        {
            gameManager.Pause();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            gameManager.Resume();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnPlayClicked()
    {
        // Reanuda el juego.
        TogglePause();
    }

    private void OnOptionsClicked()
    {
        SetStateCanvasGroup(panelSettings, true);
    }
    private void OnExitClicked()
    {
        ExitGame();
    }

    private void ExitGame()
    {
        // Carga la escena del menú principal.
        SceneManager.LoadScene(1);
    }

    private void OnSettingsBackClicked()
    {
        SetStateCanvasGroup(panelSettings, false);
    }

    private void SetStateCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        // Activa o desactiva visibilidad e interacción de un panel.
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    private void OnDestroy()
    {
        RemoveButtonsListeners();
    }

    private void RemoveButtonsListeners()
    {
        btnContinue.onClick.RemoveAllListeners();
        btnSettings.onClick.RemoveAllListeners();
        btnExit.onClick.RemoveAllListeners();
        btnSettingsBack.onClick.RemoveAllListeners();
    }
}
