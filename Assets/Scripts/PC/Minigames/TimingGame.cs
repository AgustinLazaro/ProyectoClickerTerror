using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TimingGame : MonoBehaviour, IApp
{
    [Header("Configuracion")]
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private float barSpeed = 1.5f;
    [SerializeField] private float greenZoneSpeed = 0.8f;      // nuevo — velocidad de movimiento de la zona
    [SerializeField] private float initialGreenZoneSize = 0.3f;  // nuevo — tamaño inicial (0 a 1)
    [SerializeField] private float minimumGreenZoneSize = 0.05f;  // nuevo — tamaño mínimo
    [SerializeField] private float shrinkSpeed = 0.02f;       // nuevo — cuánto se achica por segundo

    [Header("UI")]
    [SerializeField] private Button stopButton;
    [SerializeField] private Slider sliderBar;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private RectTransform handleArea; // arrastrar "Handle Slide Area"
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("References")]
    [SerializeField] private AppController appController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PCAudioPlayer pcAudio;
    //[SerializeField] private SFXEventChannelSO sfxChannel;

    private float _remainingTime;
    private float _barDirection = 1f;
    private float _zoneDirection = 1f;
    private float _currentGreenZoneSize;
    private float _greenZonePosition;       // posicion fija de la zona verde (0 a 1)
    private bool _isGameActive = false;

    private float greenZoneMin;
    private float greenZoneMax;

    private void Awake()
    {
        stopButton.onClick.AddListener(OnStop);
    }

    public void OnAppOpen() => StartGame();
    public void OnAppClose() => StopGame();

    private void StartGame()
    {
        _remainingTime = timeLimit;
        sliderBar.value = 0f;
        _barDirection = 1f;
        _zoneDirection = 1f;
        _currentGreenZoneSize = initialGreenZoneSize;
        _greenZonePosition = 0.5f;
        _isGameActive = true;

        resultText.gameObject.SetActive(false);
        ActualizarUI();
    }

    private void StopGame()
    {
        _isGameActive = false;
    }

    private void Update()
    {
        if (!_isGameActive) return;

        _remainingTime -= Time.deltaTime;
        timerText.text = $"{Mathf.CeilToInt(_remainingTime)}";

        MoveBar();
        MoveAndShrinkGreenZone();

        if (_remainingTime <= 0f)
            GameOver(win: false);
    }

    private void MoveBar()
    {
        sliderBar.value += _barDirection * barSpeed * Time.deltaTime;

        if (sliderBar.value >= 1f)
        {
            sliderBar.value = 1f;
            _barDirection = -1f;
        }
        else if (sliderBar.value <= 0f)
        {
            sliderBar.value = 0f;
            _barDirection = 1f;
        }
    }

    private void MoveAndShrinkGreenZone()
    {
        // Move the green zone
        _greenZonePosition += _zoneDirection * greenZoneSpeed * Time.deltaTime;

        // Bounce at the edges while considering the zone size
        float halfSize = _currentGreenZoneSize / 2f;

        if (_greenZonePosition + halfSize >= 1f)
        {
            _greenZonePosition = 1f - halfSize;
            _zoneDirection = -1f;
        }
        else if (_greenZonePosition - halfSize <= 0f)
        {
            _greenZonePosition = halfSize;
            _zoneDirection = 1f;
        }

        // Gradually shrink the green zone
        _currentGreenZoneSize -= shrinkSpeed * Time.deltaTime;
        _currentGreenZoneSize = Mathf.Max(_currentGreenZoneSize, minimumGreenZoneSize);

        // Update boundaries
        greenZoneMin = _greenZonePosition - _currentGreenZoneSize / 2f;
        greenZoneMax = _greenZonePosition + _currentGreenZoneSize / 2f;

        UpdateGreenZone();
    }

    private void UpdateGreenZone()
    {
        //float totalWidth = sliderBar.GetComponent<RectTransform>().rect.width;
        float totalWidth = handleArea.rect.width;

        // posición desde el extremo izquierdo, igual que el handle del slider
        float posX = (_greenZonePosition * totalWidth) - (totalWidth / 2f);

        greenZone.anchoredPosition = new Vector2(posX, greenZone.anchoredPosition.y);
        greenZone.sizeDelta = new Vector2(
            _currentGreenZoneSize * totalWidth,
            greenZone.sizeDelta.y
        );
    }

    private void OnStop()
    {
        if (!_isGameActive) return;

        pcAudio.PlaySound(SoundID.Click);

        bool isInsideGreenZone = 
            sliderBar.value >= greenZoneMin &&
            sliderBar.value <= greenZoneMax;

        Debug.Log($"Valor barra: {sliderBar.value:F3} | ZonaMin: {greenZoneMin:F3} | ZonaMax: {greenZoneMax:F3} | Dentro: {isInsideGreenZone}");

        GameOver(win: isInsideGreenZone);
    }

    private void GameOver(bool win)
    {
        _isGameActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "¡Ganaste!" : "¡Perdiste!";

        if (win)
        {
            pcAudio.PlaySound(SoundID.WinJingle);
            scoreManager.AddPoints(40);
        }
        else
        {
            pcAudio.PlaySound(SoundID.LoseJingle);
        }

        StartCoroutine(VolverAlHomeScreen());
    }

    private IEnumerator VolverAlHomeScreen()
    {
        yield return new WaitForSeconds(1.5f);
        appController.CloseCurrentApp();
    }

    private void ActualizarUI()
    {
        timerText.text = $"{Mathf.CeilToInt(_remainingTime)}s";
    }

    private void OnDestroy()
    {
        stopButton.onClick.RemoveAllListeners();
    }
}
