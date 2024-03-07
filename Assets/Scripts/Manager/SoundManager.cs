using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    [SerializeField]
    private AudioMixer MainAudioMixer;

    public List<AudioClip> AudioClipList = new List<AudioClip>();

    public AudioSource GlobalAudioSource;

    private int currentMainVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlaySound(AudioSource source, AudioClip clip, bool loop = false)
    {
        if (loop)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip, currentMainVolume);
        }
        
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        GlobalAudioSource.Stop();
        if (loop)
        {
            GlobalAudioSource.clip = clip;
            GlobalAudioSource.loop = true;
            GlobalAudioSource.Play();
        }
        else
        {
            GlobalAudioSource.PlayOneShot(clip, currentMainVolume);
        }
    }

    public void SetMainVolume(int newVolume)
    {
        MainAudioMixer.SetFloat("MusicVolume", newVolume);
    }
}
