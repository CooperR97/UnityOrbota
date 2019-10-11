using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class BossRotate : MMAutoRotate {

    Character character;
    CharacterJump characterJump;

    public void Start()
    {
        character = GetComponentInParent<Character>();
        characterJump = GetComponentInParent<CharacterJump>();
    }

    public void FixedUpdate()
    {
        flipOnCharacterFlip();
    }

    public void flipOnCharacterFlip()
    {
        if(character.IsFacingRight && RotationSpeed.z > 0)
        {
            characterJump.JumpStart();
            RotationSpeed = new Vector3(RotationSpeed.x, RotationSpeed.y, RotationSpeed.z * -1);
        } else if(!character.IsFacingRight && RotationSpeed.z < 0)
        {
            characterJump.JumpStart();
            RotationSpeed = new Vector3(RotationSpeed.x, RotationSpeed.y, RotationSpeed.z * -1);
        }
    }
}
