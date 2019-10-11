using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class SpawnMinions : MonoBehaviour {

    public GameObject minionPrefab;

    Character character;
    private Health health;
    private bool minionsSpawned = false;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        character = GetComponent<Character>();
	}

    void Update()
    {
        if(health.CurrentHealth <= (health.MaximumHealth / 2) && !minionsSpawned)
        {
            Debug.Log("Spawning minions...");
            Instantiate(minionPrefab, new Vector3(character.transform.position.x + 1, character.transform.position.y, character.transform.position.z), new Quaternion());
            Instantiate(minionPrefab, new Vector3(character.transform.position.x - 1, character.transform.position.y, character.transform.position.z), new Quaternion());
            minionsSpawned = true;
        }
    }
}
