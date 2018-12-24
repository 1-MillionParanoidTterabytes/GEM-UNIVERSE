using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSaver : MonoBehaviour {
	[SerializeField]
	private InputField nameInput;
	public static string username;

	//used to save the name entered at the start menu
	void Update () {
		{
			username = nameInput.text;
		}
	}
}