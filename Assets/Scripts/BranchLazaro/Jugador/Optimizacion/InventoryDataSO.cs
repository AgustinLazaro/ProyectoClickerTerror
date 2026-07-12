using UnityEngine;


[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Inventory/Data Inventory")]
public class InventoryDataSO : ScriptableObject
{
    [Header("Current Items")]
    public bool hasCoffeePot = false;

    public CupState currentCupState = CupState.None;

    public void ResetInventory()
    {
        hasCoffeePot = false;
        currentCupState = CupState.None;
    }
}
