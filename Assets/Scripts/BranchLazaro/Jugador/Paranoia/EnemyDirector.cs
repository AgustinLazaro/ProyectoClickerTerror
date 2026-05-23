using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [Header("Referencias")]
    public ParanoiaManager paranoiaManager;
    public AudioSource headAudioSource;

    [Header("Umbrales de Fase (Sincronizados)")]
    // Estos valores ahora coinciden EXACTAMENTE con tu ParanoiaManager
    public float umbralFase1 = 60f;
    public float umbralFase2 = 30f;
    public float umbralFase3 = 10f;

    [Header("Ataque Fase 1: Sonidos")]
    public AudioClip[] spookySounds;
    public float minAudioInterval = 10f;
    public float maxAudioInterval = 25f;
    private float audioTimer;

    [Header("Ataque Fase 2: Apariciones Aleatorias")]
    public GameObject enemyScarePrefab;
    public Transform[] scareSpawnPoints;
    public float minScareInterval = 15f;
    public float maxScareInterval = 30f;
    private float scareTimer;

    [Header("Ataque Fase 3: Muerte Inminente")]
    public float tiempoParaMorir = 15f;
    private float deathTimer;
    private bool inPhase3 = false;

    void Start()
    {
        audioTimer = Random.Range(minAudioInterval, maxAudioInterval);
        scareTimer = Random.Range(minScareInterval, maxScareInterval);
        deathTimer = tiempoParaMorir;

        Debug.Log("[DIRECTOR] Sistema iniciado. Esperando a que la estamina baje de 60...");
    }

    void Update()
    {
        if (paranoiaManager == null) return;

        // Leemos la estamina cruda directo de tu script
        float estamina = paranoiaManager.currentStamina;

        // --- MÁQUINA DE ESTADOS SINCRONIZADA ---

        if (estamina >= umbralFase1)
        {
            // FASE 0: Normal (De 60 a 100) - No hay ataques
            ResetPhase3();
        }
        else if (estamina >= umbralFase2 && estamina < umbralFase1)
        {
            // FASE 1: Ansiedad (De 30 a 59) - Solo sonidos
            ResetPhase3();
            HandleAudioScares(1f);
        }
        else if (estamina >= umbralFase3 && estamina < umbralFase2)
        {
            // FASE 2: Paranoia (De 10 a 29) - Sonidos + Sustos visuales
            ResetPhase3();
            HandleAudioScares(1f);
            HandleVisualScares();
        }
        else if (estamina < umbralFase3)
        {
            // FASE 3: Crítico (Menor a 10) - Sonidos acelerados + Muerte
            if (!inPhase3)
            {
                inPhase3 = true;
                Debug.Log("⚠️ [ALERTA DIRECTOR] ¡FASE 3 ACTIVADA! Tenés " + tiempoParaMorir + " segundos antes del ataque final.");
            }
            HandleAudioScares(0.4f); // Los sonidos se reproducen más rápido
            HandleDeathTimer();
        }
    }

    // --- LÓGICA DE ATAQUES ---
    private void HandleAudioScares(float speedMultiplier)
    {
        audioTimer -= Time.deltaTime;

        if (audioTimer <= 0f)
        {
            Debug.Log($"🔊 [DIRECTOR] Reproduciendo sonido espeluznante. (Multiplicador de velocidad: {speedMultiplier}x)");

            if (spookySounds != null && spookySounds.Length > 0 && headAudioSource != null)
            {
                AudioClip clip = spookySounds[Random.Range(0, spookySounds.Length)];
                headAudioSource.PlayOneShot(clip);
            }

            audioTimer = Random.Range(minAudioInterval, maxAudioInterval) * speedMultiplier;
        }
    }

    private void HandleVisualScares()
    {
        scareTimer -= Time.deltaTime;

        if (scareTimer <= 0f)
        {
            if (enemyScarePrefab != null && scareSpawnPoints != null && scareSpawnPoints.Length > 0)
            {
                Transform spawnPt = scareSpawnPoints[Random.Range(0, scareSpawnPoints.Length)];

                Debug.Log($"👁️ [DIRECTOR] Sustazo visual instanciado en: {spawnPt.name}");

                // Solo lo instanciamos. El monstruo ahora decide cuándo irse.
                Instantiate(enemyScarePrefab, spawnPt.position, spawnPt.rotation);
            }
            scareTimer = Random.Range(minScareInterval, maxScareInterval);
        }
    }

    private void HandleDeathTimer()
    {
        deathTimer -= Time.deltaTime;

        if (deathTimer <= 0f)
        {
            Debug.Log("💀 [DIRECTOR] GAME OVER: Se acabó el tiempo en Fase 3. ¡El monstruo te atrapó!");
            deathTimer = tiempoParaMorir; // Lo reseteamos para que no colapse la consola repitiendo el mensaje
        }
    }

    private void ResetPhase3()
    {
        if (inPhase3)
        {
            Debug.Log("☕ [DIRECTOR] Estamina recuperada. Cancelando ataque final.");
        }

        inPhase3 = false;
        deathTimer = tiempoParaMorir;
    }
}