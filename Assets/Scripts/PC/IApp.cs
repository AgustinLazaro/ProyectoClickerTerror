using UnityEngine;

public interface IApp
{
    void OnAppOpen();
    void OnAppClose();
}

//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class AppController : MonoBehaviour
//{
//    [Header("Home")]
//    [SerializeField] private CanvasGroup homeScreen;

//    [Header("Botones de íconos")]
//    [SerializeField] private Button app1Button;
//    [SerializeField] private Button app2Button;
//    [SerializeField] private Button app3Button;
//    [SerializeField] private Button app4Button;

//    [Header("Ventanas")]
//    [SerializeField] private CanvasGroup app1Window;
//    [SerializeField] private CanvasGroup app2Window;
//    [SerializeField] private CanvasGroup app3Window;
//    [SerializeField] private CanvasGroup app4Window;

//    [Header("Botones de cerrar")]
//    [SerializeField] private Button closeButton1;
//    [SerializeField] private Button closeButton2;
//    [SerializeField] private Button closeButton3;
//    [SerializeField] private Button closeButton4;

//    [Header("Fade")]
//    [SerializeField] private float fadeDuration = 0.3f;

//    private CanvasGroup currentApp = null;

//    private void Awake()
//    {
//        AddButtonsListeners();
//        SetStateCanvasGroup(homeScreen, true);
//        CloseAllWindows();
//    }

//    private void AddButtonsListeners()
//    {
//        app1Button.onClick.AddListener(() => OpenApp(app1Window));
//        app2Button.onClick.AddListener(() => OpenApp(app2Window));
//        app3Button.onClick.AddListener(() => OpenApp(app3Window));
//        app4Button.onClick.AddListener(() => OpenApp(app4Window));

//        closeButton1.onClick.AddListener(CloseCurrentApp);
//        closeButton2.onClick.AddListener(CloseCurrentApp);
//        closeButton3.onClick.AddListener(CloseCurrentApp);
//        closeButton4.onClick.AddListener(CloseCurrentApp);
//    }

//    private void OpenApp(CanvasGroup app)
//    {
//        if (currentApp == app) return;

//        CloseCurrentApp();

//        currentApp = app;
//        StartCoroutine(FadeIn(app));
//        StartCoroutine(FadeOut(homeScreen));

//        app.GetComponent<IApp>()?.OnAppOpen();
//    }

//    public void CloseCurrentApp()
//    {
//        if (currentApp == null) return;

//        currentApp.GetComponent<IApp>()?.OnAppClose();

//        StartCoroutine(FadeOut(currentApp));
//        StartCoroutine(FadeIn(homeScreen));

//        currentApp = null;
//    }

//    private IEnumerator FadeIn(CanvasGroup cg)
//    {
//        cg.interactable = true;
//        cg.blocksRaycasts = true;

//        float tiempo = 0f;
//        while (tiempo < fadeDuration)
//        {
//            tiempo += Time.deltaTime;
//            cg.alpha = Mathf.Clamp01(tiempo / fadeDuration);
//            yield return null;
//        }
//        cg.alpha = 1f;
//    }

//    private IEnumerator FadeOut(CanvasGroup cg)
//    {
//        float tiempo = 0f;
//        while (tiempo < fadeDuration)
//        {
//            tiempo += Time.deltaTime;
//            cg.alpha = Mathf.Clamp01(1f - tiempo / fadeDuration);
//            yield return null;
//        }

//        cg.alpha = 0f;
//        cg.interactable = false;
//        cg.blocksRaycasts = false;
//    }

//    private void SetStateCanvasGroup(CanvasGroup cg, bool state)
//    {
//        cg.alpha = state ? 1f : 0f;
//        cg.interactable = state;
//        cg.blocksRaycasts = state;
//    }

//    private void CloseAllWindows()
//    {
//        SetStateCanvasGroup(app1Window, false);
//        SetStateCanvasGroup(app2Window, false);
//        SetStateCanvasGroup(app3Window, false);
//        SetStateCanvasGroup(app4Window, false);
//    }

//    private void OnDestroy()
//    {
//        app1Button.onClick.RemoveAllListeners();
//        app2Button.onClick.RemoveAllListeners();
//        app3Button.onClick.RemoveAllListeners();
//        app4Button.onClick.RemoveAllListeners();

//        closeButton1.onClick.RemoveAllListeners();
//        closeButton2.onClick.RemoveAllListeners();
//        closeButton3.onClick.RemoveAllListeners();
//        closeButton4.onClick.RemoveAllListeners();
//    }
//}






