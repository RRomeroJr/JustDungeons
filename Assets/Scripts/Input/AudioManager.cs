using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using Mirror;
using System.IO;
using ParrelSync;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField, Range(0f, 1f)]
    private float sfxVol = 0.3f;
    public float SFXVol
    {
        get => sfxVol;
        set
        {
            sfxVol = value;
            SavePrefs();
        }
    }

    
    [SerializeField, Range(0f, 1f)]
    private float musicVol = 0.3f;

    public float MusicVol
    {
        get => musicVol;
        set
        {
            musicVol = value;
            SavePrefs();
        }
    }


    [SerializeField, Range(0f, 1f)]
    private float uiVol = 0.3f;

    public float UIVol
    {
        get => uiVol;
        set
        {
            uiVol = value;
            SavePrefs();
        }
    }
    public Sound testSound;
    public List<Sound> sounds = new List<Sound>();
    private void OnValidate()
    {
        SavePrefs();
    }
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
        string dirPath = Application.dataPath + "/" + "Preferences";
        string filename = "OptionsPrefs.json";
        OptionsData optionsData = null;
        if (!File.Exists($"{dirPath}/{filename}"))
        {
            SavePrefs();
        }
        optionsData = JsonUtility.FromJson<OptionsData>(File.ReadAllText($"{dirPath}/{filename}"));
        if (optionsData == null)
        {
            Debug.LogError($"Could not read audio preferences.");
        }
        sfxVol = optionsData.SFXVol;
        musicVol = optionsData.musicVol;
        uiVol = optionsData.uiVol;
    }
    public void SavePrefs()
    {
        var _prefs = new OptionsData();
        _prefs.SFXVol = SFXVol;
        _prefs.musicVol = MusicVol;
        _prefs.uiVol = UIVol;
        string dirPath = Application.dataPath + "/" + "Preferences";
        string filename = "OptionsPrefs.json";
        if (!Directory.Exists(dirPath))
        {
            Debug.Log($"Making dir: {dirPath}");
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllText($"{dirPath}/{filename}", JsonUtility.ToJson(_prefs, prettyPrint: true));
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
