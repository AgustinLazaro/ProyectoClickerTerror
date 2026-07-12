using UnityEngine;

public class BreakerManager : MonoBehaviour
{
    [Header("Settings")]
    public BreakerSwitch[] allSwitches;
    public Light houseLight;
    public float staminaRecoveryAmount = 20f; 

    [Header("Managers")]
    [SerializeField] private PlayerParanoia paranoia;
    void Start()
    {
        if (houseLight != null) houseLight.enabled = false;

       
        if (paranoia == null)
        {
            paranoia = Object.FindFirstObjectByType<PlayerParanoia>();
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

    
        if (paranoia != null)
        {
            paranoia.RefillStamina(staminaRecoveryAmount);
        }
    }
}