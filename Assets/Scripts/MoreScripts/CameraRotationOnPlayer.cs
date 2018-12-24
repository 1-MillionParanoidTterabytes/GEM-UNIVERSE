using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationOnPlayer : MonoBehaviour {
	private Vector3 rotateValue;
	private float y;
	private Quaternion _rotation2 = Quaternion.Euler (10, -90, 0);
	private Quaternion _rotation_start = Quaternion.Euler (10, 90, 0);
	private int carDirection = 1;
	
	// Update is called once per frame
	void Update () {
		y = Input.GetAxis("RotateY");
		rotateValue = new Vector3(0, y * (-0.5f), 0);
		transform.eulerAngles = transform.eulerAngles - rotateValue;
		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.eulerAngles = transform.eulerAngles - (rotateValue - new Vector3(0,180,0));
			carDirection = -1;
		} else if (Input.GetKeyUp (KeyCode.Space)) { 
			transform.eulerAngles = transform.eulerAngles - (rotateValue + new Vector3(0,180,0));
			carDirection = 1;
		}
	}
}
