using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.CorgiEngine;
public class RollerBossMovement : CharacterAbility {

    protected Vector2 _direction;

    protected virtual void Initialize()
    {
        base.Initialization();
    }
	
	// Update is called once per frame
	void Update () {
        _health.OnHit = Flip;
	}

    private void Flip()
    {
        _direction = (_character.IsFacingRight) ? transform.right : -transform.right;
    }
}
