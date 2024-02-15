using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using Mirror;

public enum SoundTags
{
    SFX,
    Music,
    UI
}
[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float baseVol = 1.0f;

    // [Range(0f, 1f)]
    // public float pitch;

    public SoundTags soundTag;
    public Sound()
    {
        baseVol = 1.0f;
    }

}

