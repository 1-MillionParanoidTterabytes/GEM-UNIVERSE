using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	//Should eventually change the respawn points to 'Checkpoints' instead of simply (0,0,0)
	void Update () {
		if (transform.position.y < -10) {
			transform.position = new Vector3 (0, 0, 0);
		}
	}
}
