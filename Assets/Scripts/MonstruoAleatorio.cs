using UnityEngine;

public class MonstruoAleatorio : MonoBehaviour
{
    [Header("Posiciones de Ataque")]
    public Transform posVentana;
    public Transform posPuerta;
    public Transform posEspalda;
    public Transform posEscondido;

    [Header("Configuración de Tiempos")]
    public float tiempoMinAparicion = 5f;
    public float tiempoMaxAparicion = 10f;
    public float tiempoParaAtacar = 4f;
    public float tiempoParaEspantarlo = 0.5f;

    [Header("Mecánica de Engaño")]
    [Range(0, 100)]
    public float chanceSonidoFalso = 40f; 
    private bool yaHizoRuidoFalso = false;

    private float timerAparicion;
    private float timerAtaque;
    private float timerMirando = 0f;

    private int ubicacionActual = -1;
    private bool estaAcechando = false;

    void Start()
    {
        EsconderMonstruo();
    }

    void Update()
    {
        if (!estaAcechando)
        {
            timerAparicion -= Time.deltaTime;

           
            if (timerAparicion < 3f && !yaHizoRuidoFalso)
            {
                yaHizoRuidoFalso = true; 
                
                if (Random.Range(0, 100) < chanceSonidoFalso)
                {
                    Debug.LogWarning("¡SONIDO ENGAÑOSO! (Escuchás algo en la oscuridad, pero no hay nada...)");
                }
            }
            

            if (timerAparicion <= 0) AparecerAleatoriamente();
        }
        else
        {
            timerAtaque -= Time.deltaTime;


            Color colorLinea;

            if (JugadorLoEstaMirando())
            {
                colorLinea = Color.green;
            }
            else
            {
                colorLinea = Color.red;   
            }

            if (JugadorLoEstaMirando())
            {
                timerMirando += Time.deltaTime;
                if (timerMirando >= tiempoParaEspantarlo)
                {
                    Debug.Log("¡Lo espantaste! Volvió a las sombras.");
                    EsconderMonstruo();
                }
            }
            else
            {
                timerMirando = 0f;
                if (timerAtaque <= 0)
                {
                    Debug.LogError("¡JUMPSCARE! Te atrapó por la " + UbicacionString() + ".");
                    ControladorJuego controlador = FindAnyObjectByType<ControladorJuego>();
                    if (controlador != null) controlador.PerderJuego();
                    EsconderMonstruo();
                }
            }
        }
    }

    void AparecerAleatoriamente()
    {
        estaAcechando = true;
        timerAtaque = tiempoParaAtacar;
        timerMirando = 0f;

        ubicacionActual = Random.Range(0, 3);

        if (ubicacionActual == 0) transform.position = posVentana.position;
        else if (ubicacionActual == 1) transform.position = posPuerta.position;
        else if (ubicacionActual == 2) transform.position = posEspalda.position;

        
        Vector3 posicionCamara = Camera.main.transform.position;
        posicionCamara.y = transform.position.y;
        transform.LookAt(posicionCamara);

        Debug.LogWarning("¡RUIDO REAL! El monstruo apareció en: " + UbicacionString());
    }

    void EsconderMonstruo()
    {
        estaAcechando = false;
        yaHizoRuidoFalso = false; 
        ubicacionActual = -1;
        timerAparicion = Random.Range(tiempoMinAparicion, tiempoMaxAparicion);
        transform.position = posEscondido.position;
    }

    bool JugadorLoEstaMirando()
    {
        
        if (!estaAcechando) return false;

        if (ubicacionActual == 0 && Input.GetKey(KeyCode.A)) return true;
        if (ubicacionActual == 1 && Input.GetKey(KeyCode.D)) return true;
        if (ubicacionActual == 2 && Input.GetKey(KeyCode.S)) return true;
        return false;
    }

    string UbicacionString()
    {
        if (ubicacionActual == 0) return "Ventana (Tecla A)";
        if (ubicacionActual == 1) return "Puerta (Tecla D)";
        if (ubicacionActual == 2) return "Espalda (Tecla S)";
        return "Desconocido";
    }
}