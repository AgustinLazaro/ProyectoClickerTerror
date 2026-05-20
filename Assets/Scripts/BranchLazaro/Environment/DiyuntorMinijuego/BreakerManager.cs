using UnityEngine;

public class BreakerManager : MonoBehaviour
{
    [Header("Settings")]
    public BreakerSwitch[] allSwitches;
    public Light houseLight;
    public float staminaRecoveryAmount = 20f; 

    [Header("Managers")]
    public ParanoiaManager paranoiaManager;

    void Start()
    {
        if (houseLight != null) houseLight.enabled = false;

       
        if (paranoiaManager == null)
        {
            paranoiaManager = Object.FindFirstObjectByType<ParanoiaManager>();
        }
    }

    public void CheckSwitches()
    {
        int onCount = 0;
        foreach (BreakerSwitch s in allSwitches)
        {
            if (s.isOn) onCount++;
        }

        if (onCount >= allSwitches.Length)
        {
            RestorePower();
        }
    }

    void RestorePower()
    {
        Debug.Log("POWER RESTORED: Sanity increased!");

        if (houseLight != null) houseLight.enabled = true;

    
        if (paranoiaManager != null)
        {
            paranoiaManager.RefillStamina(staminaRecoveryAmount);
        }
    }
}