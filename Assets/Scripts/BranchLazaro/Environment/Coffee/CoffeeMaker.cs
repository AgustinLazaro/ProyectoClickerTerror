using UnityEngine;
using System.Collections;

public class CoffeeMaker : MonoBehaviour
{
    [Header("Settings")]
    public GameObject potVisual;
    public float brewTime = 10f;

    [Header("State")]
    public bool isPotReady = true;

    public void Interact(PlayerInteraction player)
    {
        if (isPotReady && !player.HasCoffeePot && player.CurrentCupState != CupState.Full)
        {
            player.HasCoffeePot = true;
            isPotReady = false;

            if (potVisual != null) potVisual.SetActive(false);

            Debug.Log("Grabbed the coffee pot. Hold Left Click on the cup to fill it.");
        }
        else if (!isPotReady)
        {
            Debug.Log("Coffee maker is currently brewing. Please wait.");
        }
    }
    public void StartBrewing()
    {
        StartCoroutine(BrewRoutine());
    }

    IEnumerator BrewRoutine()
    {
        yield return new WaitForSeconds(brewTime);
        isPotReady = true;
        if (potVisual != null) potVisual.SetActive(true);
        Debug.Log("Coffee maker is ready again!");
    }
}