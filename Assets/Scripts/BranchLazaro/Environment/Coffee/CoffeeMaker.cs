using UnityEngine;
using System.Collections;

public class CoffeeMaker : MonoBehaviour
{
    [Header("Configuracion")]
    public float energiaQueRecupera = 25f;
    public GameObject jarraVisual;

    public void Interactuar()
    {
        // 1. Ahora buscamos al ParanoiaManager
        ParanoiaManager paranoia = Object.FindFirstObjectByType<ParanoiaManager>();

        if (paranoia != null)
        {
            // 2. Le pasamos los puntos de energía
            paranoia.RefillStamina(energiaQueRecupera);
            Debug.Log("Café tomado: + " + energiaQueRecupera);
        }
        else
        {
            Debug.LogError("¡No encontré el ParanoiaManager en la escena!");
        }

        if (jarraVisual != null) StartCoroutine(EfectoJarra());
    }

    IEnumerator EfectoJarra()
    {
        jarraVisual.SetActive(false);
        yield return new WaitForSeconds(2f);
        jarraVisual.SetActive(true);
    }
}