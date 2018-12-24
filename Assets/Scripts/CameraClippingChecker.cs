using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClippingChecker : MonoBehaviour {

	[SerializeField]
	private GameObject rotationController;

	void OnTriggerEnter(Collider other){
		//Camera will go through spawn platforms with -> Edit -> Project Settings -> Physics, unchecked Player/Spawns collisions
		//collider on Camera will stop it from going through walls
		print (rotationController.transform.rotation.eulerAngles.x + ", " + rotationController.transform.rotation.eulerAngles.y + ", " + rotationController.transform.rotation.eulerAngles.z);

		if ((other.tag == "Terrain" ) && (rotationController.transform.rotation.eulerAngles.x > 340 || rotationController.transform.rotation.eulerAngles.x < 20)) {
			rotationController.transform.Rotate (Vector3.right * 10);
		} /*else if (other.tag == "Terrain" &&rotationController.transform.rotation.eulerAngles.y > 180){
			rotationController.transform.Rotate (Vector3.up * 8);
		} else if (other.tag == "Terrain" &&rotationController.transform.rotation.eulerAngles.y < 180){
			rotationController.transform.Rotate (Vector3.down * 8);
		}*/
	}

	/*void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag != "Terrain") {
			Physics.IgnoreCollision (collision.collider, GetComponent<Collider>());
		}
	}*/
}
