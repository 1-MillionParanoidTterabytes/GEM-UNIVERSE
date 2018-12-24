using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

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
}
