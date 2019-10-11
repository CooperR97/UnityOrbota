using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class OrbotaAIWalk : AIWalk
{
    public float SecondsToFlip = 5f;

    private float flipTimer = 0;
    private bool engagedWithPlayer;

    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Start()
    {
        base.Initialization();
    }

    protected override void Update()
    {
        base.Update();
        if(flipTimer >= SecondsToFlip)
        {
            if (!engagedWithPlayer)
            {
                flipTimer = 0;
                ChangeDirection();
            } else
            {
                flipTimer = 0;
            }
        } else
        {
            flipTimer += Time.deltaTime;
        }
    }

    public void FlipAI()
    {
        ChangeDirection();
    }

    public void SetEngagedWithPlayer(bool engaged)
    {
        engagedWithPlayer = engaged;
    }
}
