using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class Pickup_CAbility : CharacterAbility
    {
        private Pickup activeItem;

        protected override void HandleInput()
        {
            if (_inputManager.SecondaryShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                //Debug.Log("Picking up object.");
                if (activeItem == null)
                {
                    var direction = transform.TransformDirection(Vector3.up);
                    if (_character.IsFacingRight)
                    {
                        direction = transform.TransformDirection(Vector3.right);
                    }
                    else
                    {
                        direction = transform.TransformDirection(Vector3.left);
                    }
                    //note the use of var as the type. This is because in c# you 
                    // can have lamda functions which open up the use of untyped variables
                    //these variables can only live INSIDE a function. 
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position + direction);
                    Debug.DrawLine(transform.position, transform.position + direction, Color.green);

                    if (hit.collider != null)
                    {

                        if (hit.collider.GetComponent<Pickup>())
                        {
                            Debug.Log("Picked up object.");
                            activeItem = hit.collider.GetComponent<Pickup>();
                            activeItem.pickupItem(this.gameObject);
                        }
                    }
                }
                else
                {
                    activeItem.dropItem();
                    activeItem = null;
                }
            }
            
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            if(activeItem != null)
            {
                activeItem.dropItem();
            }
        }

    }
}