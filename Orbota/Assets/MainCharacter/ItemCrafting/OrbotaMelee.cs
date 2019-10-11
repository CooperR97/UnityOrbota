using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;


namespace MoreMountains.CorgiEngine
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Abstracts/MeleeWeapon", order = 5)]
    public class OrbotaMelee : OrbotaWeapon
    {
        /// the possible shapes for the melee weapon's damage area
        public enum MeleeDamageAreaShapes { Rectangle, Circle }

        [Header("Damage Area")]
        /// the shape of the damage area (rectangle or circle)
        public MeleeDamageAreaShapes DamageAreaShape = MeleeDamageAreaShapes.Rectangle;
        /// the size of the damage area
        public Vector2 AreaSize = new Vector2(1, 1);
        /// the offset to apply to the damage area (from the weapon's attachment position
        public Vector2 AreaOffset = new Vector2(1, 0);

        [Header("Damage Area Timing")]
        /// the initial delay to apply before triggering the damage area
        public float InitialDelay = 0f;
        /// the duration during which the damage area is active
        public float ActiveDuration = 1f;

        [Header("Damage Caused")]
        // the layers that will be damaged by this object
        public LayerMask TargetLayerMask;
        /// The amount of health to remove from the player's health
        public int DamageCaused = 10;
        /// the kind of knockback to apply
        public DamageOnTouch.KnockbackStyles Knockback;
        /// The force to apply to the object that gets damaged
        public Vector2 KnockbackForce = new Vector2(10, 2);
        /// The duration of the invincibility frames after the hit (in seconds)
        public float InvincibilityDuration = 0.5f;

        public void updateActiveMelee(Weapon newWeapon)
        {
            //change values of new thing
        }
    }
}
