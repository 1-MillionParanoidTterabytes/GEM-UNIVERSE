using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			StartCoroutine (ItemGet ());
		}
	}

	IEnumerator ItemGet(){
		transform.position = transform.position + new Vector3 (0, 100, 0);
		yield return new WaitForSecondsRealtime (2f);
		transform.position = transform.position - new Vector3 (0, 100, 0);
	}
}
