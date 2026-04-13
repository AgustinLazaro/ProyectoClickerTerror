using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float fatigueSpeed = 5f; 

    [SerializeField] private Image eyeShadow; 

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (currentStamina> 0)
            currentStamina-=fatigueSpeed*Time.deltaTime;

        float alpha = 1f - (currentStamina / maxStamina);

        Color colorActual = eyeShadow.color;
        colorActual.a = alpha;
        eyeShadow.color = colorActual;
    }

   
    public void TakeSip()
    {
        currentStamina += 20f; 
        if (currentStamina > maxStamina) 
        {
            currentStamina = maxStamina;
        }
    }
}