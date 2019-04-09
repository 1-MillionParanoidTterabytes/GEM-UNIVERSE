using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NaturalMovementPlatforming : NetworkBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private GameObject ShotgunBulletPrefab;
	[SerializeField]
	private GameObject CompassBulletPrefab;
	[SerializeField]
	private GameObject bulletSpawn;
	[SerializeField]
	private GameObject bulletSpawn2;
	[SerializeField]
	private Camera camera;
	[SerializeField]
	private GameObject rotationChecker;

	[SerializeField]
	private GameObject waterCameras;
	[SerializeField]
	private GameObject playerCamera;

	[Header("Movement")]
	[SerializeField]
	private float _acceleration;
	[SerializeField]
	private float _maxAcceleration;
	[SerializeField]
	private float _maxVelocity;

	[Header("MoveSpeed")]
	[SerializeField]
	private float _speed;
	bool isGround = false;

	[Header("Jumping")]
	[SerializeField]
	private float _groundedDistance;
	[SerializeField]
	private float _jumpVelocity;
	[SerializeField]
	private float _jumpCooldown;
	//[SerializeField]
	//private bool Touching = false;
	[SerializeField]
	private float _hangtime;

	private Rigidbody _rigidbody3d;
	private float _jumpTimer;
	private Vector3 goForward = new Vector3(0,0,1);

	private Vector3 rotateValue;
	private float y;
	private float x;
	[Header("Sensitivity")]
	[SerializeField]
	private float sensitivity;
	[SerializeField]
	private GameObject canvas;

	[SerializeField]
	private Text AmmoText;
	[SerializeField]
	private Text AmmoText2;
	[SerializeField]
	private Text AmmoText3;

	[SerializeField]
	private Texture2D crosshair;
	[SerializeField]
	private Text items;
	[SerializeField]
	private AudioListener listener;

	[Header("Models")]
	[SerializeField]
	private GameObject smallMesh;
	[SerializeField] 
	private GameObject largeMesh;
	[SerializeField]
	private GameObject lifeGemNOTcritboxGem;

	private int numPlayers = 0;
	private Rect position;
	private float waitTime;
	private float waitTime2;
	public static string[] playerSize2 = new string[20];
	private bool pointless = true;

	[SyncVar(hook = "MakeLarge")]
	private bool large = true;
	[SyncVar(hook = "MakeSmall")]
	private bool small = true;

	private void Awake()
	{
		_rigidbody3d = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		sensitivity = -1.5f;
		waitTime2 = Time.time + 1;

	}
		
	/*//check to see if the player is in contact with the ground, works on slopes too!
	//as long as the player collides with the ground then isGround is true
	void OnCollisionStay(Collision hit)
	{
		if (hit.gameObject.tag == "ground") {
			isGround = true;
		}
	}

	//when stop colliding with ground give 0.2 sec hang time 
	void OnCollisionExit(Collision hit)
	{
		if (hit.gameObject.tag == "ground")
		{
			StartCoroutine(notcollidingwithGround());
		}
	}

	//some platformer hangtime, even after not colliding with the ground there is a 0.2 second window to jump (this is too generous, change sometime?s)
	IEnumerator notcollidingwithGround(){
		yield return new WaitForSeconds (_hangtime);
		isGround = false;
	}*/
	int index = 0;
	bool lerp = false;

	void OnTriggerEnter(Collider other){
		
		for (int i = 0; i < Health.players.Length; i++) {
			if (Health.players [i] == (gameObject.GetComponent<NetworkIdentity> ().netId).ToString()) {
				index = i; 
			}
		}

		if (other.tag == "s_ammo") {
			Health.small_ammo[index] += 30;
			AmmoText.text = "S" + Health.small_ammo[index].ToString(); 
		} else if (other.tag == "m_ammo") {
			Health.medium_ammo[index] += 10;
			AmmoText2.text = "M" + Health.medium_ammo[index].ToString(); 
		} else if (other.tag == "l_ammo") {
			Health.large_ammo[index] += 3;
			AmmoText3.text = "L" + Health.large_ammo[index].ToString(); 
		}
	}

	void OnGUI() {
		GUI.DrawTexture (position, crosshair);
	}

	private string[] activeItem = Health.item0;

	[Command]
	void CmdMeshLarge(){
		large = false;
		gameObject.name = "large";
		RpcLarge ();
	}

	[Command]
	void CmdUpdateMeshLarge(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		gameObject.name = "large";
		RpcSendLarge();
	}

	[Command]
	void CmdRemoveMeshLarge(){
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false; //make people see large mesh as your character
		RpcSendRemovedLarge();
	}

	[ClientRpc]
	void RpcSendLarge(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		gameObject.name = "large";
	}

	[ClientRpc]
	void RpcSendRemovedLarge(){
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false; //make people see large mesh as your character
	}

	[ClientRpc]
	void RpcSendSmall(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		gameObject.name = "small";
	}

	[ClientRpc]
	void RpcSendRemovedSmall(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false; //make people see large mesh as your character
	}

	[Command]
	void CmdUpdateMeshSmall(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		gameObject.name = "small";
		RpcSendSmall();
	}

	[Command]
	void CmdRemoveMeshSmall(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = false; //make people see large mesh as your character
		RpcSendRemovedSmall();
	}

	void MakeLarge(bool visibility){
		//Debug.Log (visibility);
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		camera.transform.position = camera.transform.position + new Vector3(0,1,-1);
	}

	[ClientRpc]
	void RpcLarge(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		largeMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
		gameObject.name = "large";
	}

	[Command]
	void CmdMeshSmall(){
		small = false;
		gameObject.name = "small";
		RpcSmall ();
	}
		
	void MakeSmall(bool visibility){
		//Debug.Log (visibility);
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true; //make people see large mesh as your character
	}

	[ClientRpc]
	void RpcSmall(){
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		smallMesh.GetComponent<SkinnedMeshRenderer> ().enabled = true;
		gameObject.name = "small";
	}

	private float t = 0;
	private void Update()
	{
		//print (!isLocalPlayer);
		if (isServer) {
			playerCamera.SetActive (false);
			waterCameras.SetActive (false);
		}

		if (!isLocalPlayer) {
			camera.enabled = false;
			canvas.SetActive (false);
			listener.enabled = false;
			//bulletSpawn.SetActive (false);
			//bulletSpawn2.SetActive (false);
			return;
		}

		//Escape quits this game
		if (!isServer && Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.Q)) {
			if(!isServer){
				NetworkManager.singleton.StopClient();
				//Application.Quit (); //if the password is wrong, crash!
			}
		}
	
		Cursor.lockState = CursorLockMode.Locked;


		position = new Rect ((Screen.width - crosshair.width) / 2, (Screen.height - crosshair.height) / 2, crosshair.width, crosshair.height);

		//If the player is high up above the terrain, increase render distance
		if (gameObject.transform.position.y > 135f) {//current cutoff for high ground view is -20 [should see cities below and far into the ocean]
			camera.farClipPlane = 1500;
		} else {
			camera.farClipPlane = 800; //low ground [can't see far into the ocean]
		}

		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		int temp_index = FindInPlayers ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString ());

		//item0 is the compass
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			activeItem = Health.item0; 
			items.text = "*0* 1 2 3 4 5 6   compass";
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			activeItem = Health.item1;
			items.text = "0 *1* 2 3 4 5 6   " + Health.item1 [temp_index];
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			activeItem = Health.item2; 
			items.text = "0 1 *2* 3 4 5 6   " + Health.item2 [temp_index];
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			activeItem = Health.item3;
			items.text = "0 1 2 *3* 4 5 6   " + Health.item3 [temp_index];
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			activeItem = Health.item4;
			items.text = "0 1 2 3 *4* 5 6   " + Health.item4 [temp_index];
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			activeItem = Health.item5; 
			items.text = "0 1 2 3 4 *5* 6   " + Health.item5 [temp_index];
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			activeItem = Health.item6; 
			items.text = "0 1 2 3 4 5 *6*   " + Health.item6 [temp_index];
		}
		 
		LayerMask mask = LayerMask.GetMask ("PickUPS");

			//LERPING 
		if (Time.time > waitTime2 + 0.5f && gameObject.activeInHierarchy) {
			var Clones = GameObject.FindGameObjectsWithTag ("Light2");//Destroy all solid light
			for (int i = 0; i < Clones.Length; i++) {
				Destroy (Clones [i]);
			}
		}
		//change the top if and if below by opposite operation to adjust lerping stats
		if (Time.time > waitTime2 + 0.5f && gameObject.activeInHierarchy && Time.time < waitTime2 + 2) {
			lerp = true;
		} else {
			lerp = false;
		}

		if (lerp) {
			t += 0.01f;
			var Clones = GameObject.FindGameObjectsWithTag ("Light");
			for (int i = 0; i < Clones.Length; i++){
				Clones [i].GetComponent<Renderer> ().material.color = Color.Lerp (Color.white, Color.clear, t);//lerp the transparent light
																				//every frame the lerp gets 1% closer to 1. 0 -> 1, at 1 done lerping
			}
		}

		if(Time.time > waitTime2 + 1.25f && gameObject.activeInHierarchy){
			var Clones = GameObject.FindGameObjectsWithTag ("Light");
			for(int i = 0; i < Clones.Length; i++){
				Clones [i].transform.position += new Vector3 (0, 5, 0);

				if (Clones [i].transform.localScale.x > 0) {
					Clones [i].transform.localScale += new Vector3 (-0.05f, 0, -0.05f);
				}
			}
		}
	
		if(Time.time > waitTime2+5 && gameObject.activeInHierarchy){
			var Clones = GameObject.FindGameObjectsWithTag ("Light");
			for(int i = 0; i < Clones.Length; i++){
				Destroy (Clones [i]);
			}
		}
		//END OF LERPING
		//can't move during first 2 seconds
		if (Time.time < waitTime2 + 1) {
			return;
		} 

		if (Physics.Raycast (ray, out hit, 10f, mask)) {
			Transform objectHit = hit.transform;
//			print (hit.transform.name);
			if (hit.transform.tag == "PickUp" && Input.GetKeyDown (KeyCode.F) && (Health.item1 [temp_index] != hit.transform.name && Health.item2 [temp_index] != hit.transform.name && Health.item3 [temp_index] != hit.transform.name && Health.item4 [temp_index] != hit.transform.name && Health.item5 [temp_index] != hit.transform.name && Health.item6 [temp_index] != hit.transform.name)) {
				if (Health.item1 [temp_index] == null) {
					Health.item1 [temp_index] = hit.transform.name;
				} else if (Health.item2 [temp_index] == null) {
					Health.item2 [temp_index] = hit.transform.name;
				} else if (Health.item3 [temp_index] == null) {
					Health.item3 [temp_index] = hit.transform.name;
				} else if (Health.item4 [temp_index] == null) {
					Health.item4 [temp_index] = hit.transform.name;
				} else if (Health.item5 [temp_index] == null) {
					Health.item5 [temp_index] = hit.transform.name;
				} else if (Health.item6 [temp_index] == null) {
					Health.item6 [temp_index] = hit.transform.name;
				} else {
					if (activeItem != Health.item0) {
						//drop the item, send it to server. Name the instantiated thing activeItem[temp_index] and give it PickUp tag.
						//CmdDrop (activeItem [temp_index]);
						for(int j = 0; j < 100; j++){
							if (Health.extra [temp_index, j] == null) {
								Health.extra [temp_index, j] = activeItem [temp_index];
								print (Health.extra [temp_index, j]);
								break;
							}
						}
						activeItem [temp_index] = hit.transform.name;
					}
				}
				if (isClient) {
					CmdDestroy (hit.transform.gameObject);
				}
			}
		}


		if (gameObject.GetComponent<Collider> ().enabled == false) {
			return;
		}
		/* HAVE AN ARRAY OF "PLAYER CLASS" OBJECT, EACH PLAYER HAS THEIR NETID AS A MEMBER [gain points when picking up gems, this is a kill register]
		 * LOOP THROUGH THAT ARRAY, IF THE NET ID OF THIS GAMEOBJECT MATCHES THEN THAT IS THE PLAYER THAT IS SHOOTING
		 * 100 "VICTORY" POINTS TO WIN, GAIN FROM KILLING OPPONENTS [+1 bonus per kill every 3 kills in a life] AND STAYING ALIVE FOR A CERTAIN AMOUNT OF TIME
		 * -> EACH kill has a base worth of 3. Plus 1 for every 2 kills that player got and plus 1 for every 2 minutes that player was alive.
		 * Gain points equal to 2 times the amount of minutes survived, i.e. after 1 minute gain 2 points, after 2 minutes gain 4 points etc. */
		//THE ABOVE WILL NOT BE IMPLEMENTED, IT PROBABLY WILL NEVER AND SHOULD NEVER BE...

		string gun = activeItem[temp_index]; //what gun to use for the "fire" function. Default pistol
		string shooter; //who is shooting
