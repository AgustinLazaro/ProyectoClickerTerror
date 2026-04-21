using UnityEngine;

public class ComputerInteraction : MonoBehaviour
{
    // Arrastrá acá el objeto 'arms_rig' (el que tiene el Animator)
    public Animator misBrazos;

    // Empezamos en true porque el jugador arranca sentado
    private bool estaSentado = true;

    public void UsarComputadora()
    {
        // Cambiamos el estado (si era true, pasa a false)
        estaSentado = !estaSentado;

        if (misBrazos != null)
        {
            // Le mandamos la orden directo al parámetro del Animator
            misBrazos.SetBool("EnPC", estaSentado);
        }

        Debug.Log("¿Está sentado?: " + estaSentado);
    }
}