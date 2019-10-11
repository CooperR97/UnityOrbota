using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkShoot : NetworkBehaviour {

    public GameObject bulletPrefab;
    public float bulletSpawnOffset = 2f;
    public int velocity = 15;
    public int clipSize = 5;
    private NetworkedBehaviorSC networkUtil;
    private NetworkedCorgiCharacter character;
    private Vector3 bulletSpawnPosition;
    private int clipCount;

    // Use this for initialization
    void Start () {
        networkUtil = GetComponent<NetworkedBehaviorSC>();
        character = GetComponent<NetworkedCorgiCharacter>();
        clipCount = clipSize;
	}
	
	// Update is called once per frame
	void Update () {
		if(networkUtil.isLocalPlayer && Input.GetMouseButtonDown(0))
        {
            //Send the bullet to the server (shoots bullet on host/server)
            CmdFire();
            //This is not actually doing anything right now

        }
	}


    /**x
     * Currently, when the Host fires everything works fine but when the client fires, the command only gets executed on the server
     *          -- Somehow need to make it happen on the local game instance
     * **/
    [Command]
    void CmdFire()
    {
        RpcFire();
        if (clipCount == 0)
        {
            
        } else
        {
            clipCount--;
        }
        if(character.IsFacingRight)
        {
            bulletSpawnPosition = new Vector3(character.transform.position.x + bulletSpawnOffset, character.transform.position.y, 0);
        } else
        {
            bulletSpawnPosition = new Vector3(character.transform.position.x - bulletSpawnOffset, character.transform.position.y, 0);
        }
        var bullet = Instantiate(bulletPrefab, bulletSpawnPosition, character.transform.rotation);
        bullet.GetComponent<Projectile>().SetOwner(character.gameObject);
        if (character.IsFacingRight)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * velocity;
        } else
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * velocity;
        }


        Destroy(bullet, 2f);
    }


    [ClientRpc]
    void RpcFire()
    {
        if(isServer)
        {
            return;
        }
        if (clipCount == 0)
        {

        }
        else
        {
            clipCount--;
        }
        if (character.IsFacingRight)
        {
            bulletSpawnPosition = new Vector3(character.transform.position.x + bulletSpawnOffset, character.transform.position.y, 0);
        }
        else
        {
            bulletSpawnPosition = new Vector3(character.transform.position.x - bulletSpawnOffset, character.transform.position.y, 0);
        }
        var bullet = Instantiate(bulletPrefab, bulletSpawnPosition, character.transform.rotation);

        if (character.IsFacingRight)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * velocity;
        }
        else
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * velocity;
        }

        Destroy(bullet, 2f);
    }
}
