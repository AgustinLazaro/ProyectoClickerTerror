using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerParanoia paranoia;
    public AudioSource headAudioSource;

    [Header("UI de Derrota")]
    public GameObject pantallaDerrota;

    [Header("Umbrales de Fase")]
    public float umbralFase1 = 60f;
    public float umbralFase2 = 30f;
    public float umbralFase3 = 10f;

    [Header("Ataque Fase 1 y 2: Susurros")]
    public AudioClip[] spookySounds;
    public float minAudioInterval = 5f;
    public float maxAudioInterval = 12f;
    [Range(0f, 100f)] public float probabilidadSusurro = 40f;
    private float audioTimer;

    [Header("Ataque Fase 2: Apariciones")]
    public GameObject enemyScarePrefab;
    public Transform[] scareSpawnPoints;
    public float minScareInterval = 15f;
    public float maxScareInterval = 30f;
    private float scareTimer;

    [Header("Ataque Fase 3: Muerte Inminente")]
    public AudioClip sonidoFase3;
    public float tiempoParaMorir = 15f;
    private float deathTimer;
    private bool inPhase3 = false;
    private bool atacando = false;

    void Start()
    {
        audioTimer = Random.Range(minAudioInterval, maxAudioInterval);
        scareTimer = Random.Range(minScareInterval, maxScareInterval);
        deathTimer = tiempoParaMorir;
    }

    void Update()
    {
        if (paranoia == null || atacando) return;

        float estamina = paranoia.CurrentStamina;

        if (estamina >= umbralFase1)
        {
            ResetPhase3();
        }
        else if (estamina >= umbralFase2 && estamina < umbralFase1)
        {
            ResetPhase3();
            HandleAudioScares(1f);
        }
        else if (estamina >= umbralFase3 && estamina < umbralFase2)
        {
            ResetPhase3();
            HandleAudioScares(1f);
            HandleVisualScares();
        }
        else if (estamina < umbralFase3)
        {
            if (!inPhase3)
            {
                inPhase3 = true;

                if (headAudioSource != null && sonidoFase3 != null)
                {
                    headAudioSource.Stop();
                    headAudioSource.panStereo = 0f;
                    headAudioSource.PlayOneShot(sonidoFase3);
                }
            }

            HandleAudioScares(0.4f);
            HandleDeathTimer();
        }
    }

    private void HandleAudioScares(float speedMultiplier)
    {
        audioTimer -= Time.deltaTime;

        if (audioTimer <= 0f)
        {
            float dado = Random.Range(0f, 100f);

            if (dado <= probabilidadSusurro)
            {
                if (spookySounds != null && spookySounds.Length > 0 && headAudioSource != null)
                {
                    if (!headAudioSource.isPlaying || headAudioSource.clip != sonidoFase3)
                    {
                        AudioClip clip = spookySounds[Random.Range(0, spookySounds.Length)];
                        headAudioSource.panStereo = Random.Range(-0.8f, 0.8f);
                        StartCoroutine(SuavizarAudio(clip));
                    }
                }
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
                Instantiate(enemyScarePrefab, spawnPt.position, spawnPt.rotation);
            }
            scareTimer = Random.Range(minScareInterval, maxScareInterval);
        }
    }

    private void HandleDeathTimer()
    {
        deathTimer -= Time.deltaTime;

        if (deathTimer <= 0f && !atacando)
        {
            atacando = true;
            StartCoroutine(AtaqueFinal());
        }
    }

    private void ResetPhase3()
    {
        inPhase3 = false;
        atacando = false;
        deathTimer = tiempoParaMorir;
    }

    private System.Collections.IEnumerator SuavizarAudio(AudioClip clip)
    {
        headAudioSource.clip = clip;
        headAudioSource.volume = 0f;
        headAudioSource.Play();

        float tiempoFade = 2f;
        float timer = 0f;

        while (timer < tiempoFade)
        {
            timer += Time.deltaTime;
            headAudioSource.volume = Mathf.Lerp(0f, 1f, timer / tiempoFade);
            yield return null;
        }

        float tiempoRestante = clip.length - (tiempoFade * 2);
        if (tiempoRestante > 0)
        {
            yield return new WaitForSeconds(tiempoRestante);
        }

        timer = 0f;
        while (timer < tiempoFade)
        {
            timer += Time.deltaTime;
            headAudioSource.volume = Mathf.Lerp(1f, 0f, timer / tiempoFade);
            yield return null;
        }

        headAudioSource.Stop();
        headAudioSource.volume = 1f;
    }

    private System.Collections.IEnumerator AtaqueFinal()
    {
        Transform playerCam = Camera.main.transform;
        Vector3 spawnPos = playerCam.position + playerCam.forward * 5f;
        spawnPos.y = playerCam.position.y - 1.2f;

        GameObject monstruoFinal = Instantiate(enemyScarePrefab, spawnPos, Quaternion.LookRotation(-playerCam.forward));

        MonoBehaviour scriptAlucinacion = (MonoBehaviour)monstruoFinal.GetComponent("ComportamientoAlucinacion");
        if (scriptAlucinacion != null)
        {
            Destroy(scriptAlucinacion);
        }

        Animator anim = monstruoFinal.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetBool("Caminando", true);
        }

        float tiempoAtaque = 1.2f;
        float timer = 0f;
        Vector3 posInicial = monstruoFinal.transform.position;

        while (timer < tiempoAtaque)
        {
            timer += Time.deltaTime;
            Vector3 targetPos = playerCam.position - new Vector3(0, 0.4f, 0);
            monstruoFinal.transform.position = Vector3.Lerp(posInicial, targetPos, timer / tiempoAtaque);
            monstruoFinal.transform.LookAt(playerCam);
            yield return null;
        }

        if (pantallaDerrota != null)
        {
            pantallaDerrota.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}