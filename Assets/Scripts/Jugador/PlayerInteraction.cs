using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float reachDistance = 3f;

    void Update()
    {
        bool isClicking = Input.GetMouseButtonDown(0);

        if (isClicking == true)
        {
            CheckWhatWeTouched();
        }
    }

    void CheckWhatWeTouched()
    {
        float screenCenterX = Screen.width / 2;
        float screenCenterY = Screen.height / 2;
        Vector3 screenCenter = new Vector3(screenCenterX, screenCenterY, 0);

        Ray laserRay = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;

        bool didHitSomething = Physics.Raycast(laserRay, out hitInfo, reachDistance);

        if (didHitSomething == true)
        {
            
            CoffeeMaker foundCoffeeMaker = hitInfo.collider.GetComponent<CoffeeMaker>();

            
            bool isCoffeeMaker = (foundCoffeeMaker != null);

            if (isCoffeeMaker == true)
            {
                Debug.Log("  Refilling stamina...");
              
            }
            else
            {
                Debug.Log("Touched: " + hitInfo.collider.name + ". This object doesnt have the CoffeeMaker script.");
            }
        }
    }
}