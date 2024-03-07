using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{
    private GatewayManager gatewayManager;

    [SerializeField] private ScriptableScene loadingSceneData;
    [SerializeField] private ScriptableScene nextSceneData;
    [SerializeField] private int gatewayID = -1;

    private void Awake()
    {
        // assigne self to manager
        gatewayManager = GatewayManager.Instance;
        gatewayManager.AddGatewayToList(this);
    }

    public int GetGatewayID()
    {
        return gatewayID;
    }

    public int GetNextSceneID()
    {
        return nextSceneData.SceneID;
    }

    public void Warp()
    {
        gatewayManager.SetCurrentUsingGatewayID(gatewayID);
        SceneLoaderManager.Instance.ChangeScene(SceneLoaderManager.Instance.GetCurrentSceneIndex(), nextSceneData.SceneID, loadingSceneData.SceneID);
    }
}
