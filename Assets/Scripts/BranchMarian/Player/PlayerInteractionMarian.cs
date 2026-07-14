using UnityEngine;

public class PlayerInteractionMarian : MonoBehaviour
{
    [Header("Chair")]
    public bool IsSitting = false;
    private Chair _currentChair;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 3f;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator armsAnimator;

    [Header("Data")]
    [SerializeField] private InventoryDataSO inventoryData;

    [Header("Managers")]
    [SerializeField] private PlayerParanoiaMarian paranoiaManager;

    private InteractableBaseMarian _previousTarget;
    private InteractableBaseMarian _currentTarget;

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

    private void Start()
    {
        paranoiaManager = Object.FindFirstObjectByType<PlayerParanoiaMarian>();
        inventoryData.hasCoffeePot = false;
        inventoryData.currentCupState = CupState.None;
    }

    private void Update()
    {
        DetectTarget();
        HandleHover();
        // Ya no hay Input aquí, solo lógica de raycast y hover
    }

    // --- Métodos públicos para recibir input desde PlayerInputHandler ---
    public void TryDrinkCoffee()
    {
        if (CurrentCupState == CupState.Full)
            DrinkCoffee();
    }

    public void OnHoldInput()
    {
        if (_currentTarget != null)
        {
            Debug.Log($"Holding: {_currentTarget.name}");
            _currentTarget.OnClickHold(this);
        }
    }

    public void OnClickInput()
    {
        if (_currentTarget != null)
        {
            Debug.Log($"Clicked: {_currentTarget.name}");
            if (armsAnimator != null) armsAnimator.SetTrigger("Grab");
            _currentTarget.OnClickDown(this);
        }
    }

    public void OnEInput()
    {
        if (_currentTarget != null)
            _currentTarget.OnPressE(this);
    }

    // --- Lógica interna ---
    private void DetectTarget()
    {
        _currentTarget = null;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, rayDistance))
        {
            _currentTarget = hit.collider.GetComponent<InteractableBaseMarian>();
        }
    }

    private void HandleHover()
    {
        if (_currentTarget != _previousTarget)
        {
            if (_previousTarget != null) _previousTarget.OnHoverExit();
            if (_currentTarget != null) _currentTarget.OnHoverEnter();
            _previousTarget = _currentTarget;
        }
    }

    private void DrinkCoffee()
    {
        CurrentCupState = CupState.Empty;
        Debug.Log("Coffee consumed.");
        if (paranoiaManager != null) paranoiaManager.RefillStamina(30f);
    }

    public void SitInChair(Chair chair)
    {
        IsSitting = true;

        Transform playerBody = transform.parent;

        playerBody.position = chair.sitPosition.position;
        playerBody.rotation = chair.sitPosition.rotation;
        transform.localRotation = Quaternion.Euler(0, 0, 0);

        PlayerLook look = GetComponent<PlayerLook>();
        if (look != null)
        {
            look.LockOnPC(chair.sitPosition);
        }
    }
    public void StandUp()
    {
        IsSitting = false;
        PlayerLook look = GetComponentInChildren<PlayerLook>();
        look.UnlockFromPC();
    }
}
