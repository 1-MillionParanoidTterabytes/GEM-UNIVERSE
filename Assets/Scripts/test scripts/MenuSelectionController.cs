using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelectionController : MonoBehaviour {

	[SerializeField]
	private GameObject m1;
	[SerializeField]
	private GameObject m2;
	[SerializeField]
	private Button b1;

	void Start(){
		m2.gameObject.SetActive (false);
	}

	int numpress = 0; 

	void Update(){
		if ((Input.GetKeyDown (KeyCode.S)) && (numpress % 2 == 0)) {
			m1.gameObject.SetActive (true);
			m2.gameObject.SetActive (false);
			numpress++;
		} else if ((Input.GetKeyDown (KeyCode.S)) && (numpress % 2 != 0)) {
			m1.gameObject.SetActive (false);
			m2.gameObject.SetActive (true);
			numpress++;
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			if (m1.activeInHierarchy) {
				SceneManager.LoadScene (3);
			}
			if (m2.activeInHierarchy) {
				SceneManager.LoadScene (4);
			}
		}
	}
}