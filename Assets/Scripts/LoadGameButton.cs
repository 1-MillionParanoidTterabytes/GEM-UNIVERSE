using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadGameButton : MonoBehaviour {

	[SerializeField]
	private GameObject a;
	public Boolean1 script;

	void Start()
	{
		script = a.GetComponent<Boolean1> ();
	}

	public void LoadByIndex(int sceneIndex)
	{
		if (script.A) 
		{
			SceneManager.LoadScene (sceneIndex);
		}
	}
}