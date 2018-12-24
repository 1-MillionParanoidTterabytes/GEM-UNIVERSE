using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string x = Timer.score;
		print (x);
		print (Timer.scoreVal);
	}
	
	float MaxValue(float[] intArray)
	{
		var max = intArray[0];
		int i = 1;
		for (i = 1; i < intArray.Length; i++) 
		{
			if (intArray[i] > max)
			{
				max = intArray[i];
			}
		}
		return max;
	}
}