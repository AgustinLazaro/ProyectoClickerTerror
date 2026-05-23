using UnityEngine;

public class ComportamientoAlucinacion : MonoBehaviour
{
    private Transform playerCamera;
    private ParanoiaManager paranoia;

    [Header("Mecánica: La Evitación")]
    public float tiempoParaDesaparecer = 4f; // Segundos que hay que darle la espalda
    public float penalidadPorMirar = 5f;     // Estamina que te roba por segundo si te quedás mirándolo

    private float timerIgnorando = 0f;

    void Start()
    {
        // Buscamos al jugador y al manager ni bien el monstruo aparece
        if (Camera.main != null) playerCamera = Camera.main.transform;
        paranoia = FindObjectOfType<ParanoiaManager>();

        // Respaldo por las dudas: si el jugador se queda de costado sin mirarlo ni darle 
        // la espalda completamente, se va a los 15 segundos para no romper el juego.
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        if (playerCamera == null || paranoia == null) return;

        // 1. Vector matemático desde la cámara hacia el monstruo
        Vector3 direccionAlEnemigo = (transform.position - playerCamera.position).normalized;

        // 2. Producto Punto (Dot Product): Compara hacia dónde mira el jugador vs dónde está el monstruo.
        // Da 1 si lo mirás de frente, 0 si está a 90 grados, y -1 si le das la espalda.
        float vision = Vector3.Dot(playerCamera.forward, direccionAlEnemigo);

        if (vision > 0.5f)
        {
            // LO ESTÁ MIRANDO (Está en su campo de visión frontal)
            // Reseteamos el timer de ignorar, porque volvió a mirarlo
            timerIgnorando = 0f;

            // Castigo por mirar: drena la estamina poco a poco
            paranoia.currentStamina -= penalidadPorMirar * Time.deltaTime;
        }
        else if (vision < -0.2f)
        {
            // LE ESTÁ DANDO LA ESPALDA (El monstruo quedó atrás)
            timerIgnorando += Time.deltaTime;

            if (timerIgnorando >= tiempoParaDesaparecer)
            {
                Debug.Log("👻 [ENEMIGO] Le diste la espalda lo suficiente. Desapareció en silencio.");

                // Acá a futuro podemos agregar un AudioSource.PlayClipAtPoint para que suene un 
                // crujido de madera a sus espaldas justo cuando el bicho se va.

                Destroy(gameObject);
            }
        }
    }
}
