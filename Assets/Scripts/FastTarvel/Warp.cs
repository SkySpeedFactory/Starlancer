using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{

    [SerializeField] private float speed = 10f;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MoveWarpAnimation();
    }

    private void SetPlayerPosition()
    {
        player = PlayerStats.Instance.transform;
        player.position = transform.position;
        player.rotation = transform.rotation;
        player.SetParent(transform);
        player.gameObject.GetComponent<PlayerController>().enabled = false;
    }

    private void MoveWarpAnimation()
    {
        transform.position = transform.position + new Vector3(0, 0, speed * Time.deltaTime);
    }

}
