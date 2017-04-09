using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MovementScriptV2 : MonoBehaviour {
	
//	GameObject player;
	public float avoidDist;
	Transform player;
	Vector3 newDir;
	Vector3 newVec;
	public float avoidForce = 2f;
	public float forwardForce = 50f;
	public float redirectForce = 100f;
//	public float stableForce = 50f;
	public float rotSpeed = 10f;
	public float rotCooldownValue;
	Rigidbody rb;
	[SerializeField]float raycastRange = 5f;

	private Quaternion _lookRotation;
    private Vector3 _direction;

	bool grabbed;

	float angleDiff;
	public bool startCountdown;
	float rotCoolDown;

	// Use this for initialization
	void Start () {
		startCountdown = false;
		rotCoolDown = rotCooldownValue;
//		The rigidbody of the Creature.
		rb = GetComponent<Rigidbody>();
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
	}
	
	// Update is called once per frame
	void Update () {
		if(!grabbed){
			MoveForward();
	//		ClampAngularVelo();
			AvoidPlayer();
		}
	}


	void GrabCreature(){
		grabbed = true;
	}

	void ReleaseCreature(){
		grabbed = false;
	}

	
	void MoveForward ()
	{
//		Debug.Log (rb.velocity.magnitude);
//		rb.AddForce (transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
		transform.position += transform.forward * forwardForce * Time.deltaTime;

//		forwardForce *= 1.01f;
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		
		RaycastHit rayHit = new RaycastHit ();
//		Debug.Log ("transform.eulerAngles: " + transform.eulerAngles);
//		Debug.Log ("New Vec: " + newVec);
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
//				startCountdown = true;
				newVec = rayHit.normal + Random.insideUnitSphere;
//				transform.forward = newVec; 
// 				Version 1: add opposing force.
//				rb.AddForce ((newVec) * redirectForce * Time.deltaTime, ForceMode.Impulse);	
				
			} 
//			else {
//				rotCoolDown = rotCooldownValue;
//			}  	
		}
//		Debug.Log ("Angle difference: " + Vector3.Distance (transform.localEulerAngles, newVec));
//		angleDiff = Vector3.Distance (transform.localEulerAngles, newVec);

//		if (startCountdown == true) {
//			rotCoolDown -= Time.deltaTime;
//			if (rotCoolDown > 0f) {
//				transform.Rotate (Vector3.up * rotSpeed * Time.deltaTime);
//			} else {
//				Debug.Log("stop rotation");
//				startCountdown = false;
//			} 
//		}

//		if (rayHit.transform.tag == "Wall") {
//			if (Vector3.Distance (transform.eulerAngles, newVec) > 0.01f) {
//				transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
//			} 
//			else {
//				transform.eulerAngles = transform.forward;
//			}
//		}

//		MOVEMENT USING OTHER RAYS
//		Ray rayRight = new Ray (transform.position, transform.right);
//		Ray rayLeft = new Ray (transform.position, Vector3.left);
//		Ray rayBack = new Ray (transform.position, -transform.forward);
//		Ray rayUp = new Ray (transform.position, transform.up); 
//		Ray rayDown = new Ray (transform.position, Vector3.down);
//			
//		RaycastHit rayHitUp = new RaycastHit ();
//		RaycastHit rayHitDown = new RaycastHit ();
//		RaycastHit rayHitLeft = new RaycastHit ();
//		RaycastHit rayHitRight = new RaycastHit ();
//		RaycastHit rayHitBack = new RaycastHit ();
//
//		Debug.DrawRay (rayRight.origin, rayRight.direction * raycastRange, Color.red);
//		Debug.DrawRay (rayLeft.origin, rayLeft.direction * raycastRange, Color.red);	
//		Debug.DrawRay (rayUp.origin, rayUp.direction * raycastRange, Color.red);
//		Debug.DrawRay (rayDown.origin, rayDown.direction * raycastRange, Color.red);		
//		Debug.DrawRay (rayBack.origin, rayBack.direction * raycastRange, Color.red);		
//		
//
//		if (Physics.Raycast (rayUp, out rayHitUp, raycastRange)) {
//			if (rayHitUp.transform.tag == "Wall") {
//				rb.AddForce(rayHitUp.normal + Random.insideUnitSphere * redirectForce * Time.deltaTime, ForceMode.Impulse);	
//			} 
//		}  
//		
//		if (Physics.Raycast (rayDown, out rayHitDown, raycastRange)) {
//			if (rayHitDown.transform.tag == "Wall") {
//				rb.AddForce(rayHitDown.normal + Random.insideUnitSphere * redirectForce * Time.deltaTime, ForceMode.Impulse);	
//			} 
//		}
//
//		if (Physics.Raycast (rayLeft, out rayHitLeft, raycastRange)) {
//			if (rayHitLeft.transform.tag == "Wall") {
//				rb.AddForce(rayHitLeft.normal + Random.insideUnitSphere * redirectForce * Time.deltaTime, ForceMode.Impulse);
//			}
//		}
//
//		if (Physics.Raycast (rayRight, out rayHitRight, raycastRange)) {
//			if (rayHitRight.transform.tag == "Wall") {
//				rb.AddForce(rayHitRight.normal + Random.insideUnitSphere * redirectForce * Time.deltaTime, ForceMode.Impulse);
//			} 
//		}
//		
//		if (Physics.Raycast (rayBack, out rayHitBack, raycastRange)) {
//			if (rayHitBack.transform.tag == "Wall") {
//				rb.AddForce(rayHitBack.normal + Random.insideUnitSphere * redirectForce * Time.deltaTime, ForceMode.Impulse);
//			} 
//		}
	

         //create the rotation we need to be in to look at the target
         _lookRotation = Quaternion.LookRotation(newVec);
 
         //rotate us over time according to speed until we are in the required rotation
         transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
	}


	void ClampAngularVelo ()
	{
//		rb.angularVelocity.Normalize ();
//		rb.angularVelocity.magnitude = Mathf.Clamp(rb.angularVelocity.magnitude, 0, 1);
		if (rb.angularVelocity.magnitude > 1) {
//			print("Applying stabilizing force");
//			rb.AddTorque(-rb.angularVelocity * stableForce * Time.deltaTime);			
		} 
	}
	
	void AvoidPlayer ()
	{
		float dist;
		dist = Vector3.Distance (player.position, transform.position);
		Vector3 playerDir = player.position - transform.position;
		if (dist < avoidDist) {
			print("Player in range now!");
			rb.AddForce(-playerDir * avoidForce * Time.deltaTime, ForceMode.Impulse);
		}
	}

	

}
