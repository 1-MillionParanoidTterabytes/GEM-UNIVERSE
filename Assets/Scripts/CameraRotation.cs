using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
	private Quaternion _rotation = Quaternion.Euler (80, 90, 0);

	void OnCollisionStay (Collision collision) {
		if (collision.collider.name == "Terrain") {
			transform.rotation = _rotation;
		}
	}
}
