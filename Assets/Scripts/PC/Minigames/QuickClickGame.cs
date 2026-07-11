using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickClickGame : MonoBehaviour, IApp
{
    [Header("Configuration")]
    [SerializeField] private int targetClicks = 50;
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private float speedMovement = 150f; //new

    [Header("UI")]
    [SerializeField] private Button clickButton;
    [SerializeField] private RectTransform clickButtonRect;     // nuevo
    [SerializeField] private RectTransform gameArea;
    [SerializeField] private TextMeshProUGUI clicksText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private SFXEventChannelSO sfxChannel;

    private int _currentClicks = 0;
    private float _timeLeft = 0f;
    private bool _isGameActive = false;
    private Vector2 _direction;

    private AppController _appController;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        clickButton.onClick.AddListener(OnClick);
    }

    //IApp
    public void OnAppOpen() => StartGame();
    public void OnAppClose() => StopGame();

    private void StartGame()
    {
        _currentClicks = 0;
        _timeLeft = timeLimit;
        _isGameActive = true;
        _direction = Random.insideUnitCircle.normalized;    //new

        resultText.gameObject.SetActive(false);
        clickButtonRect.anchoredPosition = Vector2.zero;    //new
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

        MoveButton();

        if (_timeLeft <= 0f)
            GameOver(win: false);
        // lógica del game
    }

    private void MoveButton() //new mothod
    {
        clickButtonRect.anchoredPosition += _direction * speedMovement * Time.deltaTime;

        //calcula limites del area del juego
        Vector2 halfArea = gameArea.rect.size / 2f;
        Vector2 halfButton = clickButtonRect.rect.size / 2f;

        Vector2 pos = clickButtonRect.anchoredPosition;

        //rebota en los bordes
        if (pos.x > halfArea.x - halfButton.x || pos.x < -halfArea.x + halfButton.x)
        {
            _direction.x = -_direction.x;
            pos.x = Mathf.Clamp(pos.x, -halfArea.x + halfButton.x, halfArea.x - halfButton.x);
        }

        if (pos.y > halfArea.y - halfButton.y || pos.y < -halfArea.y + halfButton.y)
        {
            _direction.y = -_direction.y;
            pos.y = Mathf.Clamp(pos.y, -halfArea.y + halfButton.y, halfArea.y - halfButton.y);
        }

        clickButtonRect.anchoredPosition = pos;
    }

    private void OnClick()
    {
        if (!_isGameActive) return;

        sfxChannel.Raise(SoundID.Click);
        _currentClicks++;
        UpdateUI();

        //cambiar direccion aleatoriamente al clickear
        _direction = Random.insideUnitCircle.normalized;

        if (_currentClicks >= targetClicks)
            GameOver(win: true);
    }

    private void GameOver(bool win)
    {
        _isGameActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You Win!" : "You Lose";

        if (win)
        {
            sfxChannel.Raise(SoundID.WinJingle);
            _scoreManager.AddPoints(10);
        }
        else
        {
            sfxChannel.Raise(SoundID.LoseJingle);
        }

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
