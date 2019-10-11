using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.Networking;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// Add this ability to a Character to have it handle horizontal movement (walk, and potentially run, crawl, etc)
    /// Animator parameters : Speed (float), Walking (bool)
    /// </summary>
    [AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement")]
    public class NetworkHorizontalMovement : CharacterHorizontalMovement
    {
        NetworkedBehaviorSC networkUtil;
        NetworkedCorgiCharacter character;

        /// <summary>
		/// On Initialization, we set our movement speed to WalkSpeed.
		/// </summary>
		protected override void Initialization()
        {
            base.Initialization();
            networkUtil = GetComponent<NetworkedBehaviorSC>();
            character = GetComponent<NetworkedCorgiCharacter>();
            networkCharacter();
        }

        public void networkCharacter()
        {
            if (!networkUtil.isLocalPlayer)
            {
                _inputManager = null;
            }
        }

        /// <summary>
	    /// Called at the very start of the ability's cycle, and intended to be overridden, looks for input and calls
	    /// methods if conditions are met
	    /// </summary>
	    protected override void HandleInput()
        {
            networkUtil.CmdMove(_horizontalInput);
            if(networkUtil.isServer)
            {
                networkUtil.RpcMove(_horizontalInput);
            }
            _horizontalMovement = _horizontalInput;
        }

        public void networkHandleInput(float networkHorizontalInput)
        {
            _horizontalMovement = networkHorizontalInput;
        }

        public void networkProcess()
        {
            ProcessAbility();
        }
    }
}