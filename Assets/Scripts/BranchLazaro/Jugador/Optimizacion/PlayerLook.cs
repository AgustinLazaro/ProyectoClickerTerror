using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    private float xRotation = 0f;

    // Agregamos un estado para saber si estamos sentados en la PC
    public bool isLocked { get; private set; }

    void Update()
    {
        // Si estamos interactuando con la PC, cortamos la ejecución para no mover el mouse
        if (isLocked) return;

        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Movemos el cuello (Cámara)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Movemos el cuerpo (Jugador)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    // --- MÉTODOS PARA INTERACTUAR CON EL ENTORNO (SOLID) ---

    public void LockOnPC(Transform pcMonitorPosition)
    {
        isLocked = true;

        // Opcional: Centrar la cámara apuntando al monitor al sentarse
        // transform.LookAt(pcMonitorPosition); 
    }

    public void UnlockFromPC()
    {
        isLocked = false;
    }
}