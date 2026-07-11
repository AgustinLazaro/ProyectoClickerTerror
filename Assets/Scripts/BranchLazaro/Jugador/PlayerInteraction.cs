using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayDistance = 3f;

    [Header("References")]
    public Camera playerCamera;
    public Animator armsAnimator;

    [Header("Inventory")]
    public bool hasCoffeePot = false;
    public bool hasFullCup = false;
    public bool hasEmptyCup = false;

    [Header("Managers")]
    public PlayerParanoia paranoiaManager;

    void Start()
    {
        if (paranoiaManager == null)
        {
            paranoiaManager = Object.FindFirstObjectByType<PlayerParanoia>();
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            RaycastHit continuousHit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out continuousHit, rayDistance))
            {
                CoffeeCupLazaro cup = continuousHit.collider.GetComponent<CoffeeCupLazaro>();
                if (cup != null)
                {
                    cup.TryFill(this);
                }
            }
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            if (armsAnimator != null) armsAnimator.SetTrigger("Grab");

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance))
            {
               
                CoffeeMaker coffeeMaker = hit.collider.GetComponent<CoffeeMaker>();
                if (coffeeMaker != null) coffeeMaker.Interact(this);

                
                CoffeeCupLazaro cup = hit.collider.GetComponent<CoffeeCupLazaro>();
                if (cup != null)
                {
                    if (cup.isOnTable && cup.isFull)
                    {
                        cup.GrabCup(this);
                    }
                    else if (!cup.isOnTable && hasEmptyCup)
                    {
                        cup.PlaceCup(this);
                    }
                }

                BreakerSwitch breaker = hit.collider.GetComponent<BreakerSwitch>();
                if (breaker != null) breaker.Interact();
            }
        }

        
        if (Input.GetMouseButtonDown(1))
        {
            if (hasFullCup)
            {
                DrinkCoffee();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance))
            {
                ComputerInteraction pc = hit.collider.GetComponent<ComputerInteraction>();
                if (pc != null) pc.UseComputer();
            }
        }
    }

    void DrinkCoffee()
    {
        hasFullCup = false;
        hasEmptyCup = true;
        Debug.Log("GLUP GLUP! Coffee consumed. Place the empty cup back on the table.");

        if (paranoiaManager != null)
        {
            paranoiaManager.RefillStamina(30f);
        }
    }
}