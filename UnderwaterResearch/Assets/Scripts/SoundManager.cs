using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    TUMBLE,
    PICKUP,
    WALKING,
}

public class SoundCollection
{
    private AudioClip[] clips;
    private int lastClipIndex;

    public SoundCollection(params string[] clipNames)
    {
        this.clips = new AudioClip[clipNames.Length];
        for (int i = 0; i < clipNames.Length; i++)
        {
            clips[i] = Resources.Load<AudioClip>(clipNames[i]);
            if (clips[i] == null)
            {
                Debug.LogError("you gave me an invalid clip");
            }
        }
        lastClipIndex = -1;
    }

    public AudioClip GetRandClip()
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("must have at least one clip");
            return null;
        }
        else if (clips.Length == 1)
        {
            return clips[0];
        }
        else
        {
            int index = lastClipIndex;
            while (index == lastClipIndex)
            {
                index = Random.Range(0, clips.Length);
            }
            lastClipIndex = index;
            return clips[index];
        }
    }
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public float mainVolume = 1.0f;
    private Dictionary<SoundType, SoundCollection> sounds;
    private AudioSource audioSrc;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
        sounds = new() {
      {SoundType.PICKUP, new SoundCollection("safe-door") },
      {SoundType.TUMBLE, new SoundCollection("lock-turn") },
      {SoundType.WALKING, new SoundCollection("walking") },
    };
    }

    public static void Play(SoundType type, AudioSource audioSrc = null, float pitch = -1)
    {
        if (Instance.sounds.ContainsKey(type))
        {
            //var audioSrcToPlayFrom = extAudioSrc == null ? Instance.audioSrc : extAudioSrc;
            //var audioSrcToPlayFrom = extAudioSrc ?? Instance.audioSrc;
            audioSrc ??= Instance.audioSrc;
            audioSrc.volume = Random.Range(0.7f, 1.0f) * Instance.mainVolume;
            audioSrc.pitch = pitch >= 0 ? pitch : Random.Range(0.75f, 1.25f);
            audioSrc.clip = Instance.sounds[type].GetRandClip();
            audioSrc.Play();
        }
    }
}