//		Debug.Log(playerSize2[1]);
		bool tempBool = true;
		if (Input.GetKeyDown (KeyCode.Mouse0) && (gun == "pistol" || gun == "AR")) {
			//printPlayers ();
			for (int i = 0; i < Health.players.Length; i++) {
				if (Health.players [i] == ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString ())) {
					if (true /*Health.small_ammo [i] > 0*/) {
						Health.small_ammo [i] -= 1;
						AmmoText.text = "S" + Health.small_ammo [i].ToString (); 
						shooter = (gameObject.GetComponent<NetworkIdentity> ().netId).ToString ();
						CmdFire (bulletSpawn.transform.position - bulletSpawn2.transform.position, bulletSpawn.transform.position, bulletSpawn.transform.rotation, gun, shooter);
						break;
					}
				}
			}
		} else if (Input.GetKeyDown (KeyCode.Mouse0) && (gun == "Shotgun" || gun == "Rocket") && Time.time > waitTime && GetIndex() != 100 && playerSize2[GetIndex()] == "large") {
			//cooldown for shotgun is 1 seconds
			waitTime = Time.time + 1;
			for (int i = 0; i < Health.players.Length; i++) {
				if (Health.players [i] == ((gameObject.GetComponent<NetworkIdentity> ().netId).ToString ())) {
					if (true/*Health.small_ammo [i] > 0*/) {
						Health.small_ammo [i] -= 1;
						AmmoText.text = "S" + Health.small_ammo [i].ToString (); 
						shooter = (gameObject.GetComponent<NetworkIdentity> ().netId).ToString ();
						CmdFire (bulletSpawn.transform.position - bulletSpawn2.transform.position, bulletSpawn.transform.position, bulletSpawn.transform.rotation, gun, shooter);
						break;
					}
				}
			}
		}  else if (Input.GetKeyDown(KeyCode.Mouse0) && gun == "compass" && Time.time > waitTime){
			//cooldown for compass is 10 seconds
			waitTime = Time.time + 10;
			Drawline (transform.position, transform.rotation);
		}

		if (playerSize2[GetIndex()] == "large" && gameObject.GetComponent<MeshRenderer> ().enabled == true && Time.time > waitTime2 /*&& islocalplayer*/ ) {//this becomes true after 10 seconds
			CmdMeshLarge();//This disables the default for the large character, now do another if that makes a different mesh get rendered as "true"
		}

		if (playerSize2[GetIndex()] == "small" && gameObject.GetComponent<MeshRenderer> ().enabled == true && Time.time > waitTime2 /*&& isLocalplayer*/ ) {//this becomes true after 10 seconds
			CmdMeshSmall();//This disables the default for the large character, now do another if that makes a different mesh get rendered as "true"
		}

		//print (GameObject.FindGameObjectsWithTag ("Player").Length > numPlayers);
		if(GameObject.FindGameObjectsWithTag ("Player").Length > numPlayers){
		//THIS WORKS, JUST NEED AN IF CHECK SO THAT IT STOPS WHEN A PLAYER DIES
			if (isLocalPlayer && lifeGemNOTcritboxGem.activeInHierarchy == false) {
				if (playerSize2[GetIndex()] == "large") {
					CmdUpdateMeshLarge (); //send mesh to everyone so they see it right
				}
			} 

			/*if (isLocalPlayer && lifeGemNOTcritboxGem.activeInHierarchy == true) {
				if (playerSize2[GetIndex()] == "large") {
					CmdRemoveMeshLarge (); //send mesh to everyone so they see it right
				}
			} */

			//This print is attached to both players and does not work as each will print something different. Make it so these stop on player death. 
			//Something like a "die" function that sends a variable into here when a player dies so that it stops updating the models.

			if (isLocalPlayer && lifeGemNOTcritboxGem.activeInHierarchy == false) {
				if(playerSize2[GetIndex()] == "small"){
					CmdUpdateMeshSmall (); //send mesh to everyone so they see it right
				}
			}

			/*if (isLocalPlayer && lifeGemNOTcritboxGem.activeInHierarchy == true) {
				if(playerSize2[GetIndex()] == "small"){
					CmdRemoveMeshSmall (); //send mesh to everyone so they see it right
				}
			}*/
			numPlayers = GameObject.FindGameObjectsWithTag ("Player").Length;
		}

		//Camera Rotation THIS FOR SOME REASON DOESN'T SEEM TO BE NETWORKED
		y = Input.GetAxis("Mouse X");
		x = Input.GetAxis ("Mouse Y");

		rotateValue = new Vector3(x * (-1.0f)*(sensitivity), y * (sensitivity), 0);

		//print (transform.eulerAngles + "-" + rotateValue);
		if (rotationChecker.transform.eulerAngles.x < 340 && rotationChecker.transform.eulerAngles.x > 300) {
			rotationChecker.transform.eulerAngles = new Vector3(340, rotationChecker.transform.eulerAngles.y, rotationChecker.transform.eulerAngles.z);
		}
		if (rotationChecker.transform.eulerAngles.x > 45 && rotationChecker.transform.eulerAngles.x < 90) {
			rotationChecker.transform.eulerAngles = new Vector3(45, rotationChecker.transform.eulerAngles.y, rotationChecker.transform.eulerAngles.z);
		}
		rotationChecker.transform.eulerAngles = rotationChecker.transform.eulerAngles - rotateValue;
		//End of camera rotating



		//Diagonals -> It takes approxiamately 4 minutes to go from end to end horizontally of the map created. 
		//The time it takes implies that the map from the sand of the pennisula to the wall horizontally is about 2/3's of the Fortnite island.
		if(Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.A))){
			transform.position = transform.position + (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
			transform.position = transform.position - Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
		}     
		else if(Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.D))){
			transform.position = transform.position + (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
			transform.position = transform.position + Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
		}  
		else if(Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.A))){
			transform.position = transform.position - (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) *  (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
			transform.position = transform.position - Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.D))){
			transform.position = transform.position - (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) *  (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
			transform.position = transform.position + Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
		}
		//single key
		else if(Input.GetKey(KeyCode.W)){
			transform.position = transform.position + (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) * _speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.S)){
			transform.position = transform.position - (Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) *  _speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.A)){
			transform.position = transform.position - Camera.main.transform.right * _speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.D)){
			transform.position = transform.position + Camera.main.transform.right * _speed * Time.deltaTime;
		}



