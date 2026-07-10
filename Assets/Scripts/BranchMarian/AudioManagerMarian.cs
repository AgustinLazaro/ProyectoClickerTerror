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
        sfxChannel.OnRaised += HandlePlaySFX;
        musicChannel.OnRaised += HandlePlayMusic;
        uiChannel.OnRaised += HandlePlayUI;
    }

    private void OnDisable()
    {
        sfxChannel.OnRaised -= HandlePlaySFX;
        musicChannel.OnRaised -= HandlePlayMusic;
        uiChannel.OnRaised -= HandlePlayUI;
    }

    private void HandlePlaySFX(SoundID id) => PlayOneShot(sfxSource, id);

    private void HandlePlayMusic(SoundID id) => PlayOneShot(musicSource, id);

    private void HandlePlayUI(SoundID id) 
    {
        if (!soundLibrary.TryGetClip(id, out var clip) || clip == null) return;
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlayOneShot(AudioSource source, SoundID id) 
    {
        if (soundLibrary.TryGetClip(id, out var clip) && clip != null)
            source.PlayOneShot(clip);
    }
}
