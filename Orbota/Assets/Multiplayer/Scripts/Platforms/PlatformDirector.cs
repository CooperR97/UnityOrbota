using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
    public class PlatformDirector : MonoBehaviour
    {
        [SerializeField]
        protected MultiplayerPathMovement[] myPlatforms;

        [SerializeField]
        public bool shouldMove;

        private void Start()
        {
            myPlatforms = GetComponentsInChildren<MultiplayerPathMovement>();
            if(shouldMove)
            {
                StartPlatforms();
            }
        }

        private void Update()
        {
            if(shouldMove)
            {
                shouldMove = false;
                StartPlatforms();
            }
        }

        public void StartPlatforms()
        {
            foreach (MultiplayerPathMovement path in myPlatforms)
            {
                path.activateNextMove();
            }
        }
    }
}