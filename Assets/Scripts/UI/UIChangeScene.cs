using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIChangeScene : MonoBehaviour
{
    [Header("Menu Panel")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Credits Panel")]
    [SerializeField] private GameObject panelCredits;
    [SerializeField] private Button creditsBackButton;

    public void Awake()
    {
        AddButtonsListeners();
    }

    private void AddButtonsListeners()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        creditsBackButton.onClick.AddListener(OnCreditsBackClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnPlayClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    private void OnCreditsClicked()
    {
        panelCredits.SetActive(true);
    }

    private void OnExitClicked()
    {
        // Cierra el juego en una build real (Windows, Android, etc.)
        Application.Quit();

        // Si estás en el Editor, detiene el modo Play
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnCreditsBackClicked()
    {
        panelCredits.SetActive(false);
    }

    public void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
    }
}
