using UnityEngine;
using System.Collections; 

public class PlayerMovementLazaro : MonoBehaviour
{
    [Header("States")]
    public bool isSitting = true;
    private bool isTransitioning = false; 
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float transitionTime = 0.5f; 

    [Header("Movement Feel (Weight)")]
    public float acceleration = 8f;
    public float deceleration = 10f;
    private Vector3 currentVelocity; 

    [Header("Camera Scripts")]
    public CameraSnap cameraSnapScript;
    public MouseLook mouseLookScript;

    void Start()
    {
        // Estado inicial del juego
        isSitting = true;
        cameraSnapScript.enabled = true;
        mouseLookScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && isTransitioning == false)
        {
            
            StartCoroutine(SmoothCameraTransition());
        }

        
        if (isSitting == false && isTransitioning == false)
        {
            Walk();
        }
    }

    
    IEnumerator SmoothCameraTransition()
    {
        isTransitioning = true; 
        isSitting = !isSitting; 

        
        cameraSnapScript.enabled = false;
        mouseLookScript.enabled = false;

        Quaternion startingRotation = Camera.main.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float timer = 0f; 

    
        while (timer < transitionTime)
        {
            timer = timer + Time.deltaTime; 
            float percentage = timer / transitionTime; 

          
            Camera.main.transform.localRotation = Quaternion.Lerp(startingRotation, targetRotation, percentage);

            yield return null; 
        }

       
        if (isSitting == true)
        {
            cameraSnapScript.enabled = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            mouseLookScript.enabled = true;
            Cursor.lockState = CursorLockMode.Locked; 
        }

        isTransitioning = false; 
    }

    void Walk()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        
        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;

      
        bool isPressingKeys = (moveX != 0 || moveZ != 0);

        if (isPressingKeys)
        {
          
            Vector3 targetSpeed = direction * walkSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

       
        transform.Translate(currentVelocity * Time.deltaTime);
    }
}