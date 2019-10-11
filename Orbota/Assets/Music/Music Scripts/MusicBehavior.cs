using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    public class MusicBehavior : MonoBehaviour
    {

        [SerializeField]
        private SoundManager soundManager;

        public AudioClip SoundClip;
        protected AudioSource _source;

        // Use this for initialization
        void Start()
        {
            if(soundManager)
            {
                _source = gameObject.AddComponent<AudioSource>() as AudioSource;
                _source.playOnAwake = false;
                _source.spatialBlend = 0;
                _source.rolloffMode = AudioRolloffMode.Logarithmic;
                _source.loop = true;

                _source.clip = SoundClip;
                soundManager.PlayBackgroundMusic(_source);
            }
        }
    }
}
