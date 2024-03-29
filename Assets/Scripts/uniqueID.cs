using System.Collections;
using System.Collections.Generic;
using System.IO;
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
		if (isClient && !isServer) {
			CmdIncreaseCount (DataToSendToGame.username, DataToSendToGame.password, playerNetID); //increases the counter for clients connected to this session.
		}
	}

	[Command]
	void CmdIncreaseCount(string name, string password, NetworkInstanceId playerIDname){
		if (isServer) {
			WriteString(name, password, playerIDname);
		}


		//DO THE LOCATION STUFF HERE -- UNCOMMENTING THIS COMPLETELY BREAKS THE SERVER
		string path2 = "Assets/Resources/test.txt";

		StreamReader reader = new StreamReader(path2); 
		string text2 = " ";

		bool nameFound = false;
		//MAKE THE FILE [USERNAME \n PASSWORD \n DATA \n USERNAME \n PASSWORD \n DATA \n etc... ]
		while (text2 != null) {
			if (nameFound != true) {
				text2 = reader.ReadLine (); //Username
				if (text2 == name) {
					nameFound = true;
					text2 = reader.ReadLine ();
					text2 = reader.ReadLine ();
					//Debug.Log (text2 + "," + name);
				}
			} else {
				break;
			}
		}
		reader.Close();

		//Debug.Log (text2 + "," + name);
		//currently this is being called on the server. Name is null because the server does not have a name. It cannot find location for null.

		char[] stuff = text2.ToCharArray();


		string tempstring = "";

		for (int i = 0; i < stuff.Length; i++) {
			tempstring = tempstring + stuff [i];
		}

		//print (tempstring);

		//string[] sStrings = tempstring.Split(char.Parse(","));
		string[] sStrings = tempstring.Split (new string[] { "," }, System.StringSplitOptions.None);

		float x5 = float.Parse(sStrings[0]);
		float y5 = float.Parse(sStrings[1]);
		float z5 = float.Parse(sStrings[2]);

		print (x5 + "," + y5 + "," + z5);


		Vector3 varPosition = new Vector3(x5, y5, z5);
		RpcLoadPosition (varPosition, playerNetID);
	}

	[ClientRpc]
	public void RpcLoadPosition(Vector3 new_position, NetworkInstanceId playerID){
		if (playerNetID == GetComponent<NetworkIdentity> ().netId) {
			Vector3 zeroVec = new Vector3 (0.0f, 0.0f, 0.0f);
			if (new_position != zeroVec) {
				transform.position = new_position; 
			}
		}
	}

	[ClientRpc]
	public void RpcCrash(string playerIDname){//same as playerNetID
		if (isServer) {
			return;
		}
		if (isLocalPlayer && playerIDname == (GetComponent<NetworkIdentity> ().netId).ToString()) {

			if(!isServer){
				NetworkManager.singleton.StopClient();
				//Application.Quit (); //if the password is wrong, crash!
			}

			return;
		}
		return;
	}

	void helperFunction(string playerIDname){
		print (playerName + " " + playerIDname);
		RpcCrash(playerIDname);
	}

	void WriteString(string username, string password, NetworkInstanceId playerIDname) //if the function parameters have 'static' then it can't call other functions
	{
		string path = "Assets/Resources/test.txt";

		bool readerNotFound = true;
		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		string text2 = " ";
		string usernameLocation = " ";
		string Datas = "0, 0, 0";
		string text4 = " ";
		string text6 = " ";
		//string reallypointless = " ";

		//MAKE THE FILE [USERNAME \n PASSWORD \n DATA \n USERNAME \n PASSWORD \n DATA \n etc... ]
		while (text2 != null) {
			text2 = reader.ReadLine(); //Username
			text4 = reader.ReadLine(); //Password
			text6 = reader.ReadLine(); //Data
			if (text2 == username) {
				//if the input username is equal to one read in the file, store its location
				//if (text3 == password) {
				//	print (username + " has logged in\n");
				//}
				readerNotFound = false;
				if (text4 == password) {
					//PASSWORD IS CORRECT
					//passwordCorrect = true;
				} else {
					//loop through the player gameObjects, if the one matches the PlayerNetID then lock its inputs.
					helperFunction(playerIDname.ToString()); //playerIDname is 
				}
			}
		}
		reader.Close();

		//print (name2); //NAME2 IS WHAT THE CLIENT INPUTS 
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);

		//If the username is not a new one, then do not create a new place for data to be stored.
		if (readerNotFound) {
			writer.WriteLine (username);
			if (password.Length > 1) {
				writer.WriteLine (password);
			} else {
				writer.WriteLine ("123456$"); //if your password is less than 1 character, change it to this
			}
			writer.WriteLine (Datas);
		} 
		writer.Close();

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
