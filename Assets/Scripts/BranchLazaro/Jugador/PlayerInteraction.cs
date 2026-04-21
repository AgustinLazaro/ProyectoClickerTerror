using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuracion de Raycast")]
    public float distanciaDelRayo = 3f;

    [Header("Referencias")]
    public Camera miCamara;   
    public Animator misBrazos; 

    void Update()
    {
    
        if (Input.GetMouseButtonDown(0))
        {
            
            if (misBrazos != null)
            {
                misBrazos.SetTrigger("Agarre");
            }

            
            RaycastHit golpe;
            if (Physics.Raycast(miCamara.transform.position, miCamara.transform.forward, out golpe, distanciaDelRayo))
            {
                CoffeeMaker cafetera = golpe.collider.GetComponent<CoffeeMaker>();
                if (cafetera != null)
                {
                    cafetera.Interactuar();
                }
            }
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit golpe;
            
            if (Physics.Raycast(miCamara.transform.position, miCamara.transform.forward, out golpe, distanciaDelRayo))
            {
                ComputerInteraction pc = golpe.collider.GetComponent<ComputerInteraction>();
                if (pc != null)
                {
                    pc.UsarComputadora();
                }
            }
        }
    }
}