using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneManager : MonoBehaviour
{
    private static CutSceneManager _instance;
    public static CutSceneManager Instance { get { return _instance; } }

    [SerializeField] private List<PlayableDirector> playableDirectorsList = new List<PlayableDirector>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        CreateCutsceneList();
    }
    
    public PlayableDirector GetCutScene(int index)
    {
        return playableDirectorsList[index];
    }
    private void CreateCutsceneList()
    {
        playableDirectorsList = new List<PlayableDirector>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            playableDirectorsList.Add(transform.GetChild(i).GetComponent<PlayableDirector>());
        }
    }
    public void AssignCamera()
    {
        if (Camera.main == null)
        {
            return;
        }

        Animator cam = Camera.main.GetComponent<Animator>();

        foreach (PlayableDirector playable in playableDirectorsList)
        {
            foreach (PlayableBinding output in playable.playableAsset.outputs)
            {
                if (output.outputTargetType != typeof(Animator)) continue;
                playable.SetGenericBinding(output.sourceObject, cam);
                break;
            }
        }
    }

    public void AssignAnimators()
    {
        Animator player = PlayerStats.Instance.gameObject.GetComponent<Animator>();

        foreach (PlayableBinding output in playableDirectorsList[0].playableAsset.outputs)
        {
            if (output.outputTargetType != typeof(Animator)) continue;
            playableDirectorsList[0].SetGenericBinding(output.sourceObject, player);
            break;
        }
    }

}
