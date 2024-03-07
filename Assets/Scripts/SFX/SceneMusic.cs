using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SceneMusic : MonoBehaviour
{
    public List<AudioClip> SceneClips;
    public int musicIndex = 0;

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlaySound(SceneClips[musicIndex], true);
    }

    public void playNextClip()
    {
        if (musicIndex < SceneClips.Count)
        {
            soundManager.PlaySound(SceneClips[++musicIndex], false);
        }
        else
        {
            musicIndex = 0;
            soundManager.PlaySound(SceneClips[musicIndex], false);
        }
        
    }
}
