using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickClickGame : MonoBehaviour, IApp
{
    [Header("Configuration")]
    [SerializeField] private int targetClicks = 50;
    [SerializeField] private float timeLimit = 10f;

    [Header("UI")]
    [SerializeField] private Button clickButton;
    [SerializeField] private TextMeshProUGUI clicksText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI resultText;

    private int currentClicks = 0;
    private float timeLeft = 0f;
    private bool gameActive = false;

    private AppController appController;

    private void Awake()
    {
        appController = FindAnyObjectByType<AppController>();
        clickButton.onClick.AddListener(OnClick);
    }

    //IApp
    public void OnAppOpen()
    {
        StartGame();
    }

    public void OnAppClose()
    {
        StopGame();
    }

    private void StartGame()
    {
        currentClicks = 0;
        timeLeft = timeLimit;
        gameActive = true;

        resultText.gameObject.SetActive(false);
        UpdateUI();
        // resetear estado, iniciar timer, etc.
    }

    private void StopGame()
    {
        gameActive = false;
    }

    private void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        UpdateUI();

        if (timeLeft <= 0f)
            GameOver(win: false);
        // lógica del juego
    }

    private void OnClick()
    {
        if (!gameActive) return;

        currentClicks++;
        UpdateUI();

        if (currentClicks >= targetClicks)
            GameOver(win: true);
    }

    private void GameOver(bool win)
    {
        gameActive = false;

        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You Win!" : "You Lose";

        StartCoroutine(BackToHomeScreen());
    }

    private IEnumerator BackToHomeScreen()
    {
        yield return new WaitForSeconds(2f);  //muestra resultados brevemente
        appController.CloseCurrentApp();
    }

    private void UpdateUI()
    {
        clicksText.text = $"{currentClicks} / {targetClicks}";
        timeText.text = $"{Mathf.CeilToInt(timeLeft)}s";
    }

    private void OnDestroy()
    {
        clickButton.onClick.RemoveAllListeners();
    }
}
