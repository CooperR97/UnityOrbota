using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoreMountains.CorgiEngine
{
    [RequireComponent(typeof(UpdatePlayerHealth))]
    public class HealthUI : MonoBehaviour {

        LevelManager myManager;
        Character playerCharacter;
        Health myHealth;
        UpdatePlayerHealth healthDisplay;
        
        // Use this for initialization
        void Start() {
            myManager = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
            playerCharacter = myManager.Players[0];
            myHealth = playerCharacter.GetComponent<Health>();
            healthDisplay = GetComponent<UpdatePlayerHealth>();
        }

        // Update is called once per frame
        void Update() {
            healthDisplay.updateHealthValue(myHealth.CurrentHealth);
        }
    }
}