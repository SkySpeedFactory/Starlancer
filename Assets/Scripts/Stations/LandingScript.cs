using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LandingScript : MonoBehaviour
{
    private bool isCloseToStation;
    [SerializeField] PlayerHUD hudUIHandler;
    private PlayableDirector landingCS;

    //private SceneLoaderManager sceneLoaderManager;

    [SerializeField] private ScriptableScene sceneData;

    private Station station; // wip: will not work with multiple stations. Get station from ineraction vias playercontroller

    private void Start()
    {
        if (hudUIHandler != null)
        {
            landingCS = CutSceneManager.Instance.GetCutScene(0);
            station = GameObject.Find("Station").GetComponent<Station>(); // wip: will not work with multiple stations. Get station from ineraction vias playercontroller
        }
        CutSceneManager.Instance.AssignAnimators();
        //sceneLoaderManager = SceneLoaderManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        Landing();
    }
    
    public void Landing()
    {
        int layerMask = 1 << 8;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100f, layerMask))
        {
            if (landingCS != null)
            {
                if (landingCS.state != PlayState.Playing && Input.GetKeyDown(KeyCode.F))
                {
                    StartTimeline(landingCS);
                    Invoke("ChangeScene", 6);
                }
            }
        }
    }
    public void StartTimeline(PlayableDirector director)
    {
        director.Play();
    }

    public void ChangeScene()
    {
        TradingManager.Instance.SetCurrentLandedStationID(station);
        PlayerStats.Instance.GetComponent<PlayerController>().ChangeShipLookState(true);
        SceneLoaderManager.Instance.ChangeScene(SceneLoaderManager.Instance.SceneData.CurrentScene.SceneIndex, 3);
    }
}
