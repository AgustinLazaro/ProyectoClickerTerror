using UnityEngine;
using System.Collections.Generic;
public class RadioInteractable : InteractableBase
{
    [Header("Event Connections")]
    public SFXEventChannelSO sfxChannel;

    [Header("Stations (Local 3D Audio)")]
    public List<AudioClip> radioClips;

    private AudioSource audioSource;
    private int interactionCount = 0;
    private List<AudioClip> availableClips;

    protected override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        audioSource.loop = true;

        ReloadStations();
    }

    public override void OnPressE(PlayerInteraction player)
    {
        interactionCount++;

        if (interactionCount >= 4)
        {
            TurnOffRadio();
        }
        else
        {
            ChangeStation();
        }
    }

    private void ChangeStation()
    {
        if (availableClips.Count == 0) ReloadStations();

        int randomIndex = Random.Range(0, availableClips.Count);
        AudioClip chosenClip = availableClips[randomIndex];

        availableClips.RemoveAt(randomIndex);

        audioSource.clip = chosenClip;
        audioSource.Play();
    }

    private void TurnOffRadio()
    {
        audioSource.Stop();
        interactionCount = 0;
        ReloadStations();
    }

    private void ReloadStations()
    {
        availableClips = new List<AudioClip>(radioClips);
    }
}