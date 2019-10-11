using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using System;
using UnityEngine.Networking;

public class NetworkProjectileWeapon : ProjectileWeapon {


    /// <summary>
    /// Spawns a new object and positions/resizes it
    /// </summary>
    public override GameObject SpawnProjectile(Vector3 spawnPosition, int projectileIndex, int totalProjectiles, bool triggerObjectActivation = true)
    {
        Console.WriteLine("Spawning projectile.... ");
        /// we get the next object in the pool and make sure it's not null
        GameObject nextGameObject = ObjectPooler.GetPooledGameObject();
        
        //Get the Network Weapon Utility to Spawn the Object on the Network
        NetworkWeaponUtil networkUtil = GetComponent<NetworkWeaponUtil>();

        // mandatory checks
        if (nextGameObject == null) { return null; }
        if (nextGameObject.GetComponent<MMPoolableObject>() == null)
        {
            throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
        }
        // we position the object
        nextGameObject.transform.position = spawnPosition;
        // we set its direction

        Projectile projectile = nextGameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetWeapon(this);
            if (Owner != null)
            {
                projectile.SetOwner(Owner.gameObject);
            }
        }
        // we activate the object
        nextGameObject.gameObject.SetActive(true);
        networkUtil.SpawnBulletNetworked();


        if (projectile != null)
        {
            if (RandomSpread)
            {
                _randomSpreadDirection.x = UnityEngine.Random.Range(-Spread.x, Spread.x);
                _randomSpreadDirection.y = UnityEngine.Random.Range(-Spread.y, Spread.y);
                _randomSpreadDirection.z = UnityEngine.Random.Range(-Spread.z, Spread.z);
            }
            else
            {
                _randomSpreadDirection.x = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.x, Spread.x);
                _randomSpreadDirection.y = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.y, Spread.y);
                _randomSpreadDirection.z = MMMaths.Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.z, Spread.z);
            }

            Quaternion spread = Quaternion.Euler(_randomSpreadDirection);
            projectile.SetDirection(spread * transform.right * (Flipped ? -1 : 1), transform.rotation, Owner.IsFacingRight);

            if (RotateWeaponOnSpread)
            {
                this.transform.rotation = spread;
            }
        }

        if (triggerObjectActivation)
        {
            if (nextGameObject.GetComponent<MMPoolableObject>() != null)
            {
                nextGameObject.GetComponent<MMPoolableObject>().TriggerOnSpawnComplete();
            }
        }

        return (nextGameObject);
    }
}
