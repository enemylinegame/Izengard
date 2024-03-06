﻿using UnityEngine;

namespace AudioSystem
{
    [System.Serializable]
    public class AudioSourceComponent : IAudioSource
    {
        [field: SerializeField] public int SourceCode { get; private set; } = 1111;

        [field: SerializeField] public AudioSource AudioSource { get; private set; }
    }
}
