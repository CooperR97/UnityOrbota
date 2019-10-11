using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class NetworkHandleWeapon : CharacterHandleWeapon
    {

        public void networkWeapon()
        {
            NetworkedBehaviorSC networkUtil = GetComponent<NetworkedBehaviorSC>();
            if (!networkUtil.isLocalPlayer)
            {
                _inputManager = null;
            }
        }

        protected override void Initialization()
        {
            base.Initialization();
            networkWeapon();
        }
    }
}