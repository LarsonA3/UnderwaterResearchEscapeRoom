using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SIREN,
    PRINTER,
    DOOR,
    SUCCESS,
    REACTOR,
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

        // Mapping the 4 provided files to the new SoundTypes
        sounds = new() {
            {SoundType.SIREN, new SoundCollection("siren") },     // siren.mp3
            {SoundType.PRINTER, new SoundCollection("printer") }, // printer.wav
            {SoundType.DOOR, new SoundCollection("door") },       // door.wav
            {SoundType.SUCCESS, new SoundCollection("success") }, // success.mp3
            {SoundType.REACTOR, new SoundCollection("reactor") }, // reactor.wav
        };
    }

    public static void Play(SoundType type, AudioSource audioSrc = null, float pitch = -1)
    {
        if (Instance.sounds.ContainsKey(type))
        {
            audioSrc ??= Instance.audioSrc;
            audioSrc.volume = Random.Range(0.7f, 1.0f) * Instance.mainVolume;
            audioSrc.pitch = pitch >= 0 ? pitch : Random.Range(0.75f, 1.25f);
            audioSrc.clip = Instance.sounds[type].GetRandClip();
            audioSrc.Play();
        }
    }
}