using UnityEngine;
using System.Collections;

public class PCInteraction : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float zoomSpeed = 2f;
    

    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform zoomPoint;
    [SerializeField] private Canvas pcCanvas;
    [SerializeField] private PlayerMovementLazaro playerMovement; 
    [SerializeField] private MouseLook mouseLook;                 
   

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isPCActive = false;
    private bool inTransition = false;

    public bool IsPCActive => isPCActive;

    private void Update()
    {
        if (inTransition) return;
        if (isPCActive && Input.GetKeyDown(KeyCode.Escape))
            DeactivePC();
    }

    public void ActivePC()
    {
        if (isPCActive || inTransition) return;

        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;

        
        playerMovement.enabled = false;
        mouseLook.enabled = false;
        

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(ZoomIntoMonitor());
    }

    public void DeactivePC()
    {
        if (!isPCActive || inTransition) return;

        pcCanvas.gameObject.SetActive(false);
        StartCoroutine(ZoomFromMonitor());
    }

    private IEnumerator ZoomIntoMonitor()
    {
        inTransition = true;

        while (Vector3.Distance(playerCamera.position, zoomPoint.position) > 0.01f)
        {
            playerCamera.position = Vector3.Lerp(
                    playerCamera.position,
                    zoomPoint.position,
                    Time.deltaTime * zoomSpeed
                );
            playerCamera.rotation = Quaternion.Lerp(
                playerCamera.rotation,
                zoomPoint.rotation,
                Time.deltaTime * zoomSpeed
                );
            yield return null;
        }

        playerCamera.position = zoomPoint.position;
        playerCamera.rotation = zoomPoint.rotation;

        pcCanvas.gameObject.SetActive(true);
        isPCActive = true;
        inTransition = false;
    }

    private IEnumerator ZoomFromMonitor()
    {
        inTransition = true;
        isPCActive = false;

        while (Vector3.Distance(playerCamera.position, originalCameraPosition) > 0.01f)
        {
            playerCamera.position = Vector3.Lerp
                (
                    playerCamera.position,
                    originalCameraPosition,
                    Time.deltaTime * zoomSpeed
                );
            playerCamera.rotation = Quaternion.Lerp
                (
                playerCamera.rotation,
                originalCameraRotation,
                Time.deltaTime * zoomSpeed
                );
            yield return null;
        }

        playerCamera.position = originalCameraPosition;
        playerCamera.rotation = originalCameraRotation;

        
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inTransition = false;
    }
}