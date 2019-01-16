using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounterWinTracker : MonoBehaviour {

	[SerializeField]
	private Text winText;
	[SerializeField]
	private Text winText2;
	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text scoreText2;

	private int players = 0;

	//Used to count the number of players on the stage
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			players++;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			players--;
		}
	}

	private void Update()
	{
		if(players >= 10){
			winText.text = name + " has won!";
			winText2.text = name + " has won!";
		}

		scoreText.text = name + ":   " + players.ToString ();
		scoreText2.text = name + ":   " + players.ToString (); 
	}
}
