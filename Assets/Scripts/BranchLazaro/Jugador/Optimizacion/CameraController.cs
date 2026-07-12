using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Headbob Settings")]
    public float bobSpeed = 10f;
    public float bobHeight = 0.05f;
    public float smoothReturnSpeed = 5f;

    [Header("FOV Paranoia (NUEVO)")]
    public PlayerParanoia playerParanoia;
    public float fovNormal = 60f;
    public float fovCritico = 110f;
    public float velocidadFov = 20f;
    private Camera cam;

    [Header("References")]
    public PlayerMovementLazaro playerMovement;

    private float defaultCameraY;
    private float bobTimer = 0f;
    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (playerMovement.isMoving) ApplyBob();
        else ResetBob();

        UpdateParanoiaFOV();
    }

    private void UpdateParanoiaFOV()
    {
        float fovObjetivo = (playerParanoia.ParanoiaPhase == 3) ? fovCritico : fovNormal;
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, fovObjetivo, velocidadFov * Time.deltaTime);
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
