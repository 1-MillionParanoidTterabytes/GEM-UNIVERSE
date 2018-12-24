using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
	[SerializeField]
	private GameObject path;

	private List<Transform> nodes;
	private int currentNode = 0;

	[SerializeField]
	private float speed; 

	// Use this for initialization
	void Start () {
		Transform[] pathTransforms = path.GetComponentsInChildren<Transform> ();
		nodes = new List<Transform>();

		for (int i = 0; i < pathTransforms.Length; i++){
			if(pathTransforms[i] != path.transform){
				nodes.Add(pathTransforms[i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = nodes [currentNode].position - transform.position;

		transform.position = Vector3.MoveTowards (transform.position, nodes [currentNode].position, Time.deltaTime*speed);

		if (dir.magnitude <= 0.5f) {
			currentNode++;
		
		}

		if (currentNode >= nodes.Count) {
			currentNode = 0;
		}
	}
}
