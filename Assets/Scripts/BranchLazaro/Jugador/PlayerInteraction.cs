using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayDistance = 3f;

    [Header("References")]
    public Camera playerCamera;
    public Animator armsAnimator;

    [Header("Data (Arrastrá tu cubito acá)")]
    public InventoryDataSO inventoryData; 

    [Header("Managers")]
    public PlayerParanoia paranoiaManager;

    public bool HasCoffeePot
    {
        get { return inventoryData.hasCoffeePot; }
        set { inventoryData.hasCoffeePot = value; }
    }

    public CupState CurrentCupState
    {
        get { return inventoryData.currentCupState; }
        set { inventoryData.currentCupState = value; }
    }
    // -------------------

    void Start()
    {
        paranoiaManager = Object.FindFirstObjectByType<PlayerParanoia>();
        inventoryData.hasCoffeePot = false;
        inventoryData.currentCupState = CupState.None;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.TryGetComponent(out CoffeeCupLazaro cup))
                {
                    cup.TryFill(this);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (armsAnimator != null) armsAnimator.SetTrigger("Grab");
            HandleClickInteraction();
        }

        if (Input.GetMouseButtonDown(1) && CurrentCupState == CupState.Full)
        {
            DrinkCoffee();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.TryGetComponent(out ComputerInteraction pc))
                    pc.UseComputer();
            }
        }
    }

    private void HandleClickInteraction()
    {
        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
            return;

        if (hit.collider.TryGetComponent(out CoffeeMaker maker))
        {
            maker.Interact(this);
            return;
        }

        if (hit.collider.TryGetComponent(out CoffeeCupLazaro cup))
        {
            if (cup.isOnTable && cup.isFull)
                cup.GrabCup(this);
            else if (!cup.isOnTable && CurrentCupState == CupState.Empty)
                cup.PlaceCup(this);

            return;
        }

        if (hit.collider.TryGetComponent(out BreakerSwitch breaker))
        {
            breaker.Interact();
            return;
        }
    }

    void DrinkCoffee()
    {
        CurrentCupState = CupState.Empty;
        Debug.Log("Coffee consumed.");
        if (paranoiaManager != null) paranoiaManager.RefillStamina(30f);
    }
}