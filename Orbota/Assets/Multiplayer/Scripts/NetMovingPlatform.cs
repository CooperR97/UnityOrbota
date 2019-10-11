using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MoreMountains.CorgiEngine;

public class NetMovingPlatform : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(isClient && !isServer)
        {
            GetComponent<MovingPlatform>().MovementSpeed = 0;
        }
    }
}
