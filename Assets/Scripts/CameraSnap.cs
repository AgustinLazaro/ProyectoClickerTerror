using UnityEngine;
public class CameraSnap : MonoBehaviour
{ 
    [Header("View Angles (Relative to Center)")]
    [SerializeField] private float windowAngle = -80f; 
    [SerializeField] private float doorAngle = 80f;    
    [SerializeField] private float backAngle = 180f;


    [Header("Configuration")]
    [SerializeField] float rotationSpeed = 15f;

    private float rotationY = 0f;
    private float centrerY;
    private Quaternion targetRotation;

    private void Start()
    {
        Vector3 rotacionInicial = transform.localRotation.eulerAngles;
        rotationY = rotacionInicial.y;

        if (rotationY > 180f) rotationY -= 360f;
       
        centrerY = rotationY;
        targetRotation = Quaternion.Euler(0f, centrerY, 0f);
    }

    private void Update()
    {
        DeterminarObjetivo();
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void DeterminarObjetivo()
    {
        if (Input.GetKey(KeyCode.S))
            targetRotation = Quaternion.Euler(0f, centrerY + backAngle, 0f);
        else if (Input.GetKey(KeyCode.D))
            targetRotation = Quaternion.Euler(0f, centrerY + doorAngle, 0f);
        else if (Input.GetKey(KeyCode.A))  
            targetRotation = Quaternion.Euler(0f, centrerY + windowAngle, 0f);
        else
            targetRotation = Quaternion.Euler(0f, centrerY, 0f);
    }
}