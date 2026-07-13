using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Chair")]
    public bool isSitting = false;
    private Chair currentChair;

   
    [Header("Raycast Settings")]
    public float rayDistance = 3f;

    [Header("References")]
    public Camera playerCamera;
    public Animator armsAnimator;

    [Header("Data")]
    public InventoryDataSO inventoryData;

    [Header("Managers")]
    public PlayerParanoia paranoiaManager;

    public CharacterController characterController;
    private InteractableBase previousTarget;

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

    void Start()
    {
        paranoiaManager = Object.FindFirstObjectByType<PlayerParanoia>();
        inventoryData.hasCoffeePot = false;
        inventoryData.currentCupState = CupState.None;
    }

    void Update()
    {
        if (isSitting && Input.GetKeyDown(KeyCode.E))
        {
            StandUp();
        }

        if (Input.GetMouseButtonDown(1) && CurrentCupState == CupState.Full)
        {
            DrinkCoffee();
        }

        // 1. Detectar el objeto PRIMERO
        InteractableBase currentTarget = null;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
        {
            currentTarget = hit.collider.GetComponent<InteractableBase>();
        }

        if (currentTarget != previousTarget)
        {
            if (previousTarget != null) previousTarget.OnHoverExit();
            if (currentTarget != null) currentTarget.OnHoverEnter();
            previousTarget = currentTarget;
        }

        // 3. Inputs
        if (currentTarget != null)
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log($"Holding: {currentTarget.name}"); 
                currentTarget.OnClickHold(this);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Clicked: {currentTarget.name}");
                if (armsAnimator != null) armsAnimator.SetTrigger("Grab");
                currentTarget.OnClickDown(this);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentTarget.OnPressE(this);
            }
        }
    }

    void DrinkCoffee()
    {
        CurrentCupState = CupState.Empty;
        Debug.Log("Coffee consumed.");
        if (paranoiaManager != null) paranoiaManager.RefillStamina(50f);
    }

    public void SitInChair(Chair chair)
    {
        isSitting = true;

        Transform playerBody = transform.parent;

        playerBody.position = chair.sitPosition.position;
        playerBody.rotation = chair.sitPosition.rotation;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        PlayerLook look = GetComponent<PlayerLook>(); 
        look.LockOnPC(chair.sitPosition);
    }
    public void StandUp()
    {
        isSitting = false;
        PlayerLook look = GetComponentInChildren<PlayerLook>();
        look.UnlockFromPC();
    }
}