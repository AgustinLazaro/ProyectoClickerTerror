using UnityEngine;
public class CoffeeCup : MonoBehaviour
{
    public StaminaSystem staminaSystem;
    private void OnMouseDown()
    {
        staminaSystem.TakeSip();
        Debug.Log("Tomaste un sorbo de café. Recargando energías..."); 
    }
}