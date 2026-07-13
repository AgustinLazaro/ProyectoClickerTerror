using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvoidClickGame : MonoBehaviour, IApp
{
    [Header("Tiempo")]
    [SerializeField] private float timeLimit = 20f;
    [SerializeField] private float baseTimeVisibility = 2f;
    [SerializeField] private float minTimeVisibility = 0.8f;
    [SerializeField] private float roundInterval = 1.5f;
    [SerializeField] private int totalRounds = 6;
    [SerializeField] private float timeMargin = 0.3f; // tiempo extra antes de perder si faltan correctos

    [Header("Dificultad Progresiva")]
    [SerializeField] private int baseRights = 2;
    [SerializeField] private int rightsMax = 5;
    [SerializeField] private int baseWrongs = 2;
    [SerializeField] private int wrongsMax = 5;
    [SerializeField] private int roundsToIncreseDificulty = 2; // cada X rondas, +1 objeto
    [SerializeField] private float visibilityReductionPerRound = 0.12f;

    [Header("Spawn")]
    [SerializeField] private ClickableObject objectPrefab;
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private Vector2 sizeObject = new Vector2(120f, 120f); // para evitar overlap y bordes
    [SerializeField] private int maxAttempsPerPosition = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI roundsText;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("References")]
    [SerializeField] private AppController appController;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private PCAudioPlayer pcAudio;

    // Pool de objetos reciclables (evita Instantiate/Destroy en cada ronda)
    private readonly List<ClickableObject> _pool = new List<ClickableObject>();
    private readonly List<ClickableObject> _activesInRound = new List<ClickableObject>();

    private float _timeLeft;
    private bool _isGameActive = false;
    private int _currentRound = 0;
    private int _rightsClickedInRound = 0;
    private int _rightsShowedInRound = 0;  
    private bool _waitingInput = false;
    private int _lastSecondShowing = -1;

    private void Awake()
    {
        PrecalentarPool(rightsMax + wrongsMax);
    }

    private void PrecalentarPool(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            var obj = Instantiate(objectPrefab, spawnArea);
            obj.OnClicked += OnObjetClicked;
            obj.Hide();
            _pool.Add(obj);
        }
    }

    public void OnAppOpen() => StartGame();
    public void OnAppClose() => StopGame();

    private void StartGame()
    {
        _timeLeft = timeLimit;
        _currentRound = 0;
        _lastSecondShowing = -1;
        _isGameActive = true;

        HideActives();
        resultText.gameObject.SetActive(false);
        UpdateUI(forzar: true);

        StartCoroutine(RoundsRoutine());    //new
    }

    private void StopGame()
    {
        _isGameActive = false;
        StopAllCoroutines();    //new
        HideActives();
    }

    private void Update()
    {
        if (!_isGameActive) return;

        _timeLeft -= Time.deltaTime;
        UpdateUI();

        if (_timeLeft <= 0f)
            GameOver(win: false);
    }

    private IEnumerator RoundsRoutine()
    {
        while (_isGameActive && _currentRound < totalRounds)
        {
            yield return new WaitForSeconds(roundInterval);
            if (!_isGameActive) yield break;

            float tiempoVisible = ShowRound();
            yield return new WaitForSeconds(tiempoVisible);
            if (!_isGameActive) yield break;

            if (_rightsClickedInRound < _rightsShowedInRound)
            {
                //Margen de gracia: los objetos siguen clickeables mientras esperamos.
                yield return new WaitForSeconds(timeMargin);
                if (!_isGameActive) yield break;

                if(_rightsClickedInRound < _rightsShowedInRound) 
                {
                    GameOver(win: false);
                    yield break;
                }
            }

            HideActives();
            _currentRound++;
            UpdateUI(forzar: true);
        }

        if (_isGameActive)
            GameOver(win: true);
    }

    //Calcula la dificultad de la ronda, spawnea objetos y devuelve el tiempo visible.
    private float ShowRound()
    {
        HideActives();
        _rightsClickedInRound = 0;
        _waitingInput = true;

        int cantidadCorrectos = Mathf.Clamp(
            baseRights + _currentRound / roundsToIncreseDificulty,
            baseRights, rightsMax);

        int cantidadIncorrectos = Mathf.Clamp(
            baseWrongs + _currentRound / roundsToIncreseDificulty,
            baseWrongs, wrongsMax);

        _rightsShowedInRound = cantidadCorrectos;

        var posicionesUsadas = new List<Vector2>();
        SpawnGroup(cantidadCorrectos, esCorrecto: true, posicionesUsadas);
        SpawnGroup(cantidadIncorrectos, esCorrecto: false, posicionesUsadas);

        return Mathf.Max(minTimeVisibility,
            baseTimeVisibility - _currentRound * visibilityReductionPerRound);
    }

    private void SpawnGroup(int cantidad, bool esCorrecto, List<Vector2> posicionesUsadas)
    {
        for (int i = 0; i < cantidad; i++)
        {
            var obj = GetFromPool();
            if (obj == null) break; // pool agotado, no debería pasar si correctosMax+incorrectosMax está bien seteado

            Vector2 posicion = GenerateValidPosition(posicionesUsadas);
            posicionesUsadas.Add(posicion);

            obj.SetPosition(posicion);
            obj.Configurate(esCorrecto);
            _activesInRound.Add(obj);
        }
    }

    private ClickableObject GetFromPool()
    {
        foreach (var obj in _pool)
        {
            if (!obj.gameObject.activeSelf)
                return obj;
        }
        return null;
    }

    //Busca una posición aleatoria dentro de spawnArea que no se superponga con las ya usadas.
    private Vector2 GenerateValidPosition(List<Vector2> posicionesUsadas)
    {
        Rect rect = spawnArea.rect;
        float margenX = sizeObject.x * 0.5f;
        float margenY = sizeObject.y * 0.5f;

        for (int intento = 0; intento < maxAttempsPerPosition; intento++)
        {
            float x = Random.Range(rect.xMin + margenX, rect.xMax - margenX);
            float y = Random.Range(rect.yMin + margenY, rect.yMax - margenY);
            Vector2 candidata = new Vector2(x, y);

            bool superpone = false;
            foreach (var usada in posicionesUsadas)
            {
                if (Vector2.Distance(candidata, usada) < Mathf.Max(sizeObject.x, sizeObject.y))
                {
                    superpone = true;
                    break;
                }
            }

            if (!superpone)
                return candidata;
        }

        // Si no encontró lugar libre en los intentos permitidos, devuelve la última candidata igual.
        float xFallback = Random.Range(rect.xMin + margenX, rect.xMax - margenX);
        float yFallback = Random.Range(rect.yMin + margenY, rect.yMax - margenY);
        return new Vector2(xFallback, yFallback);
    }

    private void HideActives()
    {
        foreach (var obj in _activesInRound)
            obj.Hide();

        _activesInRound.Clear();
        _waitingInput = false;
    }

    private void OnObjetClicked(ClickableObject obj)
    {
        if (!_isGameActive || !_waitingInput) return;

        if (obj.IsCorrect)
        {
            obj.Hide();
            _activesInRound.Remove(obj);
            _rightsClickedInRound++;
            pcAudio.PlaySound(SoundID.Click);
        }
        else
        {
            pcAudio.PlaySound(SoundID.Error);
            GameOver(win: false);
        }
    }

    private void GameOver(bool win)
    {
        _isGameActive = false;
        StopAllCoroutines();
        HideActives();

        resultText.gameObject.SetActive(true);
        resultText.text = win ? "¡Ganaste!" : "¡Perdiste!";

        if (win)
            pcAudio.PlaySound(SoundID.WinJingle);
        else
            pcAudio.PlaySound(SoundID.LoseJingle);

        scoreManager.AddPoints(win ? 10 : 0);
        StartCoroutine(BackToHomeScreen());
    }

    private IEnumerator BackToHomeScreen()
    {
        yield return new WaitForSeconds(1.5f);
        appController.CloseCurrentApp();
    }

    private void UpdateUI(bool forzar = false)
    {
        int segundoActual = Mathf.CeilToInt(_timeLeft);
        if (forzar || segundoActual != _lastSecondShowing)
        {
            timeText.text = $"{segundoActual}s";
            _lastSecondShowing = segundoActual;
        }

        roundsText.text = $"Ronda {_currentRound + 1} / {totalRounds}";
    }

    private void OnDestroy()
    {
        foreach (var obj in _pool)
        {
            if (obj != null)
                obj.OnClicked -= OnObjetClicked;
        }
    }
}