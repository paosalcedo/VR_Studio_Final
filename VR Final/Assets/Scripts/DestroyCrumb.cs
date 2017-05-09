using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrumb : MonoBehaviour {
	GameObject bug;
	public float distToBug;
	// Use this for initialization
	void Start () {
		bug = GameObject.Find("BugPrefab");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Vector3.Distance (transform.position, bug.transform.position) <= distToBug) {
			Destroy(gameObject);
	 		MoveToCrumb.crumbs.Remove (gameObject);
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Creature" || coll.gameObject.name == "WaterGround") {
			Destroy (gameObject);
 			MoveToCrumb.crumbs.Remove (gameObject);
 		}
	}
}
