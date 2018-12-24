using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour {

	[SerializeField]
	private GameObject m1;
	[SerializeField]
	private GameObject m2;
	[SerializeField]
	private Button b1;

	public static string character = "null";

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
				SceneManager.LoadScene (4);
				character = "red";
			}
			if (m2.activeInHierarchy) {
				SceneManager.LoadScene (4);
				character = "blue";
			}
		}
	}
}