﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletDespawnHit2 : NetworkBehaviour {

	//[SerializeField]
	//private GameObject the_shooter;
	float waitTime = 0.25f;
	float counter = 0.0f;

	//something to record parent, bullet is the 'gameObject' and is instantiated as a child
	void OnTriggerEnter(Collider other){

		if (other.tag == "Terrain") {
			Destroy (gameObject);
		}

		//adding "isServer" makes it so players can't shoot themselves. Was going to do it with netId's on each bullet.
		if (other.tag == "PlayerPart" && isServer && (counter < waitTime)) {
			//can't shoot yourself
			counter += Time.deltaTime;
			waitTime = counter+0.25f;

			print(name + ", " +  (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ());
			//if the server hits the client this prints (6,8) meaning playerID 6 shot playerID 8.
			//The server knows the shooter and shot person, how to send this to clients.
			if ((name != (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ())) {
				print ("Player Hit");
				var health3 = other.GetComponentInParent<Health> ();
				if (health3 != null) {
					if (isServer) {
						health3.RpcTakeDamage (25, name, (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ()/*, the_shooter*/);
					} //else if (isClient) {
					//health.CmdTakeDamage (10/*, parent*/);
					//get the parent passed through this function, record kill.
					//}
				}
				Destroy (gameObject);
			}
		} else if (other.tag == "PlayerCrit" && isServer && (counter < waitTime)) {
			//can't shoot yourself
			counter += Time.deltaTime;
			waitTime = counter+0.25f;

			print(name + ", " +  (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ());
			//if the server hits the client this prints (6,8) meaning playerID 6 shot playerID 8.
			//The server knows the shooter and shot person, how to send this to clients.
			if ((name != (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ())) {
				print ("Player Crit");
				var health3 = other.GetComponentInParent<Health> ();
				if (health3 != null) {
					if (isServer) {
						health3.RpcTakeDamage (50, name, (other.GetComponentInParent<NetworkIdentity> ().netId).ToString ()/*, the_shooter*/);
					} //else if (isClient) {
					//health.CmdTakeDamage (10/*, parent*/);
					//get the parent passed through this function, record kill.
					//}
				}
				Destroy (gameObject);
			}
		}


		if (other.tag == "Gem") {
			print ("Downed Character was Shot");
		}
	}
}
