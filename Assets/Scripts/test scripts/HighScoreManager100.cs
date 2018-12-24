using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager100 : MonoBehaviour
{

	private static HighScoreManager100 m_instance;
	private const int LeaderboardLength = 100;

	public static HighScoreManager100 _instance {
		get {
			if (m_instance == null) {
				m_instance = new GameObject ("HighScoreManager100").AddComponent<HighScoreManager100> ();                
			}
			return m_instance;
		}
	}

	void Awake ()
	{
		if (m_instance == null) {
			m_instance = this;            
		} else if (m_instance != this)        
			Destroy (gameObject);    

		DontDestroyOnLoad (gameObject);
	}

	public void SaveHighScore (string name, int score)
	{
		//list of scores class object
		List<Scores2> HighScores = new List<Scores2> ();

		int i = 1;
		while (i<=LeaderboardLength && PlayerPrefs.HasKey("HighScore"+i+"score")) {
			Scores2 temp = new Scores2 ();
			temp.score = PlayerPrefs.GetInt ("HighScore" + i + "score");
			temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
			HighScores.Add (temp);
			i++;
		}
		if (HighScores.Count == 0) {            
			Scores2 _temp = new Scores2 ();
			_temp.name = name;
			_temp.score = score;
			HighScores.Add (_temp);
		} else {
			for (i=1; i<=HighScores.Count && i<=LeaderboardLength; i++) {
				if (score > HighScores [i - 1].score) {
					Scores2 _temp = new Scores2 ();
					_temp.name = name;
					_temp.score = score;
					HighScores.Insert (i - 1, _temp);
					break;
				}            
				if (i == HighScores.Count && i < LeaderboardLength) {
					Scores2 _temp = new Scores2 ();
					_temp.name = name;
					_temp.score = score;
					HighScores.Add (_temp);
					break;
				}
			}
		}

		i = 1;
		while (i<=LeaderboardLength && i<=HighScores.Count) {
			PlayerPrefs.SetString ("HighScore" + i + "name", HighScores [i - 1].name);
			PlayerPrefs.SetInt ("HighScore" + i + "score", HighScores [i - 1].score);
			i++;
		}

	}

	public List<Scores2>  GetHighScore ()
	{
		List<Scores2> HighScores = new List<Scores2> ();

		int i = 1;
		while (i<=LeaderboardLength && PlayerPrefs.HasKey("HighScore"+i+"score")) {
			Scores2 temp = new Scores2 ();
			temp.score = PlayerPrefs.GetInt ("HighScore" + i + "score");
			temp.name = PlayerPrefs.GetString ("HighScore" + i + "name");
			HighScores.Add (temp);
			i++;
		}

		return HighScores;
	}

	public void ClearLeaderBoard ()
	{
		//for(int i=0;i<HighScores.
		List<Scores2> HighScores = GetHighScore();

		for(int i=1;i<=HighScores.Count;i++)
		{
			PlayerPrefs.DeleteKey("HighScore" + i + "name");
			PlayerPrefs.DeleteKey("HighScore" + i + "score");
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
}

public class Scores2
{
	public int score;
	public string name;

}