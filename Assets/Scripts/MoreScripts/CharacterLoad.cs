using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoad : MonoBehaviour {
	[SerializeField]
	private GameObject red;
	[SerializeField]
	private GameObject blue;

	private GameObject currentModel;


	// attach model to the spherical hitbox
	void Start () {
		if (CharacterSelectController.character == "red") {
			currentModel = Instantiate (red, transform.position, transform.rotation) as GameObject;
			currentModel.transform.parent = gameObject.transform;
		} else if (CharacterSelectController.character == "blue") {
			currentModel = Instantiate (blue, transform.position, transform.rotation) as GameObject;
			currentModel.transform.parent = gameObject.transform;
		} 

	}
}
