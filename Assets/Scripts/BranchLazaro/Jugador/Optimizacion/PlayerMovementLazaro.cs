using UnityEngine;

public class PlayerMovementLazaro : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float acceleration = 8f;
    public float deceleration = 10f;

    private Vector3 currentVelocity;
    private Rigidbody rb;

    public bool isMoving { get; private set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CalculateMovement();
    }

    void FixedUpdate()
    {
        ApplyPhysics();
    }

    void CalculateMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.right * moveX + transform.forward * moveZ).normalized;
        bool isPressingKeys = (moveX != 0 || moveZ != 0);

        isMoving = isPressingKeys;

        if (isPressingKeys)
        {
            Vector3 targetSpeed = direction * walkSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }
    }

    void ApplyPhysics()
    {
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }
}