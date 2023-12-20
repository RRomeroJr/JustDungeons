using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using Mirror;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [Range(0f, 1f)]
    public float sfxVol = 1.0f;
    [Range(0f, 1f)]
    public float musicVol = 1.0f;
    [Range(0f, 1f)]
    public float uiVol = 1.0f;
    public Sound testSound;
    public List<Sound> sounds = new List<Sound>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);         
        }
        else
        {
            Destroy(this);
        }

        // AudioClip[] audioClips = Resources.LoadAll("Audio", typeof(AudioClip)) as AudioClip[];
        // foreach(AudioClip clip in audioClips)
        // {
            
        // }
    }

    void Start()
    {

    }
    
    public static float GetChannelVolume(SoundTags _st)
    {
        switch (_st)
        {
            case SoundTags.SFX:
                return instance.sfxVol;
            case SoundTags.Music:
                return instance.musicVol;
            case SoundTags.UI:
                return instance.uiVol;
            default:
                return 1.0f;
        }
    }

    public static void PlayFromSource(AudioSource _as, string _clipName)
    {
        Sound s = instance.sounds.Find(x => x.audioClip.name == _clipName);
        // Sound s = instance.testSound

        if(s == null){
            Debug.LogWarning("Sound clip not found: " + _clipName);
            return;
        }
        _as.PlayOneShot(s.audioClip, s.baseVol * GetChannelVolume(s.soundTag));
    }
}
