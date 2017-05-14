using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbAnimation : MonoBehaviour {

	public Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onCollisionEnter (Collision coll)
	{
		if (coll.gameObject.tag == "Ground") {
			anim.SetBool ("Thrown", false);			
			anim.SetBool ("hitGround", true);		
		}		 
	}

	public void RollUp(){
		anim.SetBool("Thrown" , true);
	}
}
