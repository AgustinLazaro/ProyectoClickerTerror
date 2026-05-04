using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    [Header("PC")]
    [SerializeField] private CanvasGroup desktop;
    [SerializeField] private CanvasGroup appWindow;
    [SerializeField] private Button appTestButton;
    [SerializeField] private Button closeButton;

    public void Awake()
    {
        AddButtonsListeners();
        SetStateCanvasGroup(desktop, true);
        SetStateCanvasGroup(appWindow, false);
    }

    private void AddButtonsListeners()
    {
        appTestButton.onClick.AddListener(OpenApp);
        closeButton.onClick.AddListener(CloseApp);
    }

    public void OpenApp()
    {
        SetStateCanvasGroup(desktop, false);
        SetStateCanvasGroup(appWindow, true);
    }

    public void CloseApp()
    {
        SetStateCanvasGroup(appWindow, false);
        SetStateCanvasGroup(desktop, true);
    }
    private void SetStateCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        // Activa o desactiva visibilidad e interacción de un panel
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void OnDestroy()
    {
        RemoveButtonsListeners();
    }

    private void RemoveButtonsListeners()
    {
        appTestButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }
}
