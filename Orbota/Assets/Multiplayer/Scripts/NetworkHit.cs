using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("Player") || collisionObject.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
