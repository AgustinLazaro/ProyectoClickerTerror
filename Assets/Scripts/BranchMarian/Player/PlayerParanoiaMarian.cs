using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerParanoiaMarian : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ParanoiaStatsSO statsData;

    [Header("Conexión de Audio channel")]
    [SerializeField] private SFXEventChannelSO sfxChannel;

    [Header("Blink System")]
    [SerializeField] private GameObject blackScreenUI;

    [Header("UI de Estamina")]
    [SerializeField] private Image staminaFillBar;

    [Header("Referencias")]
    [SerializeField] private GameManagerMarian managerMarian;

    private float _currentStamina;
    public float CurrentStamina
    {
        get { return _currentStamina; }
        private set
        {
            if (statsData != null)
                _currentStamina = Mathf.Clamp(value, 0f, statsData.maxStamina);
        }
    }

    public int ParanoiaPhase { get; private set; } = 0;

    private float timeWithoutBlinking = 0f;
    private bool isBlinking = false;
    private int lastPhase = 0;

    private void Start()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();

        if (blackScreenUI != null) blackScreenUI.SetActive(false);

        if (statsData != null) CurrentStamina = statsData.maxStamina;
    }

    private void Update()
    {
        if (statsData == null) return;

        timeWithoutBlinking += Time.deltaTime;

        HandleStamina();
        UpdateParanoiaPhase();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (staminaFillBar != null)
        {
            staminaFillBar.fillAmount = CurrentStamina / statsData.maxStamina;
        }
    }

    private void HandleStamina()
    {
        float currentDrainSpeed = timeWithoutBlinking > statsData.penaltyThreshold 
            ? statsData.baseDrainSpeed * statsData.penaltyMultiplier 
            : statsData.baseDrainSpeed;

        CurrentStamina -= currentDrainSpeed * Time.deltaTime;

        if (CurrentStamina <= 0f)
        {
            if (managerMarian != null) managerMarian.GameOver();
        }
    }

    private void UpdateParanoiaPhase()
    {
        float fase1Threshold = statsData.maxStamina * 0.60f;
        float fase2Threshold = statsData.maxStamina * 0.30f;
        float fase3Threshold = statsData.maxStamina * 0.10f;

        if (CurrentStamina >= fase1Threshold) ParanoiaPhase = 0;
        else if (CurrentStamina >= fase2Threshold) ParanoiaPhase = 1;
        else if (CurrentStamina >= fase3Threshold) ParanoiaPhase = 2;
        else ParanoiaPhase = 3;
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        if (sfxChannel != null) sfxChannel.Raise(SoundID.Blink);
        if (blackScreenUI != null) blackScreenUI.SetActive(true);

        timeWithoutBlinking = 0f;

        yield return new WaitForSeconds(0.2f);

        if (blackScreenUI != null) blackScreenUI.SetActive(false);
        isBlinking = false;
    }

    //Método público para recibir input desde PlayerInputHandler
    public void TryBlink()
    {
        if (!isBlinking)
            StartCoroutine(BlinkRoutine());
    }

    public void RefillStamina(float cantidad)
    {
        CurrentStamina += cantidad;

        if (sfxChannel != null) sfxChannel.Raise(SoundID.StaminaRestored);

        Debug.Log("Stamina refilled! Ahora tenés: " + CurrentStamina);
    }

    public void DrainStamina(float cantidad)
    {
        CurrentStamina -= cantidad;
    }
}    
