using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.CorgiEngine;
public class NetworkedCorgiCharacter : Character
{

    NetworkedBehaviorSC NetworkUtility;
    public int score;

    public Vector3 spawnPosition;

    public void Start()
    {
        if (GameObject.Find("Player 1") == null)
        {
            this.name = "Player 1";
        }
        else
        {
            this.name = "Player 2";
        }
        spawnPosition = transform.position;
        NetworkUtility = GetComponent<NetworkedBehaviorSC>();
    }

    void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("Trophy"))
        {
            score++;
        }
    }

    protected override void EveryFrame()
    {
        HandleCharacterStatus();
        EarlyProcessAbilities();
        ProcessAbilities();
        LateProcessAbilities();

        // we send our various states to the animator.		 
        UpdateAnimators();
        RotateModel();
    }

    public void NetworkFreeze()
    {
        NetworkUtility.Freeze();
    }

    public void ResetPosition()
    {
        NetworkUtility.Reset();
    }
}
