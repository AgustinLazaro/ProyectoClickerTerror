using System;
using UnityEngine;

public class SoundEventChannelSO : ScriptableObject
{
    public event Action<SoundID> OnRaised;
    public void Raise(SoundID id) => OnRaised?.Invoke(id);
}

[CreateAssetMenu(menuName = "Audio/Channels/SFX Channel")]
public class SFXEventChannelSO : SoundEventChannelSO { }

[CreateAssetMenu(menuName = "Audio/Channels/Music Channel")]
public class MusicEventChannelSO : SoundEventChannelSO { }

[CreateAssetMenu(menuName = "Audio/Channels/UI Channel")]
public class UIEventChannelSO : SoundEventChannelSO { }
