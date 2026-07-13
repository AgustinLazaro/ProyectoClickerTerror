using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 

public class ParanoiaVFX : MonoBehaviour
{
    [Header("Conexión con el Cerebro")]
    public PlayerParanoia playerParanoia;

    [Header("Efectos Visuales (Volumes)")]
    public PostProcessVolume volumeFase1;
    public PostProcessVolume volumeFase2;
    public PostProcessVolume volumeFase3;

    public float speedTransition = 1.5f;

    private void Start()
    {
        if (volumeFase1 != null) volumeFase1.weight = 0f;
        if (volumeFase2 != null) volumeFase2.weight = 0f;
        if (volumeFase3 != null) volumeFase3.weight = 0f;
    }

    private void Update()
    {
        if (playerParanoia == null) return;

        int phase = playerParanoia.ParanoiaPhase;

        float targetFase1 = (phase == 1) ? 1f : 0f;
        float targetFase2 = (phase == 2) ? 1f : 0f;
        float targetFase3 = (phase == 3) ? 1f : 0f;

       
        if (volumeFase1 != null)
            volumeFase1.weight = Mathf.MoveTowards(volumeFase1.weight, targetFase1, speedTransition * Time.deltaTime);

        if (volumeFase2 != null)
            volumeFase2.weight = Mathf.MoveTowards(volumeFase2.weight, targetFase2, speedTransition * Time.deltaTime);

        if (volumeFase3 != null)
            volumeFase3.weight = Mathf.MoveTowards(volumeFase3.weight, targetFase3, speedTransition * Time.deltaTime);
    }
}