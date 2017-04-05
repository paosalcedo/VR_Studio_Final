using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScriptV2 : MonoBehaviour {
	
	GameObject player;
	Vector3 newDir;
	public float avoidForce = 100;
	public float forwardForce = 50f;
	public float redirectForce = 100f;
	public float stableForce = 50f;
	Rigidbody rb;
	public float raycastRange = 5f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		player = GameObject.Find("FallbackObjects");
	}
	
	// Update is called once per frame
	void Update () {
		MoveForward();
		ClampAngularVelo();
		AvoidPlayer();
	}

	void MoveForward ()
	{
//		transform.Translate(transform.forward * forwardForce * Time.deltaTime);
//		transform.Rotate(Vector3.up * 25f * Time.deltaTime);
		rb.AddForce (transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		RaycastHit rayHit = new RaycastHit ();
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
//			transform.forward = rayHit.normal;
//			transform.forward = Vector3.Lerp(transform.forward, rayHit.normal, 0.9f);
 //			transform.forward = newDir;
			if (rayHit.transform.tag == "Wall") {
				rb.AddForce ((rayHit.normal + Random.insideUnitSphere) * redirectForce * Time.deltaTime, ForceMode.Impulse);	
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
		dist = Vector3.Distance (player.transform.position, transform.position);
		Vector3 playerDir = player.transform.position - transform.position;
		if (dist < 5f) {
			print("Player in range!");
			rb.AddForce(-playerDir * avoidForce * Time.deltaTime, ForceMode.Impulse);
		}
	}
}
