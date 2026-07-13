using System.Collections;
using TMPro;
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

    [Header("Close buttons")]
    [SerializeField] private float cooldownApp1 = 10f;
    [SerializeField] private float cooldownApp2 = 10f;
    [SerializeField] private float cooldownApp3 = 10f;
    [SerializeField] private float cooldownApp4 = 10f;

    [Header("Cooldown Texts")]
    [SerializeField] private TextMeshProUGUI cooldownText1;
    [SerializeField] private TextMeshProUGUI cooldownText2;
    [SerializeField] private TextMeshProUGUI cooldownText3;
    [SerializeField] private TextMeshProUGUI cooldownText4;


    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.3f;

    [SerializeField] private PCAudioPlayer pcAudio;

    private CanvasGroup currentApp = null;

    private void Awake()
    {
        AddButtonsListeners();
        SetStateCanvasGroup(homeScreen, true);
        CloseAllWindows();
        HideAllCooldownTexts();
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
        pcAudio.PlaySound(SoundID.Open);

        StartCoroutine(FadeIn(app));
        StartCoroutine(FadeOut(homeScreen));
    }

    public void CloseCurrentApp()
    {
        if (currentApp == null) return;

        currentApp.GetComponent<IApp>()?.OnAppClose();
        pcAudio.PlaySound(SoundID.Close);

        StartCooldown(currentApp);

        StartCoroutine(FadeOut(currentApp));
        StartCoroutine(FadeIn(homeScreen));

        currentApp = null;
    }

    private void StartCooldown(CanvasGroup app)
    {
        if (app == app1Window)
            StartCoroutine(Cooldown(app1Button, cooldownApp1, cooldownText1));
        else if (app == app2Window)
            StartCoroutine(Cooldown(app2Button, cooldownApp2, cooldownText2));
        else if (app == app3Window)
            StartCoroutine(Cooldown(app3Button, cooldownApp3, cooldownText3));
        else if (app == app4Window)
            StartCoroutine(Cooldown(app4Button, cooldownApp4, cooldownText4));
    }

    private IEnumerator Cooldown(Button button, float duration, TextMeshProUGUI text)
    {
        button.interactable = false;
        text.gameObject.SetActive(true);

        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            text.text = $"{Mathf.CeilToInt(timeLeft)}";
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        button.interactable = true;
        text.gameObject.SetActive(false);
    }

    private void HideAllCooldownTexts()
    {
        cooldownText1.gameObject.SetActive(false);
        cooldownText2.gameObject.SetActive(false);
        cooldownText3.gameObject.SetActive(false);
        cooldownText4.gameObject.SetActive(false);
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

    public void CheatDeactivateCooldowns()
    {
        app1Button.interactable = true;
        app2Button.interactable = true;
        app3Button.interactable = true;
        app4Button.interactable = true;

        cooldownText1.gameObject.SetActive(false);
        cooldownText2.gameObject.SetActive(false);
        cooldownText3.gameObject.SetActive(false);
        cooldownText4.gameObject.SetActive(false);
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
