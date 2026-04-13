using UnityEngine;

public class RandomMonster : MonoBehaviour
{
    [Header("Atack Positions")]
    [SerializeField] private Transform windowPos;
    [SerializeField] private Transform doorPos;
    [SerializeField] private Transform backPos;
    [SerializeField] private Transform hiddenPos;

    [Header("Timing Configuration")]
    [SerializeField] private float minAppearanceTime = 5f;
    [SerializeField] private float maxAppearanceTime = 10f;
    [SerializeField] private float attackTime = 4f;
    [SerializeField] private float ScareAwayTime = 0.5f;

    [Header("Mecánica de Engaño")]
    [Range(0, 100)]
    public float fakeSoundChance = 40f; 
    private bool hasMadeFakeSound = false;

    private float appearanceTimer;
    private float attackTimer;
    private float lookingTimer = 0f;

    private int currentLocation = -1;
    private bool isLurking = false;

    private void Start()
    {
        HideMonster();
    }

    private void Update()
    {
        if (!isLurking)
        {
            appearanceTimer -= Time.deltaTime;

           
            if (appearanceTimer < 3f && !hasMadeFakeSound)
            {
                hasMadeFakeSound = true; 
                
                if (Random.Range(0, 100) < fakeSoundChance)
                {
                    Debug.LogWarning("¡SONIDO ENGAÑOSO! (Escuchás algo en la oscuridad, pero no hay nada...)");
                }
            }
            

            if (appearanceTimer <= 0) AppearRandomly();
        }
        else
        {
            attackTimer -= Time.deltaTime;


            Color colorLinea;

            if (PlayerIsLooking())
            {
                colorLinea = Color.green;
            }
            else
            {
                colorLinea = Color.red;   
            }

            if (PlayerIsLooking())
            {
                lookingTimer += Time.deltaTime;
                if (lookingTimer >= ScareAwayTime)
                {
                    Debug.Log("¡Lo espantaste! Volvió a las sombras.");
                    HideMonster();
                }
            }
            else
            {
                lookingTimer = 0f;
                if (attackTimer <= 0)
                {
                    Debug.LogError("¡JUMPSCARE! Te atrapó por la " + LocationString() + ".");
                    GameManager controlador = FindAnyObjectByType<GameManager>();
                    if (controlador != null) controlador.PerderJuego();
                    HideMonster();
                }
            }
        }
    }

    private void AppearRandomly()
    {
        isLurking = true;
        attackTimer = attackTime;
        lookingTimer = 0f;

        currentLocation = Random.Range(0, 3);

        if (currentLocation == 0) transform.position = windowPos.position;
        else if (currentLocation == 1) transform.position = doorPos.position;
        else if (currentLocation == 2) transform.position = backPos.position;

        
        Vector3 posicionCamara = Camera.main.transform.position;
        posicionCamara.y = transform.position.y;
        transform.LookAt(posicionCamara);

        Debug.LogWarning("¡RUIDO REAL! El monstruo apareció en: " + LocationString());
    }

    private void HideMonster()
    {
        isLurking = false;
        hasMadeFakeSound = false; 
        currentLocation = -1;
        appearanceTimer = Random.Range(minAppearanceTime, maxAppearanceTime);
        transform.position = hiddenPos.position;
    }

    private bool PlayerIsLooking()
    {
        
        if (!isLurking) return false;

        if (currentLocation == 0 && Input.GetKey(KeyCode.A)) return true;
        if (currentLocation == 1 && Input.GetKey(KeyCode.D)) return true;
        if (currentLocation == 2 && Input.GetKey(KeyCode.S)) return true;
        return false;
    }

    private string LocationString()
    {
        if (currentLocation == 0) return "Ventana (Tecla A)";
        if (currentLocation == 1) return "Puerta (Tecla D)";
        if (currentLocation == 2) return "Espalda (Tecla S)";
        return "Desconocido";
    }
}