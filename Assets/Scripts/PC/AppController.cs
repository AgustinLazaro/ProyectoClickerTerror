using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    [Header("Home")]
    [SerializeField] private CanvasGroup homeScreen;

    [Header("App Buttons")]
    [SerializeField] private Button app1Button;
    [SerializeField] private Button app2Button;
    [SerializeField] private Button app3Button;
    [SerializeField] private Button app4Button;

    [Header("Windows")]
    [SerializeField] private CanvasGroup app1Window;
    [SerializeField] private CanvasGroup app2Window;
    [SerializeField] private CanvasGroup app3Window;
    [SerializeField] private CanvasGroup app4Window;

    [Header("Close Buttons")]
    [SerializeField] private Button closeButton1;
    [SerializeField] private Button closeButton2;
    [SerializeField] private Button closeButton3;
    [SerializeField] private Button closeButton4;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.3f;

    private CanvasGroup currentApp = null;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        AddButtonsListeners();
        SetStateCanvasGroup(homeScreen, true);
        CloseAllWindows();
    }

    private void AddButtonsListeners()
    {
        app1Button.onClick.AddListener(() => OpenApp(app1Window));
        app2Button.onClick.AddListener(() => OpenApp(app2Window));
        app3Button.onClick.AddListener(() => OpenApp(app3Window));
        app4Button.onClick.AddListener(() => OpenApp(app4Window));

        closeButton1.onClick.AddListener(CloseCurrentApp);
        closeButton2.onClick.AddListener(CloseCurrentApp);
        closeButton3.onClick.AddListener(CloseCurrentApp);
        closeButton4.onClick.AddListener(CloseCurrentApp);
    }

    private void OpenApp(CanvasGroup app)
    {
        if (currentApp == app) return;

        CloseCurrentApp();

        currentApp = app;
        app.GetComponent<IApp>()?.OnAppOpen();
        audioManager.PlayOpen();

        StartCoroutine(FadeIn(app));
        StartCoroutine(FadeOut(homeScreen));

        //SetStateCanvasGroup(homeScreen, false);
        //SetStateCanvasGroup(app, true);
        //app.GetComponent<IApp>()?.OnAppOpen();
    }

    public void CloseCurrentApp()
    {
        if (currentApp == null) return;

        currentApp.GetComponent<IApp>()?.OnAppClose();
        audioManager.PlayClose();

        StartCoroutine(FadeOut(currentApp));
        StartCoroutine(FadeIn(homeScreen));

        currentApp = null;

        //SetStateCanvasGroup(currentApp, false);
        //SetStateCanvasGroup(homeScreen, true);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float tiempo = 0f;
        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(tiempo / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float tiempo = 0f;
        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - tiempo / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void CloseAllWindows()
    {
        SetStateCanvasGroup(app1Window, false);
        SetStateCanvasGroup(app2Window, false);
        SetStateCanvasGroup(app3Window, false);
        SetStateCanvasGroup(app4Window, false);
    }

    private void SetStateCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        // Activa o desactiva visibilidad e interacción de un panel
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
        app1Button.onClick.RemoveAllListeners();
        app2Button.onClick.RemoveAllListeners();
        app3Button.onClick.RemoveAllListeners();
        app4Button.onClick.RemoveAllListeners();

        closeButton1.onClick.RemoveAllListeners();
        closeButton2.onClick.RemoveAllListeners();
        closeButton3.onClick.RemoveAllListeners();
        closeButton4.onClick.RemoveAllListeners();
    }
}
