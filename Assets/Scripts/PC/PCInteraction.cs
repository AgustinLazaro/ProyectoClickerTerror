using UnityEngine;

public class PCInteraction : MonoBehaviour
{
    [Header("Configurationn")]
    [SerializeField] private float distanceMax = 3f;
    [SerializeField] private Canvas pcCanvas;
    [SerializeField] private Transform player;

    private bool isPCActive = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryInteraction();
    }

    private void TryInteraction()
    {
        // verifica distance
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > distanceMax) return;

        // lanza raycast desde el centro de la pantalla
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, distanceMax))
        {
            if (hit.collider.gameObject == gameObject)
                PCActive();
        }
    }

    private void PCActive()
    {
        isPCActive = !isPCActive;
        pcCanvas.gameObject.SetActive(isPCActive);

        // bloquea o libera el cursor
        Cursor.lockState = isPCActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPCActive;
    }
}
