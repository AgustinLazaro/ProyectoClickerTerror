using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIChangeScene : MonoBehaviour
{
    [Header("Menu Panel")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Credits Panel")]
    [SerializeField] private CanvasGroup creditsCanvasGroup;
    [SerializeField] private Button creditsBackButton;

    [Header("Options Panel")]
    [SerializeField] private CanvasGroup optionsCanvasGroup;
    [SerializeField] private Button optionsBackButton;

    public void Awake()
    {
        AddButtonsListeners();
        SetStateCanvasGroup(menuCanvasGroup, true);
        SetStateCanvasGroup(optionsCanvasGroup, false);
        SetStateCanvasGroup(creditsCanvasGroup, false);
    }

    private void AddButtonsListeners()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
        optionsBackButton.onClick.AddListener(OnOptionsBackClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        creditsBackButton.onClick.AddListener(OnCreditsBackClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnPlayClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    private void OnOptionsClicked()
    {
        SetStateCanvasGroup(menuCanvasGroup, false);
        SetStateCanvasGroup(optionsCanvasGroup, true);
    }

    private void OnOptionsBackClicked()
    {
        SetStateCanvasGroup(menuCanvasGroup, true);
        SetStateCanvasGroup(optionsCanvasGroup, false);
    }

    private void OnCreditsClicked()
    {
        SetStateCanvasGroup(menuCanvasGroup, false);
        SetStateCanvasGroup(creditsCanvasGroup, true);

    }

    private void OnCreditsBackClicked()
    {
        SetStateCanvasGroup(menuCanvasGroup, true);
        SetStateCanvasGroup(creditsCanvasGroup, false);

    }

    private void OnExitClicked()
    {
        
        Application.Quit();

       
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private void SetStateCanvasGroup(CanvasGroup canvasGroup, bool state)
    {
        
        canvasGroup.alpha = state ? 1 : 0;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
    }
}
