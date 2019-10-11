using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ObjectiveLink : MonoBehaviour
    {

        public enum LinkType { LocationLink, ActorLink }

        [SerializeField]
        private LinkType myType;
        private BoxCollider2D myCollider;
        private Health myHealth;
        private ObjectiveDisplay myDisplay;
        [SerializeField]
        private int index;

        void Start()
        {
            switch(myType)
            {
                case LinkType.ActorLink:
                    CreateActorLink();
                    break;
                case LinkType.LocationLink:
                    CreateLocationLink();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateLink(myType);
        }

        private void UpdateLink(LinkType type)
        {
            switch (type)
            {
                case LinkType.ActorLink:
                    UpdateActor();
                    break;
                case LinkType.LocationLink:
                    break;
            }
        }

        public void setIndex(int newIndex, ObjectiveDisplay newDisplay)
        {
            index = newIndex;
            myDisplay = newDisplay;
        }

        private void UpdateActor()
        {
            if (myHealth.CurrentHealth <= 10)
            {
                
                myDisplay.updateIndex(index);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if(other.GetComponent<Pickup>())
            {
                myDisplay.updateIndex(index);
            }
        }

        private void CreateLocationLink()
        {
            myCollider = GetComponent<BoxCollider2D>();
            myCollider.isTrigger = true;
        }

        private void CreateActorLink()
        {
            myHealth = GetComponent<Health>();
        }
    }
}