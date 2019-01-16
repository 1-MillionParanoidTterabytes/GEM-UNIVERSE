using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NaturalMovementPlatforming : NetworkBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private GameObject bulletSpawn;
	[SerializeField]
	private GameObject bulletSpawn2;
	[SerializeField]
	private Camera camera;
	[SerializeField]
	private GameObject rotationChecker;
	[SerializeField]
	private AudioSource source;
	[SerializeField]
	private AudioListener listener;

	[Header("Movement")]
	[SerializeField]
	private float _acceleration;
	[SerializeField]
	private float _maxAcceleration;
	[SerializeField]
	private float _maxVelocity;

	[Header("MoveSpeed")]
	[SerializeField]
	private float _speed; //default speed, the 'normal' good speed is 12. Now it is at 20 for ease of testing
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
	private string instrument = "";
	private int[] flags = new int[22];
	private float waitTime = 0.5f;
	float time = 0.0f;
	bool octave = false;

	private void Awake()
	{
		_rigidbody3d = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		sensitivity = -1.5f;

	}
				
	private void Update()
	{
		if (!isLocalPlayer)
		{
			camera.enabled = false;
			listener.enabled = false;
			return;
		}
			
		//Shooting code function for left mouse button
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			CmdFire (bulletSpawn.transform.position - bulletSpawn2.transform.position, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
		}

		//If both shifts are not being pressed then make all flags off and stop notes.
		if (!(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
			for (int i = 0; i < 20; i++) {
				flags [i] = 0;
			}
		}

		//While holding shift and near a piano you can play piano notes with q->] and 1->10
		if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
			if (Input.GetKey (KeyCode.RightBracket)) {
				flags [0] = 1;
			}
			if (Input.GetKeyUp (KeyCode.RightBracket)) {
				flags [0] = 0;
			}

			if(Input.GetKey(KeyCode.Equals)){
				flags [1] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Equals)) {
				flags [1] = 0;
			}

			if (Input.GetKey (KeyCode.LeftBracket)) {
				flags [2] = 1;
			}
			if (Input.GetKeyUp (KeyCode.LeftBracket)) {
				flags [2] = 0;
			}

			if(Input.GetKey(KeyCode.Minus)){
				flags [3] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Minus)) {
				flags [3] = 0;
			}

			if (Input.GetKey (KeyCode.P)) {
				flags [4] = 1;
			}
			if (Input.GetKeyUp (KeyCode.P)) {
				flags [4] = 0;
			}

			if (Input.GetKey (KeyCode.O)) {
				flags [5] = 1;
			}
			if (Input.GetKeyUp (KeyCode.O)) {
				flags [5] = 0;
			}

			if(Input.GetKey(KeyCode.Alpha9)){
				flags [6] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Alpha9)) {
				flags [6] = 0;
			}

			if (Input.GetKey (KeyCode.I)) {
				flags [7] = 1;
			}
			if (Input.GetKeyUp (KeyCode.I)) {
				flags [7] = 0;
			}

			if(Input.GetKey(KeyCode.Alpha8)){
				flags [8] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Alpha8)) {
				flags [8] = 0;
			}

			if (Input.GetKey (KeyCode.U)) {
				flags [9] = 1;
			}
			if (Input.GetKeyUp (KeyCode.U)) {
				flags [9] = 0;
			}

			if (Input.GetKey (KeyCode.Y)) {
				flags [10] = 1;
			} 
			if (Input.GetKeyUp (KeyCode.Y)) {
				flags [10] = 0;
			}

			if(Input.GetKey(KeyCode.Alpha6)){
				flags [11] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Alpha6)) {
				flags [11] = 0;
			}

			if (Input.GetKey (KeyCode.T)) {
				flags [12] = 1;
			}
			if (Input.GetKeyUp (KeyCode.T)) {
				flags [12] = 0;
			}

			if(Input.GetKey(KeyCode.Alpha5)){
				flags [13] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Alpha5)) {
				flags [13] = 0;
			}

			if (Input.GetKey (KeyCode.R)) {
				flags [14] = 1;
			}
			if (Input.GetKeyUp (KeyCode.R)) {
				flags [14] = 0;
			}

			if(Input.GetKey(KeyCode.Alpha4)){
				flags [15] = 1;
			}
			if (Input.GetKeyUp (KeyCode.Alpha4)) {
				flags [15] = 0;
			}

			if (Input.GetKey (KeyCode.E)) {
				flags [16] = 1;
			} 
			if(Input.GetKeyUp(KeyCode.E)){
				flags [16] = 0;
			}

			if (Input.GetKey (KeyCode.W)) {
				flags [17] = 1;
			} 
			if(Input.GetKeyUp(KeyCode.W)){
				flags [17] = 0;
			}

			if (Input.GetKey (KeyCode.Alpha2)) {
				flags [18] = 1;
			} 
			if(Input.GetKeyUp(KeyCode.Alpha2)){
				flags [18] = 0;
			}

			if (Input.GetKey (KeyCode.Q)) {
				flags [19] = 1;
			}
			if (Input.GetKeyUp(KeyCode.Q)){
				flags [19] = 0;
			}
				
			if (Input.GetKeyDown (KeyCode.BackQuote)) {
				if (octave == true) {
					octave = false;
				} else {
					octave = true;
				}
			}
		}

		time += Time.deltaTime;
		if (time > waitTime) {
			waitTime = time + 0.05f;
			if (flags [19] == 1) {
				CmdMusic (62, octave, instrument);
			}
			if (flags [18] == 1) {
				CmdMusic (63, octave, instrument);
			}
			if (flags [17] == 1) {
				CmdMusic (64, octave, instrument);
			}
			if (flags [16] == 1) {
				CmdMusic (65, octave, instrument);
			}
			if (flags [15] == 1) {
				CmdMusic (66, octave, instrument);
			}
			if (flags [14] == 1) {
				CmdMusic (67, octave, instrument);
			}
			if (flags [13] == 1) {
				CmdMusic (68, octave, instrument);
			}
			if (flags [12] == 1) {
				CmdMusic (69, octave, instrument);
			}
			if (flags [11] == 1) {
				CmdMusic (70, octave, instrument);
			}
			if (flags [10] == 1) {
				CmdMusic (71, octave, instrument);
			}
			if (flags [9] == 1) {
				CmdMusic (72, octave, instrument);
			}
			if (flags [8] == 1) {
				CmdMusic (73, octave, instrument);
			}
			if (flags [7] == 1) {
				CmdMusic (74, octave, instrument);
			}
			if (flags [6] == 1) {
				CmdMusic (75, octave, instrument);
			}
			if (flags [5] == 1) {
				CmdMusic (76, octave, instrument);
			}
			if (flags [4] == 1) {
				CmdMusic (77, octave, instrument);
			}
			if (flags [3] == 1) {
				CmdMusic (78, octave, instrument);
			}
			if (flags [2] == 1) {
				CmdMusic (79, octave, instrument);
			}
			if (flags [1] == 1) {
				CmdMusic (80, octave, instrument);
			}
			if (flags [0] == 1) {
				CmdMusic (81, octave, instrument);
			}
		}

		//camera rotation code
		y = Input.GetAxis ("Mouse X");
		x = Input.GetAxis ("Mouse Y");

		rotateValue = new Vector3(x * (-1.0f)*(sensitivity), y * (sensitivity), 0);

		if (rotationChecker.transform.eulerAngles.x < 340 && rotationChecker.transform.eulerAngles.x > 300) {
			rotationChecker.transform.eulerAngles = new Vector3(340, rotationChecker.transform.eulerAngles.y, rotationChecker.transform.eulerAngles.z);
		}
		if (rotationChecker.transform.eulerAngles.x > 45 && rotationChecker.transform.eulerAngles.x < 90) {
			rotationChecker.transform.eulerAngles = new Vector3(45, rotationChecker.transform.eulerAngles.y, rotationChecker.transform.eulerAngles.z);
		}
		rotationChecker.transform.eulerAngles = rotationChecker.transform.eulerAngles - rotateValue;
		//End of camera rotating



		//Movement code, makes sure moving diagonally is not faster than one key pressed down. "Normalization of vectors" 
		if (!Input.GetKey (KeyCode.RightShift)) {
			if (!Input.GetKey (KeyCode.LeftShift)) {
				if (Input.GetKey (KeyCode.W) && (Input.GetKey (KeyCode.A))) {
					transform.position = transform.position + (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
					transform.position = transform.position - Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.W) && (Input.GetKey (KeyCode.D))) {
					transform.position = transform.position + (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
					transform.position = transform.position + Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.S) && (Input.GetKey (KeyCode.A))) {
					transform.position = transform.position - (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
					transform.position = transform.position - Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.S) && (Input.GetKey (KeyCode.A))) {
					transform.position = transform.position - (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
					transform.position = transform.position + Camera.main.transform.right * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				}
		//single key
		else if (Input.GetKey (KeyCode.W)) {
					transform.position = transform.position + (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * _speed * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.S)) {
					transform.position = transform.position - (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * _speed * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.A)) {
					transform.position = transform.position - Camera.main.transform.right * _speed * Time.deltaTime;
				} else if (Input.GetKey (KeyCode.D)) {
					transform.position = transform.position + Camera.main.transform.right * _speed * Time.deltaTime;
				}
			}
		}

		//if the jump is higher than the max velocity then it equals the max
		if (_rigidbody3d.velocity.y > _maxVelocity) {
			_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, _maxVelocity, _rigidbody3d.velocity.z);
		}

		//If the player is floating then give it a small velocity so that the if will detect and prevent it.
		if (_rigidbody3d.velocity.y == 0) {
			_rigidbody3d.velocity = new Vector3 (_rigidbody3d.velocity.x, 0.003f, _rigidbody3d.velocity.z);
		}

		//jumping code
		if (Input.GetKeyDown (KeyCode.Space)) {
			Vector3 dwn = transform.TransformDirection (Vector3.down);

			//raycasts are nice because they take care of the ramp problem, also use raycasts for realistic platforming!!!
			if (Physics.Raycast (transform.position, dwn, 1.5f)){
				_rigidbody3d.velocity = (new Vector3 (_rigidbody3d.velocity.x, _jumpVelocity, _rigidbody3d.velocity.z));
			} 
		}

		// Falling, makes it realistic... instead of 'floaty'
		if (_rigidbody3d.velocity.y < 0) {
			_rigidbody3d.velocity += Vector3.up * Physics.gravity.y * 1.5f * Time.deltaTime;
		}
			
	}

	private void FixedUpdate()
	{
		_rigidbody3d.velocity = new Vector3(
			NaturalMovement.CapVelocity(new Vector3(_rigidbody3d.velocity.x, 0.0f, _rigidbody3d.velocity.z), _maxVelocity).x,
			_rigidbody3d.velocity.y, NaturalMovement.CapVelocity(new Vector3(_rigidbody3d.velocity.x, 0.0f, _rigidbody3d.velocity.z), _maxVelocity).z);
	}

	[Command]
	void CmdFire(Vector3 Direction, Vector3 bulletPosition, Quaternion bulletRotation) //Most of this function was stolen right from the Unity tutorial pages!
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletPosition,
			bulletRotation);

		// Add velocity to the bullet and make the sound
		bullet.GetComponent<Rigidbody>().velocity = Direction.normalized * 60;
		RpcMakeSound();

		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	//piano function linking thing
	[Command]
	void CmdMusic(int value, bool IsOctaveLower, string instrument){
		if (IsOctaveLower) {
			RpcMakeLowerNote (value-24, instrument);
		} else {
			RpcMakeNote (value, instrument);
		}
	}

	[ClientRpc]
	void RpcMakeLowerNote(int value, string instrument){
		if (instrument == "Piano") {
			GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			BeeThree piano => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 1.2);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => piano.freq;
   				velocity => piano.noteOn;
			
			}}
			", value));
			//NO lowered brass, because it doesn't seem to go lower than 8 notes.
		} else if (instrument == "String") {
			GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			Mandolin mando => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 1.2);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => mando.freq;
   				velocity => mando.noteOn;
			
			}}
			", value));
		}
	}

	[ClientRpc]
	void RpcMakeNote(int value, string instrument){
		if (instrument == "Piano") {
			if (value < 70) {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			BeeThree piano => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 1.2);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => piano.freq;
   				velocity => piano.noteOn;
			
			}}
			", value));
			} else {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			BeeThree piano => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 0.9);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => piano.freq;
   				velocity => piano.noteOn;
			
			}}
			", value));
			}
		} else if (instrument == "Brass") {
			if (value < 70) {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			Brass brass => JCRev r => dac;
			1.75 => r.gain;
			0.05 => r.mix;

			0.1 => brass.lip;			
			1 => brass.vibratoFreq;
			0.1 => brass.vibratoGain;
			0.25 => brass.volume;
			1 => brass.slide;

			while(true){{
				spork ~ play({0}+7, 1.2);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => brass.freq;
   				velocity => brass.noteOn;
			
			}}
			", value));
			} else {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			Brass brass => JCRev r => dac;
			1.75 => r.gain;
			0.05 => r.mix;

			0.1 => brass.lip;			
			1 => brass.vibratoFreq;
			0.1 => brass.vibratoGain;
			0.25 => brass.volume;
			1 => brass.slide;

			while(true){{
				spork ~ play({0}+7, 1.5);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => brass.freq;
   				velocity => brass.noteOn;
			
			}}
			", value));
			}
		} else if (instrument == "String") { 
			if (value < 70) {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			Mandolin mando => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 1.2);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => mando.freq;
   				velocity => mando.noteOn;
			
			}}
			", value));
			} else {
				GetComponent<ChuckSubInstance> ().RunCode (string.Format (@"
			Mandolin mando => JCRev r => dac;

			while(true){{
				spork ~ play({0}, 0.9);
				500::ms => now;
				break;
			}}

			fun void play( float note, float velocity )
			{{
    			Std.mtof( note ) => mando.freq;
   				velocity => mando.noteOn;
			
			}}
			", value));
			}
		}
	}

	[ClientRpc] //play the shooting sound
	void RpcMakeSound(){
		source.Play (0);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Piano") {
			instrument = "Piano";
		}
		if (other.tag == "Brass") {
			instrument = "Brass";
		}
		if (other.tag == "String") {
			instrument = "String";
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Piano") {
			instrument = "";
		}
		if (other.tag == "Brass") {
			instrument = "";
		}
		if (other.tag == "String") {
			instrument = "";
		}
	}

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}