using UnityEngine;
using System.Collections; 

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

    private int paranoiaPhase = 0;

    void Start()
    {
        
        bool hasScreen = (blackScreenUI != null);
        if (hasScreen == true)
        {
            blackScreenUI.SetActive(false);
        }
    }

    void Update()
    {
       
        timeWithoutBlinking = timeWithoutBlinking + Time.deltaTime;

        
        bool isPressingSpace = Input.GetKeyDown(KeyCode.Space);

        if (isPressingSpace == true && isBlinking == false)
        {
            StartCoroutine(BlinkRoutine());
        }

      
        float currentDrainSpeed = baseDrainSpeed;

       
        if (timeWithoutBlinking > penaltyThreshold)
        {
            currentDrainSpeed = baseDrainSpeed * penaltyMultiplier;
          
        }

      
        currentStamina = currentStamina - (currentDrainSpeed * Time.deltaTime);
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

      
        UpdateParanoiaEvents();
    }

    IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        
        blackScreenUI.SetActive(true);

        

        
        timeWithoutBlinking = 0f;

       
        float blinkDuration = 0.2f; 
        yield return new WaitForSeconds(blinkDuration);

    
        blackScreenUI.SetActive(false);

        isBlinking = false;
    }

    void UpdateParanoiaEvents()
    {
        // FASE 0: Normal (Entre 100 y 60)
        if (currentStamina >= 60f && paranoiaPhase != 0)
        {
            paranoiaPhase = 0;
            Debug.Log("Phase 0: Everything is normal.");
        }

        // FASE 1: Ansiedad (Entre 59 y 30)
        if (currentStamina < 60f && currentStamina >= 30f && paranoiaPhase != 1)
        {
            paranoiaPhase = 1;
            Debug.Log("Phase 1: Anxiety. Playing distant noises...");
        }

        // FASE 2: Paranoia (Entre 29 y 10)
        if (currentStamina < 30f && currentStamina >= 10f && paranoiaPhase != 2)
        {
            paranoiaPhase = 2;
            Debug.Log("Phase 2: Paranoia. Flickering lights...");
        }

        // FASE 3: Crítico (Menor a 10)
        if (currentStamina < 10f && paranoiaPhase != 3)
        {
            paranoiaPhase = 3;
            Debug.Log("Phase 3: Critical. The hallway is stretching!");
        }
    }
    public void RefillStamina(float cantidad)
    {
        currentStamina += cantidad;

        
        currentStamina = Mathf.Clamp(currentStamina, 0f, 100f);

        Debug.Log("Stamina refilled! Ahora tenés: " + currentStamina);
    }
}




// NOTAs: 

// ESTAMINA (100 a 0):
// - Para que el nivel dure 5m:  baseDrainSpeed = 0.33f;
// - Para que el nivel dure 10m: baseDrainSpeed = 0.16f;
//
// PARPADEO (Balance de tensión vs frustración):
// - Tiempo límite sin parpadear: penaltyThreshold = 12f; (o 15f)
// - Castigo por olvidarse:  penaltyMultiplier = 3f; (o 4f)
/////////////////////////////////////////////////////////////////////