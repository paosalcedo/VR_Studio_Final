using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrumb : MonoBehaviour {


	Animator anim;
	GameObject bug;
	public float distToBug;
	// Use this for initialization
	void Start () {
		bug = GameObject.Find("MainBugPrefab");
		anim = bug.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Vector3.Distance (transform.position, bug.transform.position) <= distToBug) {
			anim.SetBool("isEating", true);
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