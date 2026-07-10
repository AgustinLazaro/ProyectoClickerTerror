using System.Collections;
using TMPro;
using UnityEngine;

public class DragObjectGame : MonoBehaviour, IApp
{
    [Header("Configuration")]
    [SerializeField] private float timeLimit = 15f;

    [Header("References")]
    [SerializeField] private RectTransform dragableObject;
    [SerializeField] private RectTransform destinationArea;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI resultText;

    private Vector2 _initialPosition;
    private float _timeLeft;
    private bool isGameActive = false;
    //private Canvas canvas;

    private AppController _appController;
    private ScoreManager _scoreManager;
    private PCAudioManager _audioManager;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _audioManager = FindAnyObjectByType<PCAudioManager>();
        //canvas = GetComponentInParent<Canvas>();
        _initialPosition = dragableObject.anchoredPosition;
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
        isGameActive = true;
        dragableObject.anchoredPosition = _initialPosition;
        resultText.gameObject.SetActive(false);
        UpdateUI();
    }

    private void StopGame()
    {
        isGameActive = false;
    }

    private void Update()
    {
        if (!isGameActive) return;

        _timeLeft -= Time.deltaTime;
        UpdateUI();

        if (_timeLeft <= 0f)
            GameOver(win: false);
    }

    public void OnBeginDrag()
    {
        if (!isGameActive) return;
        _audioManager.PlayClick();
    }

    public void OnDrag(Vector2 delta)
    {
        if (!isGameActive) return;
        dragableObject.anchoredPosition += delta;
        //dragableObject.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag()
    {
        if (!isGameActive) return;

        if (RectOverlaps(dragableObject, destinationArea))
            GameOver(win: true);
        else
            dragableObject.anchoredPosition = _initialPosition; //vuelve al inicio si no llego
    }

    private bool RectOverlaps(RectTransform dragableObject, RectTransform destinationArea)
    {
        return RectTransformUtility.RectangleContainsScreenPoint
            (destinationArea, RectTransformUtility.WorldToScreenPoint(null, dragableObject.position));
    }

    private void GameOver(bool win)
    {
        isGameActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You Win!" : "You Lose";

        if (win)
        {
            _audioManager.PlayWinJingle();
            _scoreManager.AddPoints(20);
        }
        else
        {
            _audioManager.PlayLoseJingle();
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
    }
}
