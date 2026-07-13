using UnityEngine;

public class PlayerMovementMarian : MonoBehaviour
{
    [SerializeField] private PlayerInteractionMarian playerInteraction;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float acceleration = 8f;
    [SerializeField] float deceleration = 10f;

    private Vector3 _currentVelocity;
    private Rigidbody _rb;

    private float _inputX;
    private float _inputZ;

    public bool IsMoving { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
    }

    // Método público para recibir input desde PlayerInputHandler
    public void SetMovementInput(float x, float z)
    {
        _inputX = x;
        _inputZ = z;
    }

    private void CalculateMovement()
    {
        if (playerInteraction.IsSitting)
        {
            _currentVelocity = Vector3.zero;
            IsMoving = false;
            return;
        }

        Vector3 direction = (transform.right * _inputX + transform.forward * _inputZ).normalized;
        bool isPressingKeys = _inputX != 0 || _inputZ != 0;
        IsMoving = isPressingKeys;

        if (isPressingKeys)
        {
            Vector3 targetSpeed = direction * walkSpeed;
            _currentVelocity = Vector3.Lerp(_currentVelocity, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }
    }

    private void ApplyPhysics()
    {
        _rb.linearVelocity = new Vector3(_currentVelocity.x, _rb.linearVelocity.y, _currentVelocity.z);
    }
}
