using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataToSendToGame : MonoBehaviour {

	public static string size; // small medium large or special?
	public static string critbox; //where the Gem is 
	public static int thickness; //how obese character is
	public static string color; //base color 
	//public static string outfit; this don't worry about until later
	public static string username; 
	public static string password;

	void Start(){
		if (username != null) { //If your password is wrong, send you back to menu and quit your game
			Application.Quit ();
		}
	}
}
