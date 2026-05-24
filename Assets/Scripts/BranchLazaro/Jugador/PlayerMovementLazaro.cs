using UnityEngine;

public class PlayerMovementLazaro : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float acceleration = 8f;
    public float deceleration = 10f;
    private Vector3 currentVelocity;

    [Header("Sonidos de Pasos")]
    public AudioSource audioSourcePasos;
    public AudioClip paso1;
    public AudioClip paso2;
    public float tiempoEntrePasos = 0.5f;
    private float timerPasos;
    private bool tocaPaso1 = true;

    [Header("Headbob (Cabeceo)")]
    public float bobSpeed = 10f;
    public float bobHeight = 0.05f;
    private float defaultCameraY;
    private float bobTimer = 0f;

    [Header("Camera Reference")]
    public Camera playerCamera; 

    void Start()
    {
        
        if (playerCamera != null)
        {
            defaultCameraY = playerCamera.transform.localPosition.y;
        }
        else
        {
            Debug.LogWarning("No asignaste la cámara en el PlayerMovementLazaro.");
        }

        // --- BLOQUEO DE CURSOR AGREGADO ACÁ ---
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Walk();
    }

    void Walk()
    {
        // 1. Obtener los inputs de WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 2. Calcular la dirección basada en hacia dónde mira el jugador
        // Usamos transform.right y transform.forward para que el movimiento sea relativo a la rotación
        Vector3 direction = (transform.right * moveX + transform.forward * moveZ).normalized;
        bool isPressingKeys = (moveX != 0 || moveZ != 0);

        if (isPressingKeys)
        {
            // --- CÓDIGO DE MOVIMIENTO ---
            Vector3 targetSpeed = direction * walkSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetSpeed, acceleration * Time.deltaTime);

            // --- HEADBOB ---
            if (playerCamera != null)
            {
                bobTimer = bobTimer + (Time.deltaTime * bobSpeed);
                float newCameraY = defaultCameraY + (Mathf.Sin(bobTimer) * bobHeight);

                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    newCameraY,
                    playerCamera.transform.localPosition.z
                );
            }

            // --- PASOS ---
            timerPasos -= Time.deltaTime;
            if (timerPasos <= 0f && audioSourcePasos != null)
            {
                if (tocaPaso1 && paso1 != null)
                {
                    audioSourcePasos.PlayOneShot(paso1);
                }
                else if (!tocaPaso1 && paso2 != null)
                {
                    audioSourcePasos.PlayOneShot(paso2);
                }

                tocaPaso1 = !tocaPaso1;
                timerPasos = tiempoEntrePasos;
            }
        }
        else
        {
            // --- DESACELERACIÓN (Jugador suelta las teclas) ---
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

            // Suavizar el retorno de la cámara al centro
            if (playerCamera != null)
            {
                bobTimer = 0f;
                float smoothReturnY = Mathf.Lerp(playerCamera.transform.localPosition.y, defaultCameraY, Time.deltaTime * 5f);

                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    smoothReturnY,
                    playerCamera.transform.localPosition.z
                );
            }

            timerPasos = 0f;
        }

        // 3. Aplicar el movimiento final al CharacterController o Transform
        // Usamos Space.World para que currentVelocity ya calcule la dirección correcta
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);
    }
}