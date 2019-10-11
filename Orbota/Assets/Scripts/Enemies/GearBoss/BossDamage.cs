using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.CorgiEngine;

public class BossDamage : DamageOnTouch {

    Character character;

    public void Start()
    {
        character = GetComponentInParent<Character>();
    }

    public void FixedUpdate()
    {
        flipOnCharacterFlip();
    }

    public void flipOnCharacterFlip()
    {
        if (character.IsFacingRight && DamageTakenKnockbackForce.x > 0)
        {
            DamageTakenKnockbackForce = new Vector2(DamageTakenKnockbackForce.x * -1, DamageTakenKnockbackForce.y);
        }
        else if (!character.IsFacingRight && DamageTakenKnockbackForce.x < 0)
        {
            DamageTakenKnockbackForce = new Vector2(DamageTakenKnockbackForce.x * -1, DamageTakenKnockbackForce.y);
        }
    }
}
