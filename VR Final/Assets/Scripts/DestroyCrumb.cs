using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrumb : MonoBehaviour {


	Animator anim;
	GameObject bug;

	bool hasEaten;
	GameObject clone;

//	ParticleSystem ps;

	public float distToBug;
 	// Use this for initialization
	void Start () {
		bug = GameObject.Find("MainBugPrefab");
		anim = bug.GetComponent<Animator>();
		hasEaten = false;

//		ps = bug.GetComponentInChildren<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Vector3.Distance (transform.position, bug.transform.position) <= distToBug) {
			anim.SetBool("isDashing", false);
			StartCoroutine("EatAndWait");
 		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Creature" || coll.gameObject.name == "WaterGround") {
			Destroy (gameObject);
			MoveToCrumb.crumbs.Remove (gameObject);
		}
	}

	IEnumerator EatAndWait ()

	{
		if (!hasEaten) {
			clone = Instantiate (Resources.Load ("Prefabs/PS_Eat") as GameObject);
			clone.transform.position = transform.position;
			anim.SetBool ("isEating", true);
			hasEaten = true;

		}

		yield return new WaitForSeconds(0.8f);

		Destroy (clone);
		MoveToCrumb.crumbs.Remove (gameObject);
		Destroy(gameObject);
 	}
}