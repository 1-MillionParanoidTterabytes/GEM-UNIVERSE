using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	Rigidbody rb;
	private Quaternion _rotation2 = Quaternion.Euler (10, -90, 0);
	private Quaternion _rotation_start = Quaternion.Euler (10, 90, 0);

	private bool autoAccel = true;
	private int AbilityPoint; 
	private bool[] check_passed;
	private bool allcheck_passed = false;
	private int printCounter = 0; 

	[SerializeField]
	private GameObject waterRespawn;
	[SerializeField]
	private GameObject respawn1;
	[SerializeField] 
	private GameObject checkpoint1; 

	[SerializeField]
	private GameObject _camera;
	private Quaternion _rotation = Quaternion.Euler (40, 90, 0);
	private Quaternion original_rotation = Quaternion.Euler (10,90,0);
	private float temp = 10.0f; 


	[SerializeField]
	private float _speed; 

	private Vector3 rotateValue;
	private float y;
	private int carDirection = 1;
	[SerializeField]
	private float MAX_SPEED;

	[SerializeField]
	private int DECELERATE;

	[SerializeField]
	private int ACCELERATE;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		check_passed = new bool[5];
		for (int i = 0; i < check_passed.Length; i++){
			check_passed[i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () {	
		for (int i = 0; i < check_passed.Length; i++){
			if (check_passed[i] == false) {
				allcheck_passed = false;
				break;
			} else {
				allcheck_passed = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			if (autoAccel == true) {
				autoAccel = false; 
				print (printCounter + " autoAccel toggled OFF");
				printCounter++;
			} else {
				autoAccel = true;
				print (printCounter + " autoAccel toggled ON");
				printCounter++;
			}
		}

		//check if rear view is being used 
		if (Input.GetKeyDown (KeyCode.Space)) {
			carDirection = -1;
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			carDirection = 1;
		}

		//If the statement below is true then the car will not really move forwards easily 
		if (true) {
			//no bonus speed for moving diagonally
			//The extra if statements make it so that if the magnitude is not 0.99 it becomes that...
			//REMEMBER -> to slow the car change the SPEED not this. This just means that speed*0.999 creates the vector for movement
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
				if ((Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))).magnitude < 0.8f) {
					transform.position = transform.position + carDirection * (Camera.main.transform.forward * 2 - (new Vector3 (0f, Camera.main.transform.forward.y*2, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				} else {
					transform.position = transform.position + carDirection * (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * (_speed / Mathf.Sqrt (2)) * Time.deltaTime;
				}
			} else {
				if ((Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))).magnitude < 0.8f) {
					transform.position = transform.position + carDirection * (Camera.main.transform.forward * 2 - (new Vector3 (0f, Camera.main.transform.forward.y*2, 0f))) * _speed * Time.deltaTime;
				} else {
					transform.position = transform.position + carDirection * (Camera.main.transform.forward - (new Vector3 (0f, Camera.main.transform.forward.y, 0f))) * _speed * Time.deltaTime;
				}
			}
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
				transform.position = transform.position - carDirection*(Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) *  (_speed/Mathf.Sqrt(2)) * Time.deltaTime;
			} else {
			transform.position = transform.position - carDirection*(Camera.main.transform.forward - (new Vector3(0f, Camera.main.transform.forward.y, 0f))) *  _speed * Time.deltaTime;
			}
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position = transform.position - carDirection*Camera.main.transform.right * _speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.position = transform.position + carDirection*Camera.main.transform.right * _speed * Time.deltaTime;
		}
		if ((Input.GetKey("z") || autoAccel) && _speed < MAX_SPEED)
		{
			if (_speed < MAX_SPEED) {
				_speed += ACCELERATE * Time.deltaTime;
			}
				
		}
		else 
		{
			if (_speed > MAX_SPEED) {
				_speed -= (DECELERATE * Time.deltaTime)*3;
			}
			_speed -= DECELERATE * Time.deltaTime;
			if (_speed < 0.0f)
			{
				_speed = 0.0f;
			}
		} 
	}

	void OnTriggerStay(Collider other)
	{
		//SLOW DOWN IF NOT ON TRACK
		//if (collision.collider.name != "Cube" || collision.collider.name != "Main Camera") {
			//_speed--;
		//}
		if (other.name == "WaterBasicNightime") {
			transform.position = waterRespawn.transform.position;
		}
		if (other.name == "rotate") {
			temp += 0.5f;
			if (temp > 35) {
				temp = 35;
			}
			_rotation = Quaternion.Euler (temp, 0, 0);
			Quaternion _self_rotate = Quaternion.Euler (temp-10, 90, 0);

			_camera.transform.localRotation = _rotation;
			transform.rotation = _self_rotate;
		} else if (other.name == "unrotate"){
			temp -= 0.5f;
			if (temp < 10) {
				temp = 10;
			}
			_rotation = Quaternion.Euler (temp, 0, 0);
			Quaternion _self_rotate = Quaternion.Euler (temp, 90, 0);

			_camera.transform.localRotation = _rotation;
			transform.rotation = _self_rotate;
		}
		if (other.tag == "OB1") {
			transform.position = respawn1.transform.position;
		}

		//checkpoint checking
		if (other.name == "check1") {
			check_passed [0] = true;
			print ("CHECK1 PASSED");
		}
		if (other.name == "check2") {
			check_passed [1] = true;
			print ("CHECK2 PASSED");
		}
		if (other.name == "check3") {
			check_passed [2] = true;
			print ("CHECK3 PASSED");
		}
		if (other.name == "check4") {
			check_passed [3] = true;
			print ("CHECK4 PASSED");
		}
		if (other.name == "check5") {
			check_passed [4] = true;
			print ("CHECK5 PASSED");
		}
		if (other.name == "finish" && allcheck_passed == true) {
			print ("FINISHED");
		}

		if (other.tag == "Boost") {
			_speed = 15;
		} 
		if (other.tag == "Item") {
			AbilityPoint++;
			print (AbilityPoint);
		}
	}
}
