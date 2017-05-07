using System.Collections;
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
	float callTime;
	public bool listenForCall;
	public float kinematicDelay; //the larger this value is, the longer it takes for the bug to regain control after being thrown.
	public float minCallTime;
	public float minCallVolume;
	public float forwardForce = 50f;
//	public float stableForce = 50f;
	public float rotSpeed = 10f;
//	public float rotCooldownValue;
//	public float hoverVertSpeed;
//	public float hoverHoriSpeed;
//	public float amplitude;
	public float moveToPlayerSpeed = 1f;
	float dist;
	
	private Vector3 tempPos;

	Rigidbody rb;
	[SerializeField]float raycastRange = 5f;

	private Quaternion _lookRotation;
    private Vector3 _direction;

	public bool playerIsCalling;
	public bool grabbed;
	public static bool isNearPlayer;

	void Start () {
		Debug.Log(minCallVolume);
 		rb = GetComponent<Rigidbody>();
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
		tempPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// !grabbed stops creature from moving when you grab it--because it uses Transform to move and not Rigidbody, 
		// hand.AttachObject() does not keep it from moving.
		// the crumbsInScene bool checks if there are any crumbs in the scene.

		//THIS IS FOR STANDARD MOVEMENT
		if (!grabbed && !MoveToCrumb.crumbsInScene) {//insert bool for crumbs here.)
			MoveForward ();
			GoToPlayer ();
		}

		//FOR MIC INPUT
		if (SpectrumController.desiredScale > minCallVolume && listenForCall) {
			callTime += Time.deltaTime;
			if (callTime > minCallTime) {
				Debug.Log("bug hears you!");
				GameObject.Find ("BugPrefab").SendMessage ("PlayerCallOn");
			}
		} 
		else {
			callTime = 0f;
		}
//		Check for rigidbody velocity

//		Debug.Log("velocity is: " + GetComponent<Rigidbody>().velocity.magnitude);
//		Debug.Log("angularVelocity is: " + GetComponent<Rigidbody>().angularVelocity.magnitude);
	}

	void PlayerCallOn(){
		playerIsCalling = true;
 	}

	void PlayerCallOff ()
	{
		playerIsCalling = false;
 	}

	
	void MoveForward ()
	{
 		if (!playerIsCalling) {
 			transform.position += transform.forward * forwardForce * Time.deltaTime;
 
	    	//create the rotation we need to be in to look at the target
         	_lookRotation = Quaternion.LookRotation(newVec);
 
         	//rotate us over time according to speed until we are in the required rotation
         	transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
		}

 		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		
		RaycastHit rayHit = new RaycastHit ();		 
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			Debug.Log(rayHit.collider.name);
			if (rayHit.transform.tag == "Wall" || rayHit.transform.tag == "Ground") {
				newVec = rayHit.normal + Random.insideUnitSphere;				
			} 
		}     
	}

	void GoToPlayer ()
	{
//		Vector3 playerDir = player.position - transform.position;
		
//		Debug.Log(transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y);
 
		if (playerIsCalling) {
			dist = Vector3.Distance (player.position, transform.position);
			Vector3 playerDir = ((player.forward * 0.75f) + (player.up * 1.5f)) - transform.position;
			//ROTATE NECK animation state here.
			//make the creature look at the player
			_lookRotation = Quaternion.LookRotation (playerDir);
//			newVec = playerDir;
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
			if ((transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) < 10f) {
				transform.position += playerDir * moveToPlayerSpeed * Time.deltaTime;	
			}
		}
	}

	void RegainControl ()
	{
		Invoke ("MakeKinematic", kinematicDelay);
	}

	void MakeKinematic(){
		GetComponent<Rigidbody>().isKinematic = true;
	}


	


	
 
}
