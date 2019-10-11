using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class NetworkWeaponUtil : NetworkBehaviour {

    [SerializeField]
    Projectile machineGunProjectile;


    [SyncVar]
    int shooting = 0;

	public void SpawnBulletNetworked()
    {
        shooting = 1;
        //var msg = new IntegerMessage(value);
        //NetworkServer.SendToAll(MsgType.Scene, msg);
    }

    public void Update()
    {
        if(shooting == 1)
        {
            if(machineGunProjectile)
            {
                NetworkIdentity identity = GetComponent<NetworkIdentity>();


                Debug.Log("spawning mg projectile...");
                Debug.Log("localPlayerAuthority --> " + identity.localPlayerAuthority);
                Debug.Log("hasAuthority --> " + identity.hasAuthority);

                if(!identity.localPlayerAuthority)
                {
                    machineGunProjectile.SetWeapon(GetComponent<NetworkProjectileWeapon>());
                    NetworkServer.Spawn(Instantiate(machineGunProjectile).gameObject);
                }
            }
            shooting = 0;
        }
    }
}
