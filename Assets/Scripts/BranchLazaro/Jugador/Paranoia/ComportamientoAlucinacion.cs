using UnityEngine;

public class ComportamientoAlucinacion : MonoBehaviour
{
    private Transform playerCamera;
    [SerializeField] private PlayerParanoia paranoia;

    [Header("Mecánica: La Evitación")]
    public float tiempoParaDesaparecer = 4f; 
    public float penalidadPorMirar = 5f;     

    private float timerIgnorando = 0f;

    void Start()
    {
       
        if (Camera.main != null) playerCamera = Camera.main.transform;
        paranoia = FindObjectOfType<PlayerParanoia>();

       
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        if (playerCamera == null || paranoia == null) return;

       
        Vector3 direccionAlEnemigo = (transform.position - playerCamera.position).normalized;

        
        float vision = Vector3.Dot(playerCamera.forward, direccionAlEnemigo);

        if (vision > 0.5f)
        {
            
            timerIgnorando = 0f;

            
            paranoia.currentStamina -= penalidadPorMirar * Time.deltaTime;
        }
        else if (vision < -0.2f)
        {
            
            timerIgnorando += Time.deltaTime;

            if (timerIgnorando >= tiempoParaDesaparecer)
            {
                

                

                Destroy(gameObject);
            }
        }
    }
}
