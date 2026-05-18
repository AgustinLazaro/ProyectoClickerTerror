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

    private int _currentClicks = 0;
    private float _timeLeft = 0f;
    private bool _isGameActive = false;

    private AppController _appController;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
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
        _currentClicks = 0;
        _timeLeft = timeLimit;
        _isGameActive = true;

        resultText.gameObject.SetActive(false);
        UpdateUI();
        // resetear estado, iniciar timer, etc.
    }

    private void StopGame()
    {
        _isGameActive = false;
    }

    private void Update()
    {
        if (!_isGameActive) return;

        _timeLeft -= Time.deltaTime;
        UpdateUI();

        if (_timeLeft <= 0f)
            GameOver(win: false);
        // lógica del game
    }

    private void OnClick()
    {
        if (!_isGameActive) return;

        _currentClicks++;
        UpdateUI();

        if (_currentClicks >= targetClicks)
            GameOver(win: true);
    }

    private void GameOver(bool win)
    {
        _isGameActive = false;

        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You Win!" : "You Lose";

        StartCoroutine(BackToHomeScreen());
    }

    private IEnumerator BackToHomeScreen()
    {
        yield return new WaitForSeconds(2f);  //muestra resultados brevemente
        _appController.CloseCurrentApp();
    }

    private void UpdateUI()
    {
        clicksText.text = $"{_currentClicks} / {targetClicks}";
        timeText.text = $"{Mathf.CeilToInt(_timeLeft)}s";
    }

    private void OnDestroy()
    {
        clickButton.onClick.RemoveAllListeners();
    }
}
