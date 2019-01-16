using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDespawnAndHit : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Terrain") {
			Destroy (gameObject);
		}
	}
}
