using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour 
{	
	[SerializeField]
	private Text timerText;
	private float startTime;

	public static float scoreVal = 0.0f;
	public static string score = "null";
	public static int scoreinsec = 0;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float t = Time.time - startTime;

		string hours = ((int)t / 3600).ToString("00");
		string minutes = ((int)t / 60).ToString("00");
		string seconds = ((int)t % 60).ToString("00");

		timerText.text = hours + ":" + minutes + ":" + seconds;
		score = timerText.text;
		scoreinsec = (int)t;
		scoreVal = t;
	}
}
