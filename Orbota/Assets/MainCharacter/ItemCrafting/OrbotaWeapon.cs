using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Abstracts/Weapon", order = 4)]
    public class OrbotaWeapon : OrbotaAbstractObject
    {
        //Setting for adjusting if a weapon is melee or otherwise
        public bool notMelee;
        /// the possible use modes for the trigger
        public enum TriggerModes { SemiAuto, Auto }
        /// the possible states the weapon can be in
        public enum WeaponStates { WeaponIdle, WeaponStart, WeaponDelayBeforeUse, WeaponUse, WeaponDelayBetweenUses, WeaponStop, WeaponReloadNeeded, WeaponReloadStart, WeaponReload, WeaponReloadStop, WeaponInterrupted }

        [Header("Use")]
        /// is this weapon on semi or full auto ?
        public TriggerModes TriggerMode = TriggerModes.Auto;
        /// the delay before use, that will be applied for every shot
        public float DelayBeforeUse = 0f;
        /// whether or not the delay before used can be interrupted by releasing the shoot button (if true, releasing the button will cancel the delayed shot)
        public bool DelayBeforeUseReleaseInterruption = true;
        /// the time (in seconds) between two shots		
        public float TimeBetweenUses = 1f;
        /// whether or not the time between uses can be interrupted by releasing the shoot button (if true, releasing the button will cancel the time between uses)
        public bool TimeBetweenUsesReleaseInterruption = true;

        [Header("Magazine")]
        /// whether or not the weapon is magazine based. If it's not, it'll just take its ammo inside a global pool
        public bool MagazineBased = false;
        /// the size of the magazine
        public int MagazineSize = 30;
        /// if this is true, pressing the fire button when a reload is needed will reload the weapon. Otherwise you'll need to press the reload button
        public bool AutoReload;
        /// the time it takes to reload the weapon
        public float ReloadTime = 2f;
        /// the amount of ammo consumed everytime the weapon fires
        public int AmmoConsumedPerShot = 1;
        /// if this is set to true, the weapon will auto destroy when there's no ammo left
        public bool AutoDestroyWhenEmpty;
        /// the delay (in seconds) before weapon destruction if empty
        public float AutoDestroyWhenEmptyDelay = 1f;
        [ReadOnly]
        /// the current amount of ammo loaded inside the weapon
        public int CurrentAmmoLoaded = 0;

        [Header("Position")]
        /// an offset that will be applied to the weapon once attached to the center of the WeaponAttachment transform.
        public Vector3 WeaponAttachmentOffset = Vector3.zero;
        /// should that weapon be flipped when the character flips ?
        public bool FlipWeaponOnCharacterFlip = true;
        /// the FlipValue will be used to multiply the model's transform's localscale on flip. Usually it's -1,1,1, but feel free to change it to suit your model's specs
        public Vector3 FlipValue = new Vector3(-1, 1, 1);

        [Header("Effects")]
        /// a list of effects to trigger when the weapon is used
        public List<ParticleSystem> ParticleEffects;

        [Header("Animation")]
        /// the other animators (other than the Character's) that you want to update every time this weapon gets used
        public List<Animator> Animators;

        [Header("Animation Parameters Names")]
        /// the name of the weapon's idle animation parameter : this will be true all the time except when the weapon is being used
        public string IdleAnimationParameter;
        /// the name of the weapon's start animation parameter : true at the frame where the weapon starts being used
        public string StartAnimationParameter;
        /// the name of the weapon's delay before use animation parameter : true when the weapon has been activated but hasn't been used yet
        public string DelayBeforeUseAnimationParameter;
        /// the name of the weapon's single use animation parameter : true at each frame the weapon activates (shoots)
        public string SingleUseAnimationParameter;
        /// the name of the weapon's in use animation parameter : true at each frame the weapon has started firing but hasn't stopped yet
        public string UseAnimationParameter;
        /// the name of the weapon's delay between each use animation parameter : true when the weapon is in use
        public string DelayBetweenUsesAnimationParameter;
        /// the name of the weapon stop animation parameter : true after a shot and before the next one or the weapon's stop 
        public string StopAnimationParameter;
        /// the name of the weapon reload start animation parameter
        public string ReloadStartAnimationParameter;
        /// the name of the weapon reload animation parameter
        public string ReloadAnimationParameter;
        /// the name of the weapon reload end animation parameter
        public string ReloadStopAnimationParameter;
        /// the name of the weapon's angle animation parameter
        public string WeaponAngleAnimationParameter;
        /// the name of the weapon's angle animation parameter, adjusted so it's always relative to the direction the character is currently facing
        public string WeaponAngleRelativeAnimationParameter;

        [Header("Sounds")]
        /// the sound to play when the weapon starts being used
        public AudioClip WeaponStartSfx;
        /// the sound to play while the weapon is in use
        public AudioClip WeaponUsedSfx;
        /// the sound to play when the weapon stops being used
        public AudioClip WeaponStopSfx;
        /// the sound to play when the weapon gets reloaded
        public AudioClip WeaponReloadSfx;
        /// the sound to play when the weapon gets reloaded
        public AudioClip WeaponReloadNeededSfx;

        [Header("Feedback")]
        /// whether or not the screen should shake when shooting
        public bool ScreenShake = false;
        /// Shake parameters : intensity, duration (in seconds) and decay
        public Vector3 ShakeParameters = new Vector3(1.5f, 0.5f, 1f);
        /// whether or not the screen should flash when shooting
        public bool ScreenFlash = false;

        [Header("Settings")]
        /// If this is true, the weapon will initialize itself on start, otherwise it'll have to be init manually, usually by the CharacterHandleWeapon class
        public bool InitializeOnStart = false;
        /// whether or not this weapon can be interrupted 
        public bool Interruptable = false;

        //Method called to change the values of a given weapon.
        public void UpdateActiveWeapon(Weapon updatedWeapon)
        {
            //change values of new thing
        }
    }
}