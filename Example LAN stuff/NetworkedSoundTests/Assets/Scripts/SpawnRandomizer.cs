using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomizer : MonoBehaviour {
	[SerializeField]
	private GameObject s1;
	[SerializeField]
	private GameObject s2;
	[SerializeField]
	private GameObject s3;
	[SerializeField]
	private GameObject s4;
	[SerializeField]
	private GameObject s5;
	[SerializeField]
	private GameObject s6;
	[SerializeField]
	private GameObject s7;
	[SerializeField]
	private GameObject s8;
	[SerializeField]
	private GameObject s9;
	[SerializeField]
	private GameObject s10;

	[SerializeField]
	private GameObject PLAYER;

	// Use this for initialization
	void Awake () {
		int spawn = Random.Range (0, 10);	
		if (spawn == 0) {
			PLAYER.transform.position = s1.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 1) {
			PLAYER.transform.position = s2.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 2) {
			PLAYER.transform.position = s3.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 3) {
			PLAYER.transform.position = s4.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 4) {
			PLAYER.transform.position = s5.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 5) {
			PLAYER.transform.position = s6.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 6) {
			PLAYER.transform.position = s7.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 7) {
			PLAYER.transform.position = s8.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 8) {
			PLAYER.transform.position = s9.transform.position + new Vector3(0,1,0); 
		} else if (spawn == 9) {
			PLAYER.transform.position = s10.transform.position + new Vector3(0,1,0); 
		} 
	}
}
