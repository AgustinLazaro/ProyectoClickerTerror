using UnityEngine;
using System.Collections;

public class ParanoiaSFX : MonoBehaviour
{
    [Header("Brain Connection")]
    public PlayerParanoia playerParanoia;

    [Header("Event System (For Phase 3)")]
    public SFXEventChannelSO sfxChannel;

    [Header("Ambient Audio (Phases 1 & 2)")]
    public AudioSource ambientSource;
    public AudioClip audioPhase1;
    public AudioClip audioPhase2;

    [Header("Randomness Settings")]
    public Vector2 silenceTimeRange = new Vector2(3f, 8f);
    public Vector2 playingTimeRange = new Vector2(4f, 10f);
    public float fadeSpeed = 1f;

    private int lastPhase = 0;

    private void Start()
    {
        if (ambientSource != null) ambientSource.volume = 0f;
        StartCoroutine(ParanoiaAudioRoutine());
    }

    private IEnumerator ParanoiaAudioRoutine()
    {
        while (true)
        {
            int currentPhase = playerParanoia.ParanoiaPhase;

            if (currentPhase != lastPhase)
            {
              
                if (currentPhase == 1 && lastPhase < 3)
                {
                    if (sfxChannel != null) sfxChannel.Raise(SoundID.StaminaLow);
                }
                lastPhase = currentPhase;
            }

            
            if (currentPhase == 1 || currentPhase == 2)
            {
                AudioClip targetClip = (currentPhase == 1) ? audioPhase1 : audioPhase2;

                if (ambientSource.clip != targetClip)
                {
                    ambientSource.clip = targetClip;
                    ambientSource.Play();
                }
                yield return StartCoroutine(DoFade(1f));

                float playTime = Random.Range(playingTimeRange.x, playingTimeRange.y);
                yield return new WaitForSeconds(playTime);

              
                yield return StartCoroutine(DoFade(0f));
            }
            else if (currentPhase == 0) 
            {
                if (ambientSource.volume > 0)
                {
                    yield return StartCoroutine(DoFade(0f));
                }
            }
            float waitTime = Random.Range(silenceTimeRange.x, silenceTimeRange.y);
            yield return new WaitForSeconds(waitTime);
        }
    }
    private IEnumerator DoFade(float targetVolume)
    {
        while (Mathf.Abs(ambientSource.volume - targetVolume) > 0.01f)
        {
            ambientSource.volume = Mathf.MoveTowards(ambientSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        ambientSource.volume = targetVolume;
    }
}
