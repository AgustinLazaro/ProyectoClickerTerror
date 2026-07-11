using UnityEngine;
using System.Collections;

public class PlayerParanoia : MonoBehaviour
{
    [Header("Stamina System")]
    public float currentStamina = 1000f;
    public float baseDrainSpeed = 5f;

    public int paranoiaPhase { get; private set; } = 0;

    [Header("Blink System")]
    public float penaltyThreshold = 5f;
    public float penaltyMultiplier = 2f;
    public GameObject blackScreenUI;

    [Header("Conexión de Audio channel")]
    public SFXEventChannelSO sfxChannel;

    [Header("Referencias")]
    [SerializeField] private GameManagerMarian managerMarian;

    private float timeWithoutBlinking = 0f;
    private bool isBlinking = false;
    private int lastPhase = 0;

    private void Start()
    { 
         FindAnyObjectByType<GameManagerMarian>();
        blackScreenUI.SetActive(false);
    }

    private void Update()
    {
        HandleBlink();
        HandleStamina();
        UpdateParanoiaPhase();
    }

    private void HandleBlink()
    {
        timeWithoutBlinking += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && !isBlinking)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    private void HandleStamina()
    {
        float currentDrainSpeed = timeWithoutBlinking > penaltyThreshold ? baseDrainSpeed * penaltyMultiplier : baseDrainSpeed;

        currentStamina -= currentDrainSpeed * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

        if (currentStamina <= 0f)
        {
            managerMarian.GameOver();
        }
    }

    private void UpdateParanoiaPhase()
    {
        if (currentStamina >= 60f) paranoiaPhase = 0;
        else if (currentStamina >= 30f) paranoiaPhase = 1;
        else if (currentStamina >= 10f) paranoiaPhase = 2;
        else paranoiaPhase = 3;

        // Si cambiamos de fase y entramos a estado crítico, reproducimos el sonido
        if (paranoiaPhase != lastPhase)
        {
            if (paranoiaPhase == 3 && lastPhase < 3)
            {
                if (sfxChannel != null) sfxChannel.Raise(SoundID.StaminaLow);
            }
            lastPhase = paranoiaPhase;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        // ¡Disparamos el sonido del parpadeo!
        if (sfxChannel != null) sfxChannel.Raise(SoundID.Blink);
        if (blackScreenUI != null) blackScreenUI.SetActive(true);

        timeWithoutBlinking = 0f;

        yield return new WaitForSeconds(0.2f);

        if (blackScreenUI != null) blackScreenUI.SetActive(false);
        isBlinking = false;
    }

    public void RefillStamina(float cantidad)
    {
        currentStamina += cantidad;
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

        // ¡Disparamos el sonido de recuperación!
        if (sfxChannel != null) sfxChannel.Raise(SoundID.StaminaRestored);

        Debug.Log("Stamina refilled! Ahora tenés: " + currentStamina);
    }
}
