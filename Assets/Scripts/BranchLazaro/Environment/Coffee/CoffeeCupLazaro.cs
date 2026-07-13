using UnityEngine;
using System.Collections;

public class CoffeeCupLazaro : InteractableBase
{
    [Header("References")]
    public Transform liquid;
    public CoffeeMaker coffeeMaker;

    [Header("Visuals")]
    public MeshRenderer cupMesh;
    public MeshRenderer liquidMesh;

    [Header("Settings")]
    public Vector3 fullScale;
    private Vector3 emptyScale;

    [Header("State")]
    public float fillProgress = 0f;
    public float fillSpeed = 40f;
    public bool isFull = false;
    public bool isOnTable = true;
    private bool isFilling = false;

    [Header("Audio Connection")]
    public SFXEventChannelSO sfxChannel;

    protected override void Start()
    {
        base.Start();
        emptyScale = liquid.localScale;
    }
    public override void OnClickHold(PlayerInteraction player) { }

    public override void OnClickDown(PlayerInteraction player)
    {
        Debug.Log("Cup clicked.");
        if (isOnTable && !isFull && !isFilling && player.HasCoffeePot)
        {
            StartCoroutine(FillCupRoutine());
        }
        else if (isOnTable && isFull)
        {
            GrabCup(player);
        }
        else if (!isOnTable && player.CurrentCupState == CupState.Empty)
        {
            PlaceCup(player);
        }
    }

    private IEnumerator FillCupRoutine()
    {
        isFilling = true;
        liquidMesh.enabled = true;

        while (fillProgress < 100f)
        {
            fillProgress += fillSpeed * Time.deltaTime;
            liquid.localScale = Vector3.Lerp(emptyScale, fullScale, fillProgress / 100f);

            yield return null; 
        }

        fillProgress = 100f;
        isFull = true;
        isFilling = false;
        Debug.Log("Cup full. Ready to grab.");
    }

    private void GrabCup(PlayerInteraction player)
    {
        player.CurrentCupState = CupState.Full;
        player.HasCoffeePot = false;
        isOnTable = false;

        coffeeMaker.StartBrewing();

        cupMesh.enabled = false;
        liquidMesh.enabled = false;

        Debug.Log("Grabbed the cup.");
    }

    private void PlaceCup(PlayerInteraction player)
    {
        player.CurrentCupState = CupState.None;
        isOnTable = true;
        isFull = false;
        fillProgress = 0f;

        cupMesh.enabled = true;
        liquidMesh.enabled = false;

        sfxChannel.Raise(SoundID.PlaceCup);

        Debug.Log("Placed empty cup.");
    }
}