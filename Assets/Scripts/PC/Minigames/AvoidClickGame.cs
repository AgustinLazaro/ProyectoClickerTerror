using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using Random = UnityEngine.Random;

public class AvoidClickGame : MonoBehaviour, IApp
{
    [Header("Configuration")]
    [SerializeField] private float timeLimit = 15f;
    [SerializeField] private float intervalAppearance = 1.5f;
    [SerializeField] private int totalRights = 3;

    [Header("Objects")]
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private Button[] wrongButtons;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI rightsText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private SFXEventChannelSO sfxChannel;

    private float _timeLeft;
    private float _timeNextAppearance;
    private int _rightsClicked = 0;
    private bool isGameActive = false;

    private AppController _appController;
    private ScoreManager _scoreManager;
    //private PCAudioManager _audioManager;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        //_audioManager = FindAnyObjectByType<PCAudioManager>();
        RegisterButtons();
    }

    private void RegisterButtons()
    {
        Debug.Log($"Correctos: {rightButtons.Length} | Incorrectos: {wrongButtons.Length}");

        for (int i = 0; i < rightButtons.Length; i++)
        {
            int index = i;
            rightButtons[index].onClick.AddListener(() => OnRightClick(rightButtons[index]));
        }

        for (int i = 0; i < wrongButtons.Length; i++)
            wrongButtons[i].onClick.AddListener(OnWrongClick);
    }

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
        _timeLeft = timeLimit;
        _timeNextAppearance = intervalAppearance;
        _rightsClicked = 0;
        isGameActive = true;

        HideAll();
        resultText.gameObject.SetActive(false);
        UpdateUI();
    }

    private void StopGame()
    {
        isGameActive = false;
        HideAll();
    }

    private void Update()
    {
        if (!isGameActive) return;
        _timeLeft -= Time.deltaTime;
        _timeNextAppearance -= Time.deltaTime;
        UpdateUI();

        if(_timeNextAppearance <= 0f)
        {
            DisplayRandom();
            _timeNextAppearance = intervalAppearance;
        }

        if (_timeLeft <= 0f)
            GameOver(win: false);
    }

    private void DisplayRandom()
    {
        HideAll();

        
        bool showCorrect = Random.value > 0.4f;
        Debug.Log($"Mostrando: {(showCorrect ? "correcto" : "incorrecto")}");


        if (showCorrect)
        {
            var button = rightButtons[Random.Range(0, rightButtons.Length)];
            button.gameObject.SetActive(true);
        }
        else
        {
            var button = wrongButtons[Random.Range(0, wrongButtons.Length)];
            button.gameObject.SetActive(true);
        }
    }

    private void HideAll()
    {
        foreach (var button in rightButtons)
            button.gameObject.SetActive(false);

        foreach (var button in wrongButtons)
            button.gameObject.SetActive(false);
    }

    private void OnRightClick(Button button)
    {
        Debug.Log($"Click correcto en: {button.name}");
        if (!isGameActive) return;

        //_audioManager.PlayClick();
        sfxChannel.Raise(SoundID.Click);
        button.gameObject.SetActive(false);
        _rightsClicked++;
        UpdateUI();

        if (_rightsClicked >= totalRights)
            GameOver(win: true);
    }

    private void OnWrongClick()
    {
        Debug.Log("Click incorrecto");
        if (!isGameActive) return;

        //_audioManager.PlayClick();
        sfxChannel.Raise(SoundID.Click);
        GameOver(win: false);
    }

    private void GameOver(bool win)
    {
        isGameActive = false;
        HideAll();
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You win!" : "You lose";

        if (win)
        {
            //_audioManager.PlayWinJingle();
            sfxChannel.Raise(SoundID.WinJingle);
            _scoreManager.AddPoints(30);
        }
        else
        {
            //_audioManager.PlayLoseJingle();
            sfxChannel.Raise(SoundID.LoseJingle);
        }

        StartCoroutine(BackToHomeScreen());
    }

    private IEnumerator BackToHomeScreen()
    {
        yield return new WaitForSeconds(2f);
        _appController.CloseCurrentApp();
    }

    private void UpdateUI()
    {
        timeText.text = $"{Mathf.CeilToInt(_timeLeft)}s";
        rightsText.text = $"{_rightsClicked} / {totalRights}";
    }

    private void OnDestroy()
    {
        foreach (var button in rightButtons)
            button.onClick.RemoveAllListeners();

        foreach (var button in wrongButtons)
            button.onClick.RemoveAllListeners();
    }
}
