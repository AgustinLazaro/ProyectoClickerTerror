using UnityEngine;
using System.Collections;

// 1. Hereda de InteractableBase
public class CoffeeMaker : InteractableBase
{
    [Header("Settings")]
    public GameObject potVisual;
    public float brewTime = 10f;

    [Header("Conexión de Audio channel")]
    public SFXEventChannelSO sfxChannel;

    [Header("State")]
    public bool isPotReady = true;

    public override void OnClickDown(PlayerInteraction player)
    {
        if (isPotReady && !player.HasCoffeePot && player.CurrentCupState != CupState.Full)
        {
            player.HasCoffeePot = true;
            isPotReady = false;

            if (potVisual != null) potVisual.SetActive(false);

            Debug.Log("Grabbed coffee pot. Hold Left Click on cup to fill.");
        }
        else if (!isPotReady)
        {
            Debug.Log("Coffee brewing. Wait.");
        }
    }

    public void StartBrewing()
    {
        StartCoroutine(BrewRoutine());
    }

    // ... adentro de tu rutina ...
    IEnumerator BrewRoutine()
    {
        yield return new WaitForSeconds(brewTime);
        isPotReady = true;
        if (potVisual != null) potVisual.SetActive(true);

        // Acá está la magia, idéntico a PlayerParanoia
        if (sfxChannel != null) sfxChannel.Raise(SoundID.CoffeReady);

        Debug.Log("Coffee maker is ready again!");
    }
}