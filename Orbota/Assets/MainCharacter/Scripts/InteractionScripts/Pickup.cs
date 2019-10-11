using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoreMountains.CorgiEngine
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pickup : MonoBehaviour
    {
        private BoxCollider2D myCollider;
        private Rigidbody2D myRigidbody;
        private GameObject myParent;

        // Use this for initialization
        void Start()
        {
            myCollider = GetComponent<BoxCollider2D>();
            myRigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void pickupItem(GameObject newParent)
        {
            myParent = newParent;
            transform.parent = myParent.transform;
            myRigidbody.simulated = false;
            myCollider.enabled = false;
        }

        public virtual void dropItem()
        {
            myParent = null;
            transform.parent = null;
            myRigidbody.simulated = true;
            myCollider.enabled = true;
        }
    }
}