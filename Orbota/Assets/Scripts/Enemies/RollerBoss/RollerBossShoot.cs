using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class RollerBossShoot : CharacterAbility
    {
        private Character _boss;
        private Health bossHealth;

        private Weapon bossWeapon;
        private WeaponAim _aimSystem;
        private CharacterHandleWeapon weaponHandler;

        protected Vector2 _direction;
        protected BoxCollider2D _collider;
        protected int _colliderHeight;

        /// The maximum distance at which the AI can shoot at the player
		public float ShootDistance = 10f;
        /// The offset to apply to the shoot origin point (by default the position of the object)
		public Vector2 RaycastOriginOffset = new Vector2(0, -1.7f);

        /// The layers the agent will try to shoot at
        public LayerMask TargetLayerMask;


        protected Vector2 _raycastOrigin;
        protected RaycastHit2D _raycast;

        private AimControl aimControl;

        protected override void Initialization()
        {
            base.Initialization();
            Setup();
        }

        private void Setup()
        {
            _collider = GetComponent<BoxCollider2D>();
            _boss = GetComponent<Character>();

            weaponHandler = GetComponent<CharacterHandleWeapon>();

            bossWeapon = weaponHandler.InitialWeapon;
            bossHealth = GetComponent<Health>();

            _aimSystem = bossWeapon.GetComponent<WeaponAim>();
            aimControl = bossWeapon.GetComponent<AimControl>();
        }

        private void FixedUpdate()
        {
            if ((_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)) {
                weaponHandler.ShootStop();
                return;
            }
            rollerBossFire();
        }

        private void rollerBossFire()
        {
            _character.UnFreeze();
            // determine the direction of the raycast 
            _direction = (_character.IsFacingRight) ? transform.right : -transform.right;

            for(int i = 0; i < 10; i++)
            {
                _raycastOrigin.x = _character.IsFacingRight ? transform.position.x + RaycastOriginOffset.x : transform.position.x - RaycastOriginOffset.x;
                _raycastOrigin.y = transform.position.y + RaycastOriginOffset.y + i/2f;
                _raycast = MMDebug.RayCast(_raycastOrigin, _direction, ShootDistance, TargetLayerMask, Color.yellow, true);
                if (_raycast)
                {
                    weaponHandler.ShootStart();
                    _character.Freeze();
                }
            }
        }
    }
}
