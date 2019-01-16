/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RotateCamera : NetworkBehaviour {

	private Vector3 rotateValue;
	private float y;
	private float x;

	public float sensitivity;

	// Use this for initialization
	private void Awake () {
		Cursor.lockState = CursorLockMode.Locked;
		sensitivity = -1.5f;
	}

	void Update()
	{

		y = Input.GetAxis("Mouse X");
		x = Input.GetAxis ("Mouse Y");

		rotateValue = new Vector3(x * (-1.0f)*(sensitivity), y * (sensitivity), 0);

		//print (transform.eulerAngles + "-" + rotateValue);
		if (transform.eulerAngles.x < 340 && transform.eulerAngles.x > 300) {
			transform.eulerAngles = new Vector3(340, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 90) {
			transform.eulerAngles = new Vector3(45, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		transform.eulerAngles = transform.eulerAngles - rotateValue;

		float mouseX = (Input.mousePosition.x / Screen.width ) - 0.5f;
		float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
		transform.localRotation = Quaternion.Euler (new Vector4 (-1f * (mouseY * 180f), mouseX * 360f, transform.localRotation.z));

		if (Input.GetKeyDown (KeyCode.Space)) {
			transform.rotation = Quaternion.Inverse (transform.rotation);
			transform.position = -1 * transform.position;
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			transform.rotation = Quaternion.Inverse (transform.rotation);
		}
	}
}*/