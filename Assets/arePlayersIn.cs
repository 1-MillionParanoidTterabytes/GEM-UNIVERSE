using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class arePlayersIn : NetworkBehaviour {

	//If the localPlayer is not near the lobby then deactivate it.
	//If no LocalPlayers are near the lobby then delete it from the server
	private int players = 0;


	//OnTriggerEnter and OnTriggerExit is called once per collision
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			players++;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			players--;
			//print (players);
			if(players == 0 && isClient /*&& GameObject.FindGameObjectsWithTag ("cubeCounter").Length > 1*/){
				//THE SERVER MIRROR CANNOT ALLOCATE LOBBIES!!! ONLY CLIENTS CAN AND THAT IS GOOD!
				//Instantiate(cubeCounter, pointlessPosition, pointlessPosition);
				//print (numDeletes);
				//Destroy (gameObject); I got this to work once... But it seems to only work if the client and server make effort to sync up.
			}
		}
	}

	//Should we have number of players colliding with a large cube, when that hits 0, delete it...
	//or-- Use the distance formula to see if people are within the range to be 'in' the lobby [<(1000,1000,1000)*lobbyNumber*5610] 
	//																						probably is a good distance.

}
