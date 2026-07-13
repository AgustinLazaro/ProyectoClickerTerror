using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    [Header("PC Sitting Lerp")]
    public float pcLookLerpSpeed = 5f; 
    private float targetYaw = 0f;     
    private float currentYaw = 0f;     
    private Quaternion baseSitRotation;

    private float xRotation = 0f;
    public bool isLocked { get; private set; }

    void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        if (isLocked)
        {

            if (Input.GetKey(KeyCode.A))
            {
                targetYaw = -90f;
            }
         
            else if (Input.GetKey(KeyCode.D))
            {
                targetYaw = 0f;
            }

            currentYaw = Mathf.Lerp(currentYaw, targetYaw, pcLookLerpSpeed * Time.deltaTime);

            if (playerBody != null)
            {
                playerBody.rotation = baseSitRotation * Quaternion.Euler(0, currentYaw, 0);
            }
        }
        else
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }

    public void LockOnPC(Transform pcMonitorPosition)
    {
        isLocked = true;

        xRotation = 0f;
        currentYaw = 0f;
        targetYaw = 0f;
        baseSitRotation = pcMonitorPosition.rotation;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnlockFromPC()
    {
        isLocked = false;
        xRotation = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}