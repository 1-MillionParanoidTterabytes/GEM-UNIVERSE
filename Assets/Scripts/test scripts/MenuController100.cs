using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController100 : MonoBehaviour {

	string name="";
	int score=0;
	List<Scores2> highscore;

	// Use this for initialization
	void Start () {
		//EventManager._instance._buttonClick += ButtonClicked;

		highscore = new List<Scores2>();

		highscore = HighScoreManager100._instance.GetHighScore();   

		name = InputSaver.username;

		score = Timer.scoreinsec;
		HighScoreManager100._instance.SaveHighScore(name,score);
		highscore = HighScoreManager100._instance.GetHighScore();    
	}


	void ButtonClicked(GameObject _obj)
	{
		print("Clicked button:"+_obj.name);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI()
	{
		//Buttons for input name and score then add those to the high scores list.
		/*
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name :");
		name =  GUILayout.TextField(name);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Score :");
		score =  GUILayout.TextField(score);
		GUILayout.EndHorizontal();

		if(GUILayout.Button("Add Score"))
		{
			HighScoreManager._instance.SaveHighScore(name,System.Int32.Parse(score));
			highscore = HighScoreManager._instance.GetHighScore();    
		}
		*/

		/*
		if(GUILayout.Button("Get LeaderBoard"))
		{
			highscore = HighScoreManager._instance.GetHighScore();            
		}
		*/

		//Clears the leaderboard, remember to comment this out if the game is placed in a public place.
		if(GUILayout.Button("Clear Leaderboard"))
		{
			HighScoreManager100._instance.ClearLeaderBoard();            
		}

		foreach(Scores2 _score in highscore)
		{
			GUILayout.BeginHorizontal ();
			int t = _score.score;

			string hours = ((int)t / 3600).ToString ("00");
			string minutes = ((int)t / 60).ToString ("00");
			string seconds = ((int)t % 60).ToString ("00");

			string timeinhours = hours + ":" + minutes + ":" + seconds;
			GUILayout.Label ("\t" + _score.name + "\t" + timeinhours);
			GUILayout.EndHorizontal ();
		}
	}
}