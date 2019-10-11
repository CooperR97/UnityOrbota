using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.CorgiEngine;
public class RollerBossProjectileWeapon : ProjectileWeapon {

    protected WeaponAim _weaponAim;

    public override void Initialization()
    {
        base.Initialization();
        _weaponAim = GetComponent<WeaponAim>();
    }

    public WeaponAim getWeaponAim()
    {
        return _weaponAim;
    }
}
