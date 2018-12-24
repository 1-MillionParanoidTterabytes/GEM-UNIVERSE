using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class uniqueID : NetworkBehaviour {

	[SyncVar] public string playerName;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;
	public static int counter = 0;

	public override void OnStartLocalPlayer(){
		GetNetIdentity ();
		SetID ();

	}

	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetID ();
		}
		AddToPlayers ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString());
	}

	void SetID(){
		//counts number of players
		counter += 1;
		myTransform.name = "Player" + counter;
	}

	//loops through the data thing and if it finds a matching ID, return. If it does not match then set the next null to that ID
	public void AddToPlayers(string playerID){
		for (int i = 0; i < Health.players.Length; i++) {
			if (Health.players [i] == playerID) {
				return;
			} else if (Health.players [i] == null) {
				Health.players [i] = playerID;
				return;
			}
		}
	}

	//returns the index of all data for that player, ideally all data for that player is that INDEX in all the other arrays. Copy-pasted into all scripts
	public int FindInPlayers(string playerID){
		for (int i = 0; i < Health.players.Length; i++) {
			if (Health.players [i] == playerID) {
				return i;
			}
		}
		return 0;
	}

	//also placed into all scripts
	void printPlayers(){
		for (int i = 0; i < Health.players.Length; i++) {
			Debug.Log (Health.players [i]);
		}
	}

	[Client]
	void GetNetIdentity(){
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerName (makeUniqueID());
	}

	string makeUniqueID(){
		string uniqueName = "Player" + playerNetID.ToString();
		return uniqueName;
	}

	[Command]
	void CmdTellServerName(string name){
		playerName = name;
	}
}
