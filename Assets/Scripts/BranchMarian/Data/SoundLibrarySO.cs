using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Library")]
public class SoundLibrarySO : ScriptableObject
{
    [Serializable]
    private struct SoundEntry
    {
        public SoundID id;
        public AudioClip clip;
    }

    [SerializeField] private SoundEntry[] sounds;

    private Dictionary<SoundID, AudioClip> _clipsById;

    private void OnEnable()
    {
        _clipsById = new Dictionary<SoundID, AudioClip>();
        foreach (var entry in sounds)
            _clipsById[entry.id] = entry.clip;
    }

    public bool TryGetClip(SoundID id, out AudioClip clip) =>
        _clipsById.TryGetValue(id, out clip);
}
