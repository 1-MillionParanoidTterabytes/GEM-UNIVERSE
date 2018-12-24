using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{

	//private Vector3 rotateValue;
	[SerializeField]
	private GameObject player;
	private Vector3 offset;

	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}

	//causes rotation that is good for first person view
	//	void Update()
	//	{
	//		y = Input.GetAxis("RotateY");
	//		rotateValue = new Vector3(0, y * -1, 0);
	//		transform.eulerAngles = transform.eulerAngles - rotateValue;
	//	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = player.transform.position + offset;
	}
}