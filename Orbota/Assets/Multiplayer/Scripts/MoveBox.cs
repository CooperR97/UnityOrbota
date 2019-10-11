using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveBox : NetworkBehaviour
{
    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = spawnPosition;
        CmdReset();
    }

    [Command]
    public void CmdReset()
    {
        transform.position = spawnPosition;
    }
}
