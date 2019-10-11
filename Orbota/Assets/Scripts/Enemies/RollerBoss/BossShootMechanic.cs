using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.CorgiEngine
{
    public class BossShootMechanic : Weapon
    {
        private ProjectileWeapon projectileWeapon;

        public override void Initialization()
        {
            base.Initialization();
            Setup();
        }

        private void Setup()
        {
            projectileWeapon = GetComponent<ProjectileWeapon>();

        }
    }
}