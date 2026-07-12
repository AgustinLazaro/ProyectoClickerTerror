using UnityEngine;

[CreateAssetMenu(fileName = "NewParanoiaStats", menuName = "Stats/Paranoia Stats")]
public class ParanoiaStatsSO : ScriptableObject
{
    public float maxStamina = 1000f;
    public float baseDrainSpeed = 5f;
    public float penaltyThreshold = 5f;
    public float penaltyMultiplier = 2f;
}