using System;
using UnityEngine;

public class SoundEventChannelSO : ScriptableObject
{
    public event Action<SoundID> OnRaised;
    public void Raise(SoundID id) => OnRaised?.Invoke(id);
}