//		other moving
//		_rigidbody3d.AddForce(
//			NaturalMovement.MatchVelocityForce(
//				new Vector3(
//					(transform.right * Input.GetAxis("Horizontal")).x,
//					0.0f,
//					(transform.forward * Input.GetAxis("Vertical")).z) * _maxVelocity,
//				new Vector3(_rigidbody3d.velocity.x, 0.0f, _rigidbody3d.velocity.z),
//				_acceleration,
//				_maxAcceleration));

		//if the jump is higher than the max velocity then it equals the max
		if (_rigidbody3d.velocity.y > _maxVelocity) {
			_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, _maxVelocity, _rigidbody3d.velocity.z);
		}

		//If the player is floating then give it a small velocity so that the if will detect and prevent it.
		if (_rigidbody3d.velocity.y == 0) {
			_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, 0.003f, _rigidbody3d.velocity.z);
		}
		//previous jump controller || (_rigidbody3d.velocity.y >= 0 && _rigidbody3d.velocity.y <= 0.01)
		if (Input.GetKeyDown (KeyCode.Space)) {
			Vector3 dwn = transform.TransformDirection (Vector3.down);

			//raycasts are nice because they take care of the ramp problem, also use raycasts for realistic platforming!!!
			if (Physics.Raycast (transform.position, dwn, 1.5f)){
					//_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, _jumpVelocity, _rigidbody3d.velocity.z)

					//instead of the weird velocity thing just add an impulse
					//_rigidbody3d.AddForce (new Vector3 (0, _jumpVelocity, 0), ForceMode.Impulse);
				//THESE ARE 0 AND NEED TO BE FIXED, REMEMBER THE JUMPVELOCITY SHOULD INCREASE AS XY-PLANE MOVEMENT INCREASES
				//print(_rigidbody3d.velocity.x);                                            
				//print (_rigidbody3d.velocity.z);                       //CHANGE THIS VALUE (the 4f) FOR JUMP HEIGHT
				_rigidbody3d.velocity = (new Vector3 (_rigidbody3d.velocity.x, _jumpVelocity*4f, _rigidbody3d.velocity.z));

				} //else {
					//print ("There is nothing below the object!");
				//}
			}
		/*if (_rigidbody3d.velocity.y > -0.02 && _rigidbody3d.velocity.y <= 0.001) {
			_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, 0, _rigidbody3d.velocity.z);
			_rigidbody3d.useGravity = false;
		} else {
			_rigidbody3d.useGravity = true;
			//_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, 1, _rigidbody3d.velocity.z);
		}*/

		// Falling 
		if (_rigidbody3d.velocity.y < 0) {                             //CHANGE THIS VALUE FOR GRAVITY
			_rigidbody3d.velocity += Vector3.up * Physics.gravity.y * 1.5f*4f * Time.deltaTime;
		}
			
	}

	private void FixedUpdate()
	{
		_rigidbody3d.velocity = new Vector3(
			NaturalMovement.CapVelocity(new Vector3(_rigidbody3d.velocity.x, 0.0f, _rigidbody3d.velocity.z), _maxVelocity).x,
			_rigidbody3d.velocity.y, NaturalMovement.CapVelocity(new Vector3(_rigidbody3d.velocity.x, 0.0f, _rigidbody3d.velocity.z), _maxVelocity).z);
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

	[Command]
	void CmdFire(Vector3 Direction, Vector3 bulletPosition, Quaternion bulletRotation, string gun, string shooter) 
	{			
		//use the shooter passed into here to record who shot the bullet and who was hit other.GetComponent<NetworkIdentity>().netID is the other person
		if (gun == "pistol") {
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate (
				             bulletPrefab,
				             bulletPosition,
				             bulletRotation);

			bullet.name = shooter;//ONLY ON THE SERVER!!
			GiveBulletName (bullet, shooter); 

			//print ("bullet" + transform.name);
			//bullet.transform.parent = gameObject.transform;
			bullet.GetComponent<Rigidbody> ().velocity = Direction.normalized * 60;
			//print (rotationChecker.transform.rotation.eulerAngles); //position and rotation for each player can be tracked easily as data.
		
			NetworkServer.Spawn (bullet);

			// Destroy the bullet after 2 seconds
			StartCoroutine (despawnBullet (bullet));
		} else if (gun == "Shotgun") {
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate (
				ShotgunBulletPrefab,
				bulletPosition,
				bulletRotation);

			bullet.name = shooter;
			GiveBulletName (bullet, shooter); 

			//print ("bullet" + transform.name);
			//bullet.transform.parent = gameObject.transform;
			bullet.GetComponent<Rigidbody> ().velocity = Direction.normalized * 60;
			//print (rotationChecker.transform.rotation.eulerAngles); //position and rotation for each player can be tracked easily as data.

			NetworkServer.Spawn (bullet);

			// Destroy the bullet after 2 seconds
			StartCoroutine (despawnBullet (bullet));
		}
	}
		
	[Command]
	void CmdDestroy(GameObject obj){
		Destroy (obj);
	}

	void GiveBulletName(GameObject bullet, string shooter){
		bullet.name = shooter;
	}

	void Drawline (Vector3 bulletPosition, Quaternion bulletRotation){
		float disClosest = Mathf.Infinity;
		Health enemyClosest = null;
		Health[] allEnemies = GameObject.FindObjectsOfType<Health> ();

		foreach (Health currentEnemy in allEnemies) {
			float distanceTo = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			if (distanceTo < disClosest && distanceTo > 1.5f) {
				disClosest = distanceTo;
				enemyClosest = currentEnemy;
			}
		}
		 
		if (enemyClosest != null) {
			//if there is an enemy that can be tracked then... 
			StartCoroutine(spawnTrackingBullets (bulletPosition, bulletRotation, enemyClosest));
		}
	}

	IEnumerator spawnTrackingBullets(Vector3 bulletPosition, Quaternion bulletRotation, Health enemyClosest){
		int counter = 0;
		while (counter < 30) {
			var bullet = (GameObject)Instantiate (
				CompassBulletPrefab,
				bulletPosition,
				bulletRotation);

			Vector3 directionTo = enemyClosest.transform.position - transform.position;
			bullet.GetComponent<Rigidbody> ().velocity = directionTo.normalized * 20;

			//Instantiate (bullet);

			StartCoroutine (despawnNonServerBullet (bullet));

			//because counter += 3 was not working... 
			counter++;
			counter++;
			counter++;

			yield return new WaitForSeconds (0.25f);
		}
	}

	IEnumerator despawnBullet(GameObject bullet){
		yield return new WaitForSeconds (2.0f);
		NetworkServer.Destroy (bullet);
	}

	int GetIndex(){
		for (int i = 0; i < Health.players.Length; i++) {
			if (Health.players [i] == (gameObject.GetComponent<NetworkIdentity> ().netId).ToString ()) {
				return i;
			}
		}
		return 100;
	}

	IEnumerator despawnNonServerBullet(GameObject bullet){
		yield return new WaitForSeconds (2.5f);
		Destroy (bullet);
	}
	//public override void OnStartLocalPlayer()
	//{
	//	GetComponent<MeshRenderer>().material.color = Color.blue;
	//}
}