using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletOnHit : MonoBehaviour {
    /**
     * On Collision with a bullet destroy the bullet (needed because Player is a Trigger)
     **/
    void OnCollisionEnter2D(Collision2D col)
    {
       Debug.Log("Collided " + col.gameObject.name);
       Destroy(gameObject);
    }
}
