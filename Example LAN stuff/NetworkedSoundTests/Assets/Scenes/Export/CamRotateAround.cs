using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotateAround : MonoBehaviour {

	private Vector3 rotateValue;
	private float y;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update()
	{
		y = Input.GetAxis("RotateY");
		rotateValue = new Vector3(0, y * -1, 0);
		transform.eulerAngles = transform.eulerAngles - rotateValue;
	}
}