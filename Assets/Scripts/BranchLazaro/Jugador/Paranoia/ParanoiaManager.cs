using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices; 

public class ParanoiaManager : MonoBehaviour
{
    [Header("Stamina System")]
    public float currentStamina = 100f;
    public float baseDrainSpeed = 5f;

    [Header("Blink System (Parpadeo)")]
    public float timeWithoutBlinking = 0f;
    public float penaltyThreshold = 5f;
    public float penaltyMultiplier = 2f;
    public GameObject blackScreenUI;
    private bool isBlinking = false;

    [Header("Efectos Visuales (Volumes)")]
    public Volume volumeFase1;
    public Volume volumeFase2;
    public Volume volumeFase3;
    public float speedTransition = 1.5f; 

    private int paranoiaPhase = 0;

    [Header("Cámara y FOV")]
    public Camera playerCamera; 
    public float fovNormal = 60f; 
    public float fovCritico = 110f; 
    public float velocidadFov = 20f; 

    [SerializeField] GameManagerMarian managerMarian;

    private void Awake()
    {
        managerMarian = FindAnyObjectByType<GameManagerMarian>();
    }

    private void Start()
    {
        if (blackScreenUI != null)
        {
            blackScreenUI.SetActive(false);
        }

        
        if (volumeFase1 != null) volumeFase1.weight = 0f;
        if (volumeFase2 != null) volumeFase2.weight = 0f;
        if (volumeFase3 != null) volumeFase3.weight = 0f;
    }

    private void Update()
    {
        
        timeWithoutBlinking += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && !isBlinking)
        {
            StartCoroutine(BlinkRoutine());
        }

        
        float currentDrainSpeed = baseDrainSpeed;
        if (timeWithoutBlinking > penaltyThreshold)
        {
            currentDrainSpeed = baseDrainSpeed * penaltyMultiplier;
        }

        currentStamina -= (currentDrainSpeed * Time.deltaTime);
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

        
        if (currentStamina <= 0f)
            managerMarian.GameOver();
            
        
        UpdateParanoiaEvents();
        UpdateVFX();
    }

    private void UpdateParanoiaEvents()
    {
       
        if (currentStamina >= 60f && paranoiaPhase != 0)
        {
            paranoiaPhase = 0;
            Debug.Log(" FASE 0: Todo normal. (Estamina: " + currentStamina.ToString("F0") + ")");
        }

       
        else if (currentStamina >= 30f && currentStamina < 60f && paranoiaPhase != 1)
        {
            paranoiaPhase = 1;
            Debug.Log(" FASE 1: Ansiedad. Entra Volumen 1. (Estamina: " + currentStamina.ToString("F0") + ")");
        }

        // FASE 2: Paranoia (Entre 29 y 10)
        else if (currentStamina >= 10f && currentStamina < 30f && paranoiaPhase != 2)
        {
            paranoiaPhase = 2;
            Debug.Log(" FASE 2: Paranoia. Entra Volumen 2. (Estamina: " + currentStamina.ToString("F0") + ")");
        }

        // FASE 3: Crítico (Menor a 10)
        else if (currentStamina < 10f && paranoiaPhase != 3)
        {
            paranoiaPhase = 3;
            Debug.Log(" FASE 3: CRÍTICO. Entra Volumen 3. (Estamina: " + currentStamina.ToString("F0") + ")");
        }
    }

    private void UpdateVFX()
    {
        
        float targetFase1 = (paranoiaPhase == 1) ? 1f : 0f;
        float targetFase2 = (paranoiaPhase == 2) ? 1f : 0f;
        float targetFase3 = (paranoiaPhase == 3) ? 1f : 0f;

       
        if (volumeFase1 != null)
            volumeFase1.weight = Mathf.MoveTowards(volumeFase1.weight, targetFase1, speedTransition * Time.deltaTime);

        if (volumeFase2 != null)
            volumeFase2.weight = Mathf.MoveTowards(volumeFase2.weight, targetFase2, speedTransition * Time.deltaTime);

        if (volumeFase3 != null)
            volumeFase3.weight = Mathf.MoveTowards(volumeFase3.weight, targetFase3, speedTransition * Time.deltaTime);

        
        if (playerCamera != null)
        {
            
            float fovObjetivo = (paranoiaPhase == 3) ? fovCritico : fovNormal;

            
            playerCamera.fieldOfView = Mathf.MoveTowards(playerCamera.fieldOfView, fovObjetivo, velocidadFov * Time.deltaTime);
        }
    }

    IEnumerator BlinkRoutine()
    {
        isBlinking = true;
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
        Debug.Log("Stamina refilled! Ahora tenés: " + currentStamina);
    }
}


