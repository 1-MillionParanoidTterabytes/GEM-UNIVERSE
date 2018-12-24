using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	[SerializeField]
	private GameObject text1;
	[SerializeField]
	private GameObject text2;
	private string x = "";

	void Start ()
	{
		StartCoroutine (DataStuff ());
	}

	IEnumerator DataStuff()
	{
		string x = Timer.score;
		text2.GetComponent<Text> ().text = x;
		text1.GetComponent<Text> ().text = "GAME OVER!";
		yield return new WaitForSeconds(2);
		text1.GetComponent<Text> ().text = "";
	}
}