//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class AppController : MonoBehaviour
//{
//    [Header("Home")]
//    [SerializeField] private CanvasGroup homeScreen;

//    [Header("App Buttons")]
//    [SerializeField] private Button app1Button;
//    [SerializeField] private Button app2Button;
//    [SerializeField] private Button app3Button;
//    [SerializeField] private Button app4Button;

//    [Header("Windows")]
//    [SerializeField] private CanvasGroup app1Window;
//    [SerializeField] private CanvasGroup app2Window;
//    [SerializeField] private CanvasGroup app3Window;
//    [SerializeField] private CanvasGroup app4Window;

//    [Header("Close Buttons")]
//    [SerializeField] private Button closeButton1;
//    [SerializeField] private Button closeButton2;
//    [SerializeField] private Button closeButton3;
//    [SerializeField] private Button closeButton4;

//    [Header("Fade")]
//    [SerializeField] private float fadeDuration = 0.3f;  // único campo nuevo

//    private CanvasGroup currentApp = null;

//    private void Awake()
//    {
//        AddButtonsListeners();
//        SetStateCanvasGroup(homeScreen, true);
//        CloseAllWindows();
//    }

//    private void AddButtonsListeners()
//    {
//        app1Button.onClick.AddListener(() => OpenApp(app1Window));
//        app2Button.onClick.AddListener(() => OpenApp(app2Window));
//        app3Button.onClick.AddListener(() => OpenApp(app3Window));
//        app4Button.onClick.AddListener(() => OpenApp(app4Window));

//        closeButton1.onClick.AddListener(CloseCurrentApp);
//        closeButton2.onClick.AddListener(CloseCurrentApp);
//        closeButton3.onClick.AddListener(CloseCurrentApp);
//        closeButton4.onClick.AddListener(CloseCurrentApp);
//    }

//    private void OpenApp(CanvasGroup app)
//    {
//        if (currentApp == app) return;

//        CloseCurrentApp();

//        currentApp = app;
//        app.GetComponent<IApp>()?.OnAppOpen();
//        AudioManager.Instance.PlayOpen();

//        StartCoroutine(FadeOut(homeScreen));
//        StartCoroutine(FadeIn(app));
//    }

//    public void CloseCurrentApp()
//    {
//        if (currentApp == null) return;

//        currentApp.GetComponent<IApp>()?.OnAppClose();
//        AudioManager.Instance.PlayClose();

//        StartCoroutine(FadeOut(currentApp));
//        StartCoroutine(FadeIn(homeScreen));

//        currentApp = null;
//    }

//    private IEnumerator FadeIn(CanvasGroup cg)
//    {
//        cg.interactable = true;
//        cg.blocksRaycasts = true;

//        float tiempo = 0f;
//        while (tiempo < fadeDuration)
//        {
//            tiempo += Time.deltaTime;
//            cg.alpha = Mathf.Clamp01(tiempo / fadeDuration);
//            yield return null;
//        }
//        cg.alpha = 1f;
//    }

//    private IEnumerator FadeOut(CanvasGroup cg)
//    {
//        float tiempo = 0f;
//        while (tiempo < fadeDuration)
//        {
//            tiempo += Time.deltaTime;
//            cg.alpha = Mathf.Clamp01(1f - tiempo / fadeDuration);
//            yield return null;
//        }

//        cg.alpha = 0f;
//        cg.interactable = false;
//        cg.blocksRaycasts = false;
//    }

//    private void CloseAllWindows()
//    {
//        SetStateCanvasGroup(app1Window, false);
//        SetStateCanvasGroup(app2Window, false);
//        SetStateCanvasGroup(app3Window, false);
//        SetStateCanvasGroup(app4Window, false);
//    }

//    private void SetStateCanvasGroup(CanvasGroup cg, bool state)
//    {
//        cg.alpha = state ? 1f : 0f;
//        cg.interactable = state;
//        cg.blocksRaycasts = state;
//    }

//    private void OnDestroy()
//    {
//        RemoveButtonsListeners();
//    }

//    private void RemoveButtonsListeners()
//    {
//        app1Button.onClick.RemoveAllListeners();
//        app2Button.onClick.RemoveAllListeners();
//        app3Button.onClick.RemoveAllListeners();
//        app4Button.onClick.RemoveAllListeners();

//        closeButton1.onClick.RemoveAllListeners();
//        closeButton2.onClick.RemoveAllListeners();
//        closeButton3.onClick.RemoveAllListeners();
//        closeButton4.onClick.RemoveAllListeners();
//    }
//}