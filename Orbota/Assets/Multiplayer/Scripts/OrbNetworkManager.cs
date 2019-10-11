using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMountains.CorgiEngine;

public class OrbNetworkManager : NetworkManager
{
    public GameObject trophyPrefab;
    private TrophyGrab trophy;

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        var newTrophy = Instantiate(trophyPrefab, new Vector3(1, 16, 0), Quaternion.identity);
        trophy = newTrophy.GetComponent<TrophyGrab>();
        NetworkServer.Spawn(newTrophy);
    }
}
