using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolean1 : MonoBehaviour {

	public bool A = false;

	public void SetBool(){
		SetVal ();
	}

	public bool SetVal(){
		A = true;
		return A;
	}
}
