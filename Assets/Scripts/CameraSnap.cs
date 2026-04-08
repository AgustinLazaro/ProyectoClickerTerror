using UnityEngine;

public class CameraSnap : MonoBehaviour
{
    
    [Header("Ángulos de Vista (Relativos al centro)")]
    public float anguloVentana = -80f; 
    public float anguloPuerta = 80f;    
    public float anguloAtras = 180f;    

    
    [Header("Configuración")]
    public float velocidadGiro = 15f;

    
    float rotacionY = 0f;
    float centroY;
    Quaternion rotacionObjetivo;

    void Start()
    {
       

       
        Vector3 rotacionInicial = transform.localRotation.eulerAngles;
        rotacionY = rotacionInicial.y;

        if (rotacionY > 180f) rotacionY -= 360f;

       
        centroY = rotacionY;

      
        rotacionObjetivo = Quaternion.Euler(0f, centroY, 0f);
    }

    void Update()
    {
        
        DeterminarObjetivo();

       
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
    }

    void DeterminarObjetivo()
    {
     
        if (Input.GetKey(KeyCode.S))
        {
           
            rotacionObjetivo = Quaternion.Euler(0f, centroY + anguloAtras, 0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotacionObjetivo = Quaternion.Euler(0f, centroY + anguloPuerta, 0f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            
            rotacionObjetivo = Quaternion.Euler(0f, centroY + anguloVentana, 0f);
        }
        else
        {
            
            rotacionObjetivo = Quaternion.Euler(0f, centroY, 0f);
        }
    }
}