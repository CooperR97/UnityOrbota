using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class AimAtCharacter : MonoBehaviour
    {
        /// The layers the agent will try to shoot at
        public LayerMask TargetLayerMask;

        protected Character _character;

        protected Weapon _weapon;
        protected WeaponAim _aimSystem;

        protected Vector2 _raycastOrigin;
        protected RaycastHit2D _raycast;

        // Use this for initialization
        void Start()
        {
            _weapon = GetComponent<Weapon>();
            _aimSystem = GetComponent<WeaponAim>();
            _character = GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {
            //_aimSystem.CurrentAngle is the current angle of the weapon
            Aim();
            if(_weapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse)
            {

            }
        }

        void Aim()
        {
//
            float currentAngle =_aimSystem.CurrentAngle;

        }
    }
}