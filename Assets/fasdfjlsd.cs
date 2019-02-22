using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fasdfjlsd : MonoBehaviour {

	[SerializeField]
	private GameObject Cone;

	[SerializeField]
	private Transform Spawn;

	[SerializeField]
	private GameObject Enemy;


	// Use this for initialization
	void Start () {
		Instantiate(Enemy, Spawn, Spawn);
	}

	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10) {
			transform.position = new Vector3(0,0,0);
		}
		if (Input.GetKey(KeyCode.W)) {
			transform.position += new Vector3(0,0,-1);
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.position += new Vector3(1,0,0);
		}
		if (Input.GetKey(KeyCode.S)) {
			transform.position += new Vector3(0,0,1);
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.position += new Vector3(-1,0,0);
		}
		transform.Rotate(Vector3.right * Time.deltaTime);

	}
}
