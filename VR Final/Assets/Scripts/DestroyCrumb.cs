using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrumb : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Creature" || coll.gameObject.name == "WaterGround") {
			Destroy (gameObject);
 			MoveToCrumb.crumbs.Remove (gameObject);
 		}
	}
}
