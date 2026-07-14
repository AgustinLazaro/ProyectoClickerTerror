using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerMovementMarian playerMovement;
    [SerializeField] private PlayerLookMarian playerLook;
    [SerializeField] private PlayerInteractionMarian playerInteraction;
    [SerializeField] private PlayerParanoiaMarian playerParanoia;

    private void Update()
    {
        if (!playerInteraction.IsSitting)
            HandleMovementInput();

        HandleLookInput();
        HandleInteractionInput();
        HandleParanoiaInput();
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        playerMovement.SetMovementInput(moveX, moveZ);
    }

    private void HandleLookInput()
    {
        if (playerLook.IsLocked)
        {
            bool pressA = Input.GetKey(KeyCode.A);
            bool pressD = Input.GetKey(KeyCode.D);
            playerLook.SetSitInput(pressA, pressD);
        }
        else
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            playerLook.SetLookInput(mouseX, mouseY);
        }
    }

    private void HandleInteractionInput()
    {
        if (playerInteraction.IsSitting && Input.GetKeyDown(KeyCode.E))
        {
            playerInteraction.StandUp();
            return;
        }

        if (Input.GetMouseButtonDown(1))
            playerInteraction.TryDrinkCoffee();

        if (Input.GetMouseButton(0))
            playerInteraction.OnHoldInput();

        if (Input.GetMouseButtonDown(0))
            playerInteraction.OnClickInput();

        if (Input.GetKeyDown(KeyCode.E))
            playerInteraction.OnEInput();
    }

    private void HandleParanoiaInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            playerParanoia.TryBlink();
    }
}
