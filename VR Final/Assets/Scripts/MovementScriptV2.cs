using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MovementScriptV2 : MonoBehaviour {
	
//	GameObject player;
	public float avoidDist;
	Transform player;
	Vector3 newDir;
	public float avoidForce = 2f;
	public float forwardForce = 50f;
	public float redirectForce = 100f;
	public float stableForce = 50f;
	Rigidbody rb;
	[SerializeField]float raycastRange = 5f;
	// Use this for initialization
	void Start () {
//		The rigidbody of the Creature.
		rb = GetComponent<Rigidbody>();
//		player = GameObject.Find("FallbackObjects");
		
		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
	}
	
	// Update is called once per frame
	void Update () {
		MoveForward();
//		ClampAngularVelo();
		AvoidPlayer();
	}

	private bool rotating;
	Vector3 to;
	Vector3 newVec;
	void MoveForward ()
	{
		rb.AddForce (transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		RaycastHit rayHit = new RaycastHit ();
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
				newVec = rayHit.normal + Random.insideUnitSphere;
				rb.AddForce ((newVec) * redirectForce * Time.deltaTime, ForceMode.Impulse);
				rotating = true;
			}  	
		}

		if (rotating == true) {
			if (Vector3.Distance (transform.eulerAngles.y, newVec.y) > 0.01f) {
				transform.eulerAngles = Vector3.Lerp (transform.rotation.eulerAngles.y, newVec.y, Time.deltaTime);
			} else {
				rotating = false;
				transform.eulerAngles = transform.forward;
			}
		}
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
	}


	void ClampAngularVelo ()
	{
//		rb.angularVelocity.Normalize ();
//		rb.angularVelocity.magnitude = Mathf.Clamp(rb.angularVelocity.magnitude, 0, 1);
		if (rb.angularVelocity.magnitude > 1) {
//			print("Applying stabilizing force");
			rb.AddTorque(-rb.angularVelocity * stableForce * Time.deltaTime);			
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
