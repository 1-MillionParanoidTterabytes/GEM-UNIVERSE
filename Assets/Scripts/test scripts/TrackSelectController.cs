using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackSelectController : MonoBehaviour {

	[SerializeField]
	private GameObject m1;
	[SerializeField]
	private GameObject m2;
	[SerializeField]
	private Button b1;

	void Start(){
		print(CharacterSelectController.character);
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
				SceneManager.LoadScene (1);
			}
			if (m2.activeInHierarchy) {
				SceneManager.LoadScene (2);
			}
		}
	}
}