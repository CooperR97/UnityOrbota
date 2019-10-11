using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

using UnityEngine.Networking;
public class NetworkedBehaviorSC : NetworkBehaviour
{

    public GameObject localPlayerIdentifier;
    public NetworkedCorgiCharacter character;
    public SpriteRenderer characterSprite;

    public Vector2 serverPlayerPosition;
    
    public void Start()
    {
        character = GetComponent<NetworkedCorgiCharacter>();
        characterSprite = GetComponent<SpriteRenderer>();
    }

    public bool IsLocal()
    {
        return isLocalPlayer;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        localPlayerIdentifier.SetActive(true);
    }

    public void Freeze()
    {
        CmdFreeze();
        RpcFreeze();
    }

    [Command]
    public void CmdFreeze()
    {
        character.Freeze();
    }

    [ClientRpc]
    public void RpcFreeze()
    {
        character.Freeze();
    }

    public void Reset()
    {
        CmdReset();
        RpcReset();
    }

    [Command]
    public void CmdReset()
    {
        transform.position = character.spawnPosition;
        character.UnFreeze();
    }

    [ClientRpc]
    public void RpcReset()
    {
        transform.position = character.spawnPosition;
        character.UnFreeze();
    }

    [Command]
    public void CmdFlip()
    {
        character.IsFacingRight = !character.IsFacingRight;
        characterSprite.flipX = !characterSprite.flipX;
    }

    [Command]
    public void CmdJump()
    {
        character.GetComponent<NetworkJump>().JumpStart();
        character.GetComponent<NetworkJump>().ProcessAbility();
    }

    [Command]
    public void CmdMove(float networkHorizontalInput)
    {
        character.GetComponent<NetworkHorizontalMovement>().networkHandleInput(networkHorizontalInput);
        character.GetComponent<NetworkHorizontalMovement>().networkProcess();
    }

    [ClientRpc]
    public void RpcFlip()
    {
        character.IsFacingRight = !character.IsFacingRight;
        characterSprite.flipX = !characterSprite.flipX;
    }

    [ClientRpc]
    public void RpcJump()
    {
        character.GetComponent<NetworkJump>().JumpStart();
        character.GetComponent<NetworkJump>().ProcessAbility();
    }

    //INSTEAD OF HAVING TO CALL SPECIFIC METHODS IN THE FUNCTION JUST CALL UPDATE() ?
    [ClientRpc]
    public void RpcMove(float networkHorizontalInput)
    {
        character.GetComponent<NetworkHorizontalMovement>().networkHandleInput(networkHorizontalInput);
        character.GetComponent<NetworkHorizontalMovement>().networkProcess();
    }
}
