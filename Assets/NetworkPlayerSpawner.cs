using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkPlayerSpawner : MonoBehaviour {

	GameObject[] gameObjects;

 	void Update(){
		gameObjects = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < gameObjects.Length; i++) {
			Destroy (gameObjects[i]);
		}
	}
}
