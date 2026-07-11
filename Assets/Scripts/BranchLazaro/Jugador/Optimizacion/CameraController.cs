using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Headbob Settings")]
    public float bobSpeed = 10f;
    public float bobHeight = 0.05f;
    public float smoothReturnSpeed = 5f;

    [Header("References")]
    [Tooltip("Arrastrá al jugador acá para leer si se está moviendo")]
    public PlayerMovementLazaro playerMovement;

    private float defaultCameraY;
    private float bobTimer = 0f;

    void Start()
    {
        defaultCameraY = transform.localPosition.y;
    }

    void Update()
    {
        if (playerMovement == null) return;
       
        if (playerMovement.isMoving)
        {
            ApplyBob();
        }
        else
        {
            ResetBob();
        }
    }

    private void ApplyBob()
    {
        bobTimer += Time.deltaTime * bobSpeed;
        float newCameraY = defaultCameraY + (Mathf.Sin(bobTimer) * bobHeight);

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            newCameraY,
            transform.localPosition.z
        );
    }

    private void ResetBob()
    {
        bobTimer = 0f;
        float smoothReturnY = Mathf.Lerp(transform.localPosition.y, defaultCameraY, Time.deltaTime * smoothReturnSpeed);

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            smoothReturnY,
            transform.localPosition.z
        );
    }
}
