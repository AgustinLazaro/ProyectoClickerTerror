using UnityEngine;

public class PlayerLookMarian : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private Transform playerBody;

    [Header("PC Sitting Lerp")]
    [SerializeField] private float pcLookLerpSpeed = 5f;
    private float _targetYaw = 0f;
    private float _currentYaw = 0f;
    private Quaternion _baseSitRotation;

    private float _xRotation = 0f;
    public bool IsLocked { get; private set; }

    private void Update()
    {
        // Ya no llama a Input, solo aplica lo que se haya recibido
        if (IsLocked)
            ApplySitLook();
        else
            ApplyMouseLook();

    }

    // Método público para recibir input de movimiento del mouse
    public void SetLookInput(float mouseX, float mouseY)
    {
        _xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX * mouseSensitivity * Time.deltaTime);
    }

    // Método público para recibir input cuando está bloqueado en la PC
    public void SetSitInput(bool pressA, bool pressD)
    {
        if (pressA) _targetYaw = -90f;
        else if (pressD) _targetYaw = 0f;
        _currentYaw = Mathf.Lerp(_currentYaw, _targetYaw, pcLookLerpSpeed * Time.deltaTime);
        if (playerBody != null)
            playerBody.rotation = _baseSitRotation * Quaternion.Euler(0, _currentYaw, 0);
    }

    // Métodos internos que aplican la lógica según el estado
    // Aquí no hay Input, solo se usa lo que se haya pasado por SetLookInput
    // El cálculo ya se hace en ambos metodos
    private void ApplyMouseLook() { }
    private void ApplySitLook() { }

    public void LockOnPC(Transform pcMonitorPosition)
    {
        IsLocked = true;
        _xRotation = 0f;
        _currentYaw = 0f;
        _targetYaw = 0f;
        _baseSitRotation = pcMonitorPosition.rotation;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void UnlockFromPC()
    {
        IsLocked = false;
        _xRotation = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
