using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DragObjectGame : MonoBehaviour, IApp
{
    [Header("Configuration")]
    [SerializeField] private float timeLimit = 15f;

    [Header("Objetos arrastrables")]    //new
    [SerializeField] private RectTransform objectPurple;
    [SerializeField] private RectTransform objectOrange;
    [SerializeField] private RectTransform objectGreen;

    [Header("Zonas destino")]   //new
    [SerializeField] private RectTransform zonePurple;
    [SerializeField] private RectTransform zoneOrange;
    [SerializeField] private RectTransform zoneGreen;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI orderText; //new
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Colores de orden")]
    [SerializeField] private Color colorA = Color.magenta;
    [SerializeField] private Color colorB = new Color(1f, 0.5f, 0f); // naranja
    [SerializeField] private Color colorC = Color.green;

    [SerializeField] private SFXEventChannelSO sfxChannel;

    private Vector2 _initialPositionA;    //new
    private Vector2 _initialPositionB;    //new
    private Vector2 _initialPositionC;    //new

    private float _timeLeft;
    private bool isGameActive = false;
    private int _currentOrder = 0;    // 0=A, 1=B, 2=C

    private AppController _appController;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();

        _initialPositionA = objectPurple.anchoredPosition;
        _initialPositionB = objectOrange.anchoredPosition;
        _initialPositionC = objectGreen.anchoredPosition;
    }

    public void OnAppOpen() => StartGame();
    public void OnAppClose() => StopGame();

    private void StartGame()
    {
        _timeLeft = timeLimit;
        isGameActive = true;
        _currentOrder = 0;  //new

        objectPurple.anchoredPosition = _initialPositionA;    //new
        objectOrange.anchoredPosition = _initialPositionB;    //new
        objectGreen.anchoredPosition = _initialPositionC;    //new

        resultText.gameObject.SetActive(false);
        UpdateTextOrder(); //new
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

    public void OnBeginDrag(RectTransform draggedObject)
    {
        if (!isGameActive) return;
        if (draggedObject != GetCurrentObject()) return;    // no es el turno de este objet

        sfxChannel.Raise(SoundID.Click);
    }

    public void OnDrag(RectTransform draggedObject, Vector2 delta)
    {
        if (!isGameActive) return;
        if (draggedObject != GetCurrentObject()) return;    // ignora objetos fuera de turno
    }

    public void OnEndDrag(RectTransform draggedObject)
    {
        if (!isGameActive) return;
        if(draggedObject != GetCurrentObject()) return;

        CheckZone(draggedObject);
    }

    // --- Lógica interna ---

    private RectTransform GetCurrentObject()
    {
        switch (_currentOrder)
        {
            case 0: return objectPurple;
            case 1: return objectOrange;
            case 2: return objectGreen;
            default: return null;
        }
    }

    private RectTransform GetCurrentZone()
    {
        switch (_currentOrder)
        {
            case 0: return zonePurple;
            case 1: return zoneOrange;
            case 2: return zoneGreen;
            default: return null;
        }
    }

    private void CheckZone(RectTransform draggedObject) 
    {
        RectTransform currentZone = GetCurrentZone();
        Camera cam = Camera.main;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, draggedObject.position);
        bool insideZone = RectTransformUtility.RectangleContainsScreenPoint(currentZone, screenPoint, cam);

        if (insideZone)
        {
            _currentOrder++;
            UpdateTextOrder();

            if (_currentOrder >= 3)
                GameOver(win: true);
        }
        else
        {
            if (_currentOrder == 0) draggedObject.anchoredPosition = _initialPositionA;
            if (_currentOrder == 1) draggedObject.anchoredPosition = _initialPositionB;
            if (_currentOrder == 2) draggedObject.anchoredPosition = _initialPositionC;
        }
    }

    private void GameOver(bool win)
    {
        isGameActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "You Win!" : "You Lose";

        if (win)
        {
            sfxChannel.Raise(SoundID.WinJingle);
            _scoreManager.AddPoints(20);
        }
        else
        {
            sfxChannel.Raise(SoundID.LoseJingle);
        }

        StartCoroutine(BackToHomeScreen());
    }

    private IEnumerator BackToHomeScreen()
    {
        yield return new WaitForSeconds(2f);
        _appController.CloseCurrentApp();
    }

    private void UpdateTextOrder()
    {
        Color[] colores = { colorA, colorB, colorC };
        string[] nombres = { "Morado", "Naranja", "Verde" };

        if (_currentOrder < 3)
        {
            orderText.text = $"Arrastrá: {nombres[_currentOrder]}";
            orderText.color = colores[_currentOrder];
        }
        else
        {
            orderText.text = "¡Completado!";
            orderText.color = Color.white;
        }
    }

    private void UpdateUI()
    {
        timeText.text = $"{Mathf.CeilToInt(_timeLeft)}s";
    }
}
