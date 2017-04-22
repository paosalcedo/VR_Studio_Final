﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MovementScriptV2 : MonoBehaviour {
	
//	GameObject player;
//	public float avoidDist;

	Transform player;
	Vector3 newDir;
	Vector3 newVec;
//	public float avoidForce = 2f;
	public float forwardForce = 50f;
	public float redirectForce = 100f;
//	public float stableForce = 50f;
	public float rotSpeed = 10f;
//	public float rotCooldownValue;
	Rigidbody rb;
	[SerializeField]float raycastRange = 5f;

	private Quaternion _lookRotation;
    private Vector3 _direction;

	public bool playerIsCalling;
	public bool grabbed;
	public bool crumbsAbound;

	float angleDiff;
	public bool startCountdown;
	float rotCoolDown;

	// Use this for initialization
	void Start () {
//		startCountdown = false;
//		rotCoolDown = rotCooldownValue;
//		The rigidbody of the Creature.
		rb = GetComponent<Rigidbody>();
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
	}
	
	// Update is called once per frame
	void Update () {
		// player only moves on its own
		if(!grabbed)//insert bool for crumbs here.)
		{
//			MoveForward();
//			GoToPlayer();
 		}
	}

	void PlayerCallOn(){
		playerIsCalling = true;
		Debug.Log("player is calling is true");
	}

	void PlayerCallOff ()
	{
		playerIsCalling = false;
		Debug.Log("player is calling is false");
	}

	
	void MoveForward ()
	{
		float moveForwardInterval;
//		Debug.Log (rb.velocity.magnitude);
//		rb.AddForce (transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
		if (!playerIsCalling || playerIsCalling) {
 			transform.position += transform.forward * forwardForce * Time.deltaTime;
//			moveForwardInterval -= Time.deltaTime;
//			if (moveForwardInterval <= 0f) {
//				forwardForce = 0f;
//			}
	    	//create the rotation we need to be in to look at the target
         	_lookRotation = Quaternion.LookRotation(newVec);
 
         	//rotate us over time according to speed until we are in the required rotation
         	transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
		}

//		forwardForce *= 1.01f;
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		
		RaycastHit rayHit = new RaycastHit ();
//		Debug.Log ("transform.eulerAngles: " + transform.eulerAngles);
//		Debug.Log ("New Vec: " + newVec);
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
				newVec = rayHit.normal + Random.insideUnitSphere;				
			} 
		}     
	}

	void GoToPlayer ()
	{
//		float dist;
//		dist = Vector3.Distance (player.position, transform.position);
		Vector3 playerDir = player.position - transform.position;
//		Debug.Log(transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y);

		if (playerIsCalling) {
			//make the creature look at the player
			_lookRotation = Quaternion.LookRotation (playerDir);
			newVec = playerDir;
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
			if ((transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) < 10f){
				transform.position += playerDir * 1f * Time.deltaTime;	
			}
		}
	}

}
