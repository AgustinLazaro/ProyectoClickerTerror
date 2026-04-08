using UnityEngine;

public class TazaCafe : MonoBehaviour
{
    
    public SistemaEstamina sistemaEstamina;

    
    void OnMouseDown()
    {
       
        Debug.Log("Tomaste un sorbo de café. Recargando energías...");
       
       
        {
           
            sistemaEstamina.TomarSorbito();
        }
      
    }
}