using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimingGame : MonoBehaviour, IApp
{
    [Header("Configuracion")]
    [SerializeField] private float oscilationSpeed = 1.5f;
    [SerializeField] private float timeLmit = 10f;
    [SerializeField] private float greenZoneSize = 0.15f; // 0 a 1

    [Header("UI")]
    [SerializeField] private Button stopButton;
    [SerializeField] private Slider SliderBar;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private SFXEventChannelSO sfxChannel;

    private float _timeLeft;
    private float _direction = 1f;
    private bool isGameActive = false;

    private AppController _appController;
    private ScoreManager _scoreManager;
    //private PCAudioManager _audioManager;

    // posicion fija de la zona verde (0 a 1)
    private float greenZoneMin;
    private float greenZoneMax;

    private void Awake()
    {
        _appController = FindAnyObjectByType<AppController>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        //_audioManager = FindAnyObjectByType<PCAudioManager>();
        stopButton.onClick.AddListener(OnStop);
        GreenZoneConfiguration();
    }

    private void GreenZoneConfiguration()
    {
        // zona verde centrada en 0.5, con el tamaño configurado
        float center = 0.5f;
        greenZoneMin = center - greenZoneSize / 2f;
        greenZoneMax = center + greenZoneSize / 2f;
    }

    public void OnAppOpen() => StartGame();
    public void OnAppClose() => StopGame();

    private void StartGame()
    {
        _timeLeft = timeLmit;
        SliderBar.value = 0f;
        _direction = 1f;
        isGameActive = true;

        resultText.gameObject.SetActive(false);
        ActualizarUI();
    }

    private void StopGame()
    {
        isGameActive = false;
    }

    private void Update()
    {
        if (!isGameActive) return;

        // oscila el slider de 0 a 1
        SliderBar.value += _direction * oscilationSpeed * Time.deltaTime;

        if (SliderBar.value >= 1f)
        {
            SliderBar.value = 1f;
            _direction = -1f;
        }
        else if (SliderBar.value <= 0f)
        {
            SliderBar.value = 0f;
            _direction = 1f;
        }

        _timeLeft -= Time.deltaTime;
        ActualizarUI();

        if (_timeLeft <= 0f)
            GameOver(win: false);
    }

    private void OnStop()
    {
        if (!isGameActive) return;

        //_audioManager.PlayClick();
        sfxChannel.Raise(SoundID.Click);

        bool enZonaVerde = SliderBar.value >= greenZoneMin &&
                           SliderBar.value <= greenZoneMax;

        GameOver(win: enZonaVerde);
    }

    private void GameOver(bool win)
    {
        isGameActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = win ? "¡Ganaste!" : "¡Perdiste!";

        if (win)
        {
            //_audioManager.PlayWinJingle();
            sfxChannel.Raise(SoundID.WinJingle);
            _scoreManager.AddPoints(40);
        }
        else
        {
            //_audioManager.PlayLoseJingle();
            sfxChannel.Raise(SoundID.LoseJingle);
        }

        StartCoroutine(VolverAlHomeScreen());
    }

    private System.Collections.IEnumerator VolverAlHomeScreen()
    {
        yield return new WaitForSeconds(1.5f);
        _appController.CloseCurrentApp();
    }

    private void ActualizarUI()
    {
        timeText.text = $"{Mathf.CeilToInt(_timeLeft)}s";
    }

    private void OnDestroy()
    {
        stopButton.onClick.RemoveAllListeners();
    }
}
