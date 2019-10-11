using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDisconnect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
	}
	
}
