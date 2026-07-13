using UnityEngine;

public class PCAudioPlayer : MonoBehaviour
{
    [SerializeField] private SoundLibrarySO soundLibrary;
    [SerializeField] private AudioSource localSource;

    public void PlaySound(SoundID id)
    {
        if (soundLibrary.TryGetClip(id, out var clip) && clip != null)
            localSource.PlayOneShot(clip);
    }
}
