using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootReference : MonoBehaviour {

	public GameObject root;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos = root.transform.position;

		transform.position = pos;
		
	}
}
