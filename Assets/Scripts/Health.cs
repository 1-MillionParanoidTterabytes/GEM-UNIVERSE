using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour 
{
	private int maxHealth = 100;

	[SerializeField]
	[SyncVar(hook = "OnChangeHealth")]
	private int currentHealth;

	[SerializeField]
	private RectTransform healthBar;
	[SerializeField]
	private Text healthNum;
	[SerializeField]
	private Text KD;

	[SerializeField]
	private GameObject Gem;
	[SerializeField]
	private GameObject smallMesh;
	[SerializeField]
	private GameObject largeMesh;

	[SerializeField]
	private GameObject hitbox1;
	[SerializeField]
	private GameObject hitbox2;
	[SerializeField]
	private GameObject hitbox3;
	[SerializeField]
	private GameObject hitbox4;
	[SerializeField]
	private GameObject hitbox5;
	[SerializeField]
	private GameObject hitbox6;

	public static string[] playerNames = new string[20];
	public static string[] playerSize = new string[20];
	//private static bool[] isAlive = new bool[75];
	//private static int[] playerHp = new int[75];
	//public static string[][] bullets = new string[75][]; //bullets[i] gives the bullets currently shot by that player. Usually its Null. 
	public static string[] item1 = new string[20];
	public static string[] item2 = new string[20];
	public static string[] item3 = new string[20];
	public static string[] item4 = new string[20];
	public static string[] item5 = new string[20];
	public static string[] item6 = new string[20];
	public static string[] item0 = new string[20];
	public static int[] small_ammo = new int[20];
	public static int[] medium_ammo = new int[20];
	public static int[] large_ammo = new int[20];
	public static string[] players = new string[20]; //stores all the NetID's
	public static int[] kills = new int[20];
	public static int[] deaths = new int[20];
	public static string[,] extra = new string[20, 100];//20 rows, 100 columns, each row is a player

	private int index;
	private string SIZE;
	private bool isFirst = true;
	private float waitTime;

	void Start(){
		currentHealth = maxHealth;
		waitTime = Time.time + 0.75f;

		for (int i = 0; i < item0.Length; i++) {
			item0[i] = "compass";
		}
		//BOth of these are used, so they need to have client and server here... Just adding print statements to see what's necessary.
		if (isServer) {
			RpcSetHealthText ();
		} else if (isClient && hasAuthority) {
			CmdSetHealthText ();
		}

		if (uniqueID.counter > 75) {
			print ("Exceeded 75 players");
			return;
		}
		for (int i = 0; i < uniqueID.counter+1; i++) {
			playerNames [i] = "Player" + (i+1);
			//isAlive [i] = true;
			//playerHp [i] = 100;
		}
		/*if (isServer) {
			for (int i = 0; i < uniqueID.counter + 1; i++) {
				print (playerNames [i]);
			}
		}*/
	}

	void Update(){
		//easiest way to track kills and deaths... should use a Syncvar hook but, just if it changes then update will do it.
		int temp_index = 0;
		for(int i = 0; i < players.Length; i++){
			if (players [i] == (gameObject.GetComponent<NetworkIdentity> ().netId).ToString()) {
				KD.text = "K: " + kills [i] + "  D: " + deaths [i];
				temp_index = i;
			}
		}

		/*if (isClient) {
			if (DataToSendToGame.size == "small" && isFirst) {
				CmdSmall (temp_index);
				isFirst = false;
			} else if (DataToSendToGame.size == "medium" && isFirst) {
				CmdMedium (temp_index);
				isFirst = false;
			} else if (DataToSendToGame.size == "large" && isFirst) {
				CmdLarge (temp_index);
				isFirst = false;
			}
		}*/

		//to change sizes - THIS WORKS!!!! MAKE IT VISUALLY CORESSPOND ON MAIN MENU 
		if (Time.time > waitTime) {

			gameObject.GetComponent<NetworkIdentity> ().localPlayerAuthority = true;

//			print ("10 frames");
			if (DataToSendToGame.size == "small" && playerSize [temp_index] != "small") {
				SIZE = "small";
			//	print ("Size is now small");
				Small (temp_index);
			} else if (DataToSendToGame.size == "medium" && playerSize [temp_index] != "medium") {
				SIZE = "medium";
			//	print ("Size is now medium");
				Medium (temp_index);
			} else if (DataToSendToGame.size == "large" && playerSize [temp_index] != "large") {
				SIZE = "large";
//				print ("Size is now large");
				Large (temp_index);
			}
			if (Input.GetKeyDown (KeyCode.U) && isLocalPlayer) {
				print ("Hitboxes Swapped");
				CmdSwapBox ();
			}
		}
	}

	[Command]
	void CmdSwapBox(){
		if (hitbox1.transform.tag == "PlayerCrit") {
			hitbox1.transform.tag = "PlayerPart";
		} else if (hitbox1.transform.tag == "PlayerPart") {
			hitbox1.transform.tag = "PlayerCrit";
		}
		if (hitbox2.transform.tag == "PlayerPart") {
			hitbox2.transform.tag = "PlayerCrit";
		} else if (hitbox2.transform.tag == "PlayerCrit") {
			hitbox2.transform.tag = "PlayerPart";
		}
		if (hitbox3.transform.tag == "PlayerPart") {
			hitbox3.transform.tag = "PlayerCrit";
		} else if (hitbox3.transform.tag == "PlayerCrit") {
			hitbox3.transform.tag = "PlayerPart";
		}
		if (hitbox4.transform.tag == "PlayerPart") {
			hitbox4.transform.tag = "PlayerCrit";
		} else if (hitbox4.transform.tag == "PlayerCrit") {
			hitbox4.transform.tag = "PlayerPart";
		}
		if (hitbox5.transform.tag == "PlayerPart") {
			hitbox5.transform.tag = "PlayerCrit";
		} else if (hitbox5.transform.tag == "PlayerCrit") {
			hitbox5.transform.tag = "PlayerPart";
		}
		if (hitbox6.transform.tag == "PlayerPart") {
			hitbox6.transform.tag = "PlayerCrit";
		} else if (hitbox6.transform.tag == "PlayerCrit") {
			hitbox6.transform.tag = "PlayerPart";
		}

	}

	//THE FOLLOWING THREE COMMANDS SHOULD ONLY OCCUR ON THE START OF THE GAME
	//[ClientRpc]
	void Small(int temp_index){
		playerSize [temp_index] = "small";
		SIZE = "small";
		index = GetIndex ();

		if (SIZE == "large") {
			NaturalMovementPlatforming.playerSize2 [index] = "large";
		} else if (SIZE == "medium") {
			NaturalMovementPlatforming.playerSize2 [index] = "medium";
		} else if (SIZE == "small") {
			NaturalMovementPlatforming.playerSize2 [index] = "small";
		}
	}

	//[ClientRpc]
	void Medium(int temp_index){
		playerSize [temp_index] = "medium";
		SIZE = "medium";

		index = GetIndex ();

		if (SIZE == "large") {
			NaturalMovementPlatforming.playerSize2 [index] = "large";
		} else if (SIZE == "medium") {
			NaturalMovementPlatforming.playerSize2 [index] = "medium";
		} else if (SIZE == "small") {
			NaturalMovementPlatforming.playerSize2 [index] = "small";
		}
	}

	//[ClientRpc]
	void Large(int temp_index){
		playerSize [temp_index] = "large";
		SIZE = "large";
		index = GetIndex ();

		if (SIZE == "large") {
			NaturalMovementPlatforming.playerSize2 [index] = "large";
		} else if (SIZE == "medium") {
			NaturalMovementPlatforming.playerSize2 [index] = "medium";
		} else if (SIZE == "small") {
			NaturalMovementPlatforming.playerSize2 [index] = "small";
		}
	}//GOING TO HAVE TO USE ARMOR/HEALING BULLETS FOR THE LARGE CHARACTER AS CLASS ITEMS -> HEALTH ONLY CHANGES WHEN A COLLISION OCCURS BETWEEN A
	// BULLET, AND THAT PLAYER -> LARGE WEAPONS MIGHT SHOOT OUT HEALTHPACK BULLETS THAT "SUBTRACT NEGATIVE HEALTH"


	[ClientRpc]
	public void RpcTakeDamage(int amount, string shooter, string person_shot/*, GameObject the_shooter_gameObjec*/)
	{
		currentHealth -= amount;

		//GetIndex (transform.name);
		//playerHp [index] = currentHealth;

		//These two are also used. I checked them with 'print statement checking'
		if (isServer) {
			RpcSetHealthText ();
		} else if (isClient && hasAuthority) {
			CmdSetHealthText ();
		}

		int shooterIndex = 0;
		int person_shotIndex = 0;
		//CHECK IF DEAD
		if (currentHealth <= 0)
		{
			print(shooter + " has killed " + person_shot);
			for (int i = 0; i < players.Length; i++) {
				if (players [i] == shooter) { 
					kills [i] += 1;
					shooterIndex = i;
				}
				if (players [i] == person_shot) {
					deaths [i] += 1;
					person_shotIndex = i;
				}
			}


			//SetKDA (shooter, person_shot, shooterIndex, person_shotIndex);
			//THIS DOES NOT WORK because the script is ALWAYS CALLED ON THE PERSON SHOT -> when the person shot's hp goes to 0 
			//netId in this will always be the person shot.
			//print(person_shot + "  " + (gameObject.GetComponent<NetworkIdentity> ().netId).ToString());

			//JUST GONNA UPDATE AND LOOP THROUGH EVERYBODY
			//HOW TO GIVE THE KILL TO THE SHOOTER? pass through the gameObject of the shooter and get component?
			/*if((gameObject.GetComponent<NetworkIdentity> ().netId).ToString() == shooter){
				//print ("20398utgdsfu89o");

				KD.text = "K: " + kills [shooterIndex] + "  D: " + deaths [shooterIndex];
			}*/

			//THIS WORKS BECAUSE IT ALWAYS IS CHECKED WITH THE ID OF THE PERSON WHO'S HP IS LESS THAN OR EQUAL TO 0

			//JUST GONNA UPDATE AND LOOP THROUGH EVERYBODY -> THIS ONLY IS TRIGGERED ON DEATH...
			//even if you loop through everybody the KD.text being updated is the one on the person that was shot. 
			//if ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString () == person_shot) {
				//print ("20398utgdsfu89o");

			//	KD.text = "K: " + kills [person_shotIndex] + "  D: " + deaths [person_shotIndex];
			//}


			//printKillData ();
			//printPlayerNames();

			//send data updating to players 
			currentHealth = 0;
			//if (isServer) {
				//print ("RPCDEAD");
				//RpcDead ();
			//} else if (isClient) {
				//changed CmdDead to Dead because "trying to send command for object without authority"

			/*print((gameObject.GetComponent<NetworkIdentity> ().netId).ToString() + "  Has DIED!");
			int thingToSendIntoRpCDEAD = int.Parse ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString ());
			print (thingToSendIntoRpCDEAD);

			for (int i = 0; i < players.Length; i++) {
				if (players [i] == person_shot.ToString()) {
					print (i + " is the Index of the killed player" + " the size of " + i + " is " + playerSize[i] + " Name is " + gameObject.name);
					thingToSendIntoRpCDEAD = i;
				}
			}*/

			Dead ( gameObject.name ); //LITERALLY, NAME = SIZE
			//}
			//isAlive [index] = false;
		}
	}



	void SetKDA(string shooter, string person_shot, int shooterIndex, int person_shotIndex){

		//THIS IS WRONG BECAUSE SHOOTER AND PERSON SHOT ARE NETID'S AND WILL GET LARGE NUMBERS, Player10's ID is in array index 0, Player 9 index 1.
		//int indexKills = int.Parse (shooter); 
		//int indexDeaths = int.Parse (person_shot);
		int indexKills = shooterIndex;
		int indexDeaths = person_shotIndex;

		if((gameObject.GetComponent<NetworkIdentity> ().netId).ToString() == shooter){
			//print ("20398utgdsfu89o");

			KD.text = "K: " + kills [indexKills] + "  D: " + deaths [indexKills];
		}
		if ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString () == person_shot) {
			//print ("20398utgdsfu89o");

			KD.text = "K: " + kills [indexDeaths] + "  D: " + deaths [indexDeaths];
		}
	}

	[Command]
	void CmdSetKDA(string shooter, string person_shot, int shooterIndex, int person_shotIndex){

		//THIS IS WRONG BECAUSE SHOOTER AND PERSON SHOT ARE NETID'S AND WILL GET LARGE NUMBERS, Player10's ID is in array index 0, Player 9 index 1.
		//int indexKills = int.Parse (shooter); 
		//int indexDeaths = int.Parse (person_shot);
		int indexKills = shooterIndex;
		int indexDeaths = person_shotIndex;

		//not sure why this is not triggering, I believe the netID's should match
		if((gameObject.GetComponent<NetworkIdentity> ().netId).ToString() == shooter){
			//print ("20398utgdsfu89o");

			KD.text = "K: " + kills [indexKills] + "  D: " + deaths [indexKills];
		}
		if ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString () == person_shot) {
			//print ("20398utgdsfu89o");

			KD.text = "K: " + kills [indexDeaths] + "  D: " + deaths [indexDeaths];
		}
	}

	//players is an array usually of: [10,9, ... ] these are netID's and DO NOT CORRESPOND TO INDICES IN OTHER ARRAYS!!!!!!!
	void printPlayers(){
		for (int i = 0; i < players.Length; i++) {
			//currently this puts a kill in player 0 and player 1 slots
			print ("Player: " + i + "  ID: " + players[i]);
		}
	}

	//playerNames is an array  of [player1, player2, ... playern]
	void printPlayerNames(){
		for (int i = 0; i < playerNames.Length; i++) {
			//currently this puts a kill in player 0 and player 1 slots
			print ("Player: " + i + "  ID: " + playerNames[i]);
		}
	}

	void printKillData(){
		for (int i = 0; i < kills.Length; i++) {
			//currently this puts a kill in player 0 and player 1 slots
			print ("Player " + i + " K:" + kills[i] + ", D" + deaths[i]);
		}
	}

	/*[Command]
	public void CmdTakeDamage(int amount)
	{
		currentHealth -= amount;
		print ("OJIU()#IJWOEFH(OP");
		//GetIndex (transform.name);
		//playerHp [index] = currentHealth;

		if (isServer) {
			RpcSetHealthText ();
		} else if (isClient && hasAuthority) {
			CmdSetHealthText ();
		}

		//CHECK IF DEAD
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			if (isServer) {
				RpcDead ();
			} else if (isClient && hasAuthority) {
				CmdDead ();
			}
			//isAlive [index] = false;
		}
	}*/

	int GetIndex(){
		for (int i = 0; i < players.Length; i++) {
			if (players [i] == (gameObject.GetComponent<NetworkIdentity> ().netId).ToString ()) {
				return i;
			}
		}
		return 100;
	}

	/*public void printHealth(){
		//CHANGE THE TEXT FONT SO IT LOOKS NICE, IT IS BLURRY RIGHT NOW
		if (currentHealth >= 100) {
			healthNum.text = "              " + currentHealth;
		} else if (currentHealth < 100){
			healthNum.text = "               " + currentHealth;
		}
	}*/

	//This is failing for clients
	[ClientRpc]
	void RpcSetHealthText(){
		if (isLocalPlayer) {
			if (currentHealth >= 100) {
				healthNum.text = "              " + currentHealth;
			} else if (currentHealth < 100) {
				healthNum.text = "               " + currentHealth;
			}
		}
	}

	//HEalth does not work correctly for clients
	[Command]
	void CmdSetHealthText(){
		if (isLocalPlayer) {
			if (currentHealth >= 100) {
				healthNum.text = "              " + currentHealth;
			} else if (currentHealth < 100) {
				healthNum.text = "               " + currentHealth;
			}
		}
	}

	/*[ClientRpc]
	void RpcDead(){
		if (playerSize [GetIndex()] == "small") {
			smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		} else if (playerSize [GetIndex()] == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		}

		gameObject.GetComponent<Collider> ().enabled = false;
		hitbox1.GetComponent<Collider> ().enabled = false;
		hitbox2.GetComponent<Collider> ().enabled = false;
		hitbox3.GetComponent<Collider> ().enabled = false;
		hitbox4.GetComponent<Collider> ().enabled = false;
		hitbox5.GetComponent<Collider> ().enabled = false;
		hitbox6.GetComponent<Collider> ().enabled = false;


		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		gameObject.GetComponent<Rigidbody> ().useGravity = false; 

		Gem.SetActive (true);
		StartCoroutine (respawn());
	}*/

	//[ClientRpc]
	void Dead(string sizeOfPlayer){
		if (sizeOfPlayer == "small") {			
			smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		} else if (sizeOfPlayer == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		}


		gameObject.GetComponent<Collider> ().enabled = false;
		hitbox1.GetComponent<Collider> ().enabled = false;
		hitbox2.GetComponent<Collider> ().enabled = false;
		hitbox3.GetComponent<Collider> ().enabled = false;
		hitbox4.GetComponent<Collider> ().enabled = false;
		hitbox5.GetComponent<Collider> ().enabled = false;
		hitbox6.GetComponent<Collider> ().enabled = false;

		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
		gameObject.GetComponent<Rigidbody> ().useGravity = false; 

		Gem.SetActive (true);
		StartCoroutine (respawn(sizeOfPlayer));
	}

	[ClientRpc]
	void RpcReSpawn(string O){
		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		gameObject.GetComponent<Rigidbody> ().useGravity = true; 
		gameObject.GetComponent<Collider> ().enabled = true;

		hitbox1.GetComponent<Collider> ().enabled = true;
		hitbox2.GetComponent<Collider> ().enabled = true;
		hitbox3.GetComponent<Collider> ().enabled = true;
		hitbox4.GetComponent<Collider> ().enabled = true;
		hitbox5.GetComponent<Collider> ().enabled = true;
		hitbox6.GetComponent<Collider> ().enabled = true;

		if (O == "small") {
			smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true;
			smallMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.white;
		} else if (O == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true;
			largeMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.white;
		}

		Gem.transform.localPosition = Vector3.zero;
		Gem.SetActive (false);
	}

	[Command]
	void CmdReSpawn(string sIz){
		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		gameObject.GetComponent<Rigidbody> ().useGravity = true; 
		gameObject.GetComponent<Collider> ().enabled = true;

		//LARGE HITBOXES
		hitbox1.GetComponent<Collider> ().enabled = true;
		hitbox2.GetComponent<Collider> ().enabled = true;
		hitbox3.GetComponent<Collider> ().enabled = true;
		hitbox4.GetComponent<Collider> ().enabled = true;
		hitbox5.GetComponent<Collider> ().enabled = true;
		hitbox6.GetComponent<Collider> ().enabled = true;

		if (sIz == "small") {
			smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true;
			smallMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.white;
		} else if (sIz == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true;
			largeMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.white;
		}
	
		Gem.transform.localPosition = Vector3.zero;
		Gem.SetActive (false);
	}


	void SpawnMesh(string size_name){
		gameObject.transform.position = Gem.transform.position;
		if (size_name == "small") {
			smallMesh.GetComponent<SkinnedMeshRenderer>().enabled = true;
		} else if (size_name == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer>().enabled = true;
		}

		//for (int i = 0; i < 5; i++) {
		//	humanoidMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.Lerp (Color.clear, Color.white, 5f);
		//}

		if (size_name == "small") {
			smallMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.clear;
		} else if (size_name == "large") {
			largeMesh.GetComponent<SkinnedMeshRenderer> ().material.color = Color.clear;
		}
	}

	[Command]
	void CmdRespawnEarly(){
		maxHealth = 30;
		if (maxHealth < 30) {
			maxHealth = 30;
		}
		RpcRespawnEarly ();
	}

	[ClientRpc]
	void RpcRespawnEarly(){
		maxHealth = 30;
	}

	IEnumerator respawn(string sizePlayer){

		/*index = GetIndex ();

		if (SIZE == "large") {
			playerSize [index] = "large";
		} else if (SIZE == "medium") {
			playerSize [index] = "medium";
		} else if (SIZE == "small") {
			playerSize [index] = "small";
		}*/

		//THIS WORKS!!! 
		if (sizePlayer == "small") {
			yield return new WaitForSeconds (10);
		} else if (sizePlayer == "medium") {
			yield return new WaitForSeconds (15);
		} else if (sizePlayer == "large") {
			yield return new WaitForSeconds (20);
		}

		SpawnMesh (sizePlayer);

		//R to respawn early
		while(true){

			//Buggy early respawn code
			if ((Input.GetKey (KeyCode.R) && (isClient && hasAuthority))) {
				currentHealth = 30;
				CmdRespawnEarly ();
				break;
			}

			yield return new WaitForSeconds (0.5f);
			currentHealth += 5;
			if (currentHealth >= maxHealth/*previously maxHealth, but lets just assume that is 100*/) {
				break;
			}
		}

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		//These two are both used, print statement checking again...
		if (isServer) {
			RpcReSpawn (sizePlayer);
		} else if (isClient && hasAuthority) {
			CmdReSpawn (sizePlayer);
		}
		yield return new WaitForSeconds(0);
	}

	//This for some reason works fine for clients and servers.
	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
		//THESE TWO are also both used... print statement debug showed that they both are entered by code.
		if (isServer && currentHealth < 100) {
			RpcSetHealthText ();
		} else if (isClient && hasAuthority && currentHealth < 100) {
			CmdSetHealthText ();
		}
		//print (playerNames [index] + ", " + isAlive [index] + ", " + playerHp [index]);
	}
}