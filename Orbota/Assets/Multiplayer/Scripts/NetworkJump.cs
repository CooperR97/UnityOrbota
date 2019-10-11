using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class NetworkJump : CharacterJump
    {
        NetworkedBehaviorSC networkUtil;

        protected override void Initialization()
        {
            base.Initialization();
            networkUtil = GetComponent<NetworkedBehaviorSC>();
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            networkJump();
        }

        public void networkProcess()
        {
            ProcessAbility();
        }

        public void networkJump()
        {
            if (this.JumpAuthorized && _inputManager != null && _inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                if(networkUtil.isClient && !networkUtil.isServer)
                {
                    networkUtil.CmdJump();
                }
                if(networkUtil.isServer)
                {
                    networkUtil.RpcJump();
                }
            }
            if (!networkUtil.isLocalPlayer)
            {
                _inputManager = null;
            }
        }
    }
}