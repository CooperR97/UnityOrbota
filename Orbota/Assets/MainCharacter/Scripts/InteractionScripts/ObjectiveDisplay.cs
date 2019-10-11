using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class ObjectiveDisplay : MonoBehaviour
    {
        [SerializeField]
        private ObjectiveLink[] myLinks;
        [SerializeField]
        private SpriteRenderer[] mySprites;
        [SerializeField]
        private Sprite completedSprite;
        [SerializeField]
        private Sprite incompleteSprite;
        private int linkSize;

        // Use this for initialization
        void Start()
        {
            linkSize = myLinks.Length;
            for (int i = 0; i < linkSize; i++)
            {
                mySprites[i].sprite = incompleteSprite;
                if (myLinks[i] != null)
                    myLinks[i].setIndex(i, this);
            }
        }

        public void updateIndex(int newIndex)
        {
            mySprites[newIndex].sprite = completedSprite;
        }
    }
}