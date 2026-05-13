using UnityEngine;

public class CoffeeCupLazaro : MonoBehaviour
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

    void Start()
    {
        if (liquid != null)
        {
            emptyScale = liquid.localScale;
        }
    }

    public void TryFill(PlayerInteraction player)
    {
        if (!isFull && player.hasCoffeePot && isOnTable)
        {
            fillProgress += fillSpeed * Time.deltaTime;

            if (liquid != null)
            {
                liquid.localScale = Vector3.Lerp(emptyScale, fullScale, fillProgress / 100f);
            }

            if (fillProgress >= 100f)
            {
                isFull = true;
                fillProgress = 100f;
                Debug.Log("Cup is full. Left Click to grab it.");
            }
        }
    }

    public void GrabCup(PlayerInteraction player)
    {
        if (isFull && isOnTable)
        {
            player.hasFullCup = true;
            player.hasCoffeePot = false;
            isOnTable = false;

            if (coffeeMaker != null) coffeeMaker.StartBrewing();

            if (cupMesh != null) cupMesh.enabled = false;
            if (liquidMesh != null) liquidMesh.enabled = false;

            Debug.Log("Grabbed the cup. Right Click to drink.");
        }
    }

    public void PlaceCup(PlayerInteraction player)
    {
        if (!isOnTable && player.hasEmptyCup)
        {
            player.hasEmptyCup = false;
            isOnTable = true;

            isFull = false;
            fillProgress = 0f;
            if (liquid != null) liquid.localScale = emptyScale;

            if (cupMesh != null) cupMesh.enabled = true;
            if (liquidMesh != null) liquidMesh.enabled = true;

            Debug.Log("Placed empty cup on table. Ready for next brew.");
        }
    }
}