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
		if (transform.position.y < 0) {
			anim.SetBool ("Thrown", false);			
			anim.SetBool ("HitGround", true);	
			Debug.Log ("hit ground!");
		}

	}

	public void RollUp(){
		anim.SetBool("Thrown" , true);
	}
}
