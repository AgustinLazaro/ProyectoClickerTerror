using UnityEngine;

public class AudioManagerMarian : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private SoundLibrarySO soundLibrary;

    [Header("Canales")]
    [SerializeField] private SFXEventChannelSO sfxChannel;
    [SerializeField] private MusicEventChannelSO musicChannel;
    [SerializeField] private UIEventChannelSO uiChannel;

    [Header("Fuentes por categoria")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSource;

    private void OnEnable()
    {
        // Programación defensiva: Solo nos suscribimos si el canal existe
        if (sfxChannel != null) sfxChannel.OnRaised += HandlePlaySFX;
        if (musicChannel != null) musicChannel.OnRaised += HandlePlayMusic;
        if (uiChannel != null) uiChannel.OnRaised += HandlePlayUI;
    }

    private void OnDisable()
    {
        // Solo nos desuscribimos si el canal existe
        if (sfxChannel != null) sfxChannel.OnRaised -= HandlePlaySFX;
        if (musicChannel != null) musicChannel.OnRaised -= HandlePlayMusic;
        if (uiChannel != null) uiChannel.OnRaised -= HandlePlayUI;
    }

    private void HandlePlaySFX(SoundID id) => PlayOneShot(sfxSource, id);

    private void HandlePlayMusic(SoundID id)
    {
        // La música sí suele loopear y cambiar el clip entero
        if (!soundLibrary.TryGetClip(id, out var clip) || clip == null) return;
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void HandlePlayUI(SoundID id)
    {
        // CORREGIDO: Ahora usa el uiSource y lo reproduce como OneShot (sin loop)
        PlayOneShot(uiSource, id);
    }

    private void PlayOneShot(AudioSource source, SoundID id)
    {
        if (soundLibrary.TryGetClip(id, out var clip) && clip != null)
            source.PlayOneShot(clip);
    }
}