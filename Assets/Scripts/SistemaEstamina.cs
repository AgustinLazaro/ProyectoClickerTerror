using UnityEngine;
using UnityEngine.UI;

public class SistemaEstamina : MonoBehaviour
{
    public float estaminaMaxima = 100f;
    public float estaminaActual;
    public float velocidadCansancio = 5f; 

    public Image sombraOjos; 

    void Start()
    {
        estaminaActual = estaminaMaxima;
    }

    void Update()
    {
        if (estaminaActual> 0)
            estaminaActual-=velocidadCansancio*Time.deltaTime;

        float alpha = 1f - (estaminaActual / estaminaMaxima);

        Color colorActual = sombraOjos.color;
        colorActual.a = alpha;
        sombraOjos.color = colorActual;
    }

   
    public void TomarSorbito()
    {
        estaminaActual += 20f; 
        
       
        if (estaminaActual > estaminaMaxima) 
        {
            estaminaActual = estaminaMaxima;
        }
    }
}