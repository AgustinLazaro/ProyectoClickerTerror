using UnityEngine;

public class PCAudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSFX;
    [SerializeField] private AudioClip closeSFX;
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip errorSFX;
    [SerializeField] private AudioClip winJingleSFX;
    [SerializeField] private AudioClip loseJingleSFX;


    public void PlayOpen() => audioSource.PlayOneShot(openSFX);
    public void PlayClose() => audioSource.PlayOneShot(closeSFX);
    public void PlayClick() => audioSource.PlayOneShot(clickSFX);
    public void PlayError() => audioSource.PlayOneShot(errorSFX);
    public void PlayWinJingle() => audioSource.PlayOneShot(winJingleSFX);
    public void PlayLoseJingle() => audioSource.PlayOneShot(loseJingleSFX);
}
