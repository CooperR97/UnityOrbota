using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipScript : MonoBehaviour {

    private NetworkedCorgiCharacter character;

	// Use this for initialization
	void Start () {
        character = this.transform.parent.GetComponent<NetworkedCorgiCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		if(character.IsFacingRight)
        {
            this.transform.position = new Vector3(character.transform.position.x + 1, this.transform.position.y, 0);
        } else
        {
            this.transform.position = new Vector3(character.transform.position.x - 1, this.transform.position.y, 0);
        }
	}
}
