﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

	public InputField usernameInput;
	public InputField passwordInput;

	public void LoadByIndex(int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
	}
	public void SetSmall(int pointless)
	{
		DataToSendToGame.size = "small";
	}
	public void SetMedium(int pointless)
	{
		DataToSendToGame.size = "medium";
	}
	public void SetLarge(int pointless)
	{
		DataToSendToGame.size = "large";
	}

	public void SetUsername(int pointless)
	{
		DataToSendToGame.username = (usernameInput.text).Replace("$", "_");//Usernames cannot have $ signs
		//print (DataToSendToGame.username);
	}

	public void SetPassword(int pointlesser)
	{
		DataToSendToGame.password = (passwordInput.text+"$");
	}
}
