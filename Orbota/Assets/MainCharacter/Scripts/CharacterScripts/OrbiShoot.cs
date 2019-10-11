using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class OrbiShoot : CharacterHandleWeapon
{
    public override void ShootStart()
    {
        base.ShootStart();
        MMAnimator.UpdateAnimatorBool(_animator, "Firing", true, _character._animatorParameters);
    }

    public override void ShootStop()
    {
        base.ShootStop();
        MMAnimator.UpdateAnimatorBool(_animator, "Firing", false, _character._animatorParameters);
    }

    protected override void InitializeAnimatorParameters()
    {
        base.InitializeAnimatorParameters();
        RegisterAnimatorParameter("Firing", AnimatorControllerParameterType.Bool);
    }

}
