using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

	Rigidbody rb;
	GameObject player;
	//ATTEMPT 1 vars
//	public float rotForce = 20f;
	public float forwardForce = 20f;
	public float backForce = 20f;
	public float sideForce = 20f;
	public float upForce = 20f;
	public float downForce = 20f;
	public float rotForce = 50f;
	public float raycastRange = 1f;
	float initForwardForce;
	bool somethingUpAhead;


	//ATTEMPT VARS 2 based on https://www.youtube.com/watch?v=oLqWkR28ORM	
	float horizontalSpeed = 0.01f;
	float verticalSpeed = 1f;
	float amplitude = 1f;
	
	private Vector3 tempPosition;

	//ATTEMPT 3 "Levitate()" VARS
	float interval = 0.5f;
	public float initInterval;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		tempPosition = transform.position;
		rb = GetComponent<Rigidbody>();
		somethingUpAhead = false;
		initForwardForce = forwardForce;
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
//		ConstantMove();
//		AvoidThings();
//		MathijsMove();
		Levitate();
		Rotate();	
	}

	void Rotate ()
	{
//		rb.AddForce (Vector3.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
	// ADD TORQUE DOESN'T CHANGE DIRECTION IT'S MOVING IN
	//		rb.AddTorque (Vector3.forward * rotForce * Time.deltaTime, ForceMode.Impulse);
	//rotate around attempt.
		transform.RotateAround (player.transform.position, Vector3.up, 1);
//		if (!somethingUpAhead) {
//			rb.AddForce (Vector3.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
//		} else if (somethingUpAhead) {
//			rb.AddForce (Vector3.back * forwardForce * Time.deltaTime, ForceMode.Impulse);
//		}
	}
	
	void Levitate ()
	{
//		rb.AddForce (transform.up * upForce * Time.deltaTime, ForceMode.Impulse);
//		interval -= Time.deltaTime;
//		if (interval <= 0) {
////			Debug.Log ("up up and away");
//			rb.AddForce (transform.up * upForce * Time.deltaTime, ForceMode.Impulse);
//			interval = initInterval;
//		}

		Ray ray = new Ray (transform.position, transform.forward);
		Ray rayRight = new Ray (transform.position, transform.right);
		Ray rayLeft = new Ray (transform.position, Vector3.left);
		Ray rayUp = new Ray (transform.position, transform.up); 
		Ray rayDown = new Ray (transform.position, Vector3.down);
		Ray rayBack = new Ray (transform.position, -transform.forward);		

		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		Debug.DrawRay (rayRight.origin, rayRight.direction * raycastRange, Color.red);
		Debug.DrawRay (rayLeft.origin, rayLeft.direction * raycastRange, Color.red);	
		Debug.DrawRay (rayUp.origin, rayUp.direction * raycastRange, Color.red);
		Debug.DrawRay (rayDown.origin, rayDown.direction * raycastRange, Color.red);		
		Debug.DrawRay (rayBack.origin, rayBack.direction * raycastRange, Color.yellow);		
		RaycastHit rayHit = new RaycastHit ();
		RaycastHit rayHitUp = new RaycastHit ();
		RaycastHit rayHitDown = new RaycastHit ();
		RaycastHit rayHitLeft = new RaycastHit ();
		RaycastHit rayHitRight = new RaycastHit ();
		RaycastHit rayHitBack = new RaycastHit ();

		//3. we're ready to shoot our raycast

		if (Physics.Raycast (rayUp, out rayHitUp, raycastRange)) {
			if (rayHitUp.transform.tag == "Wall") {
				Debug.Log("Detected something above");
				rb.AddForce(Vector3.left * 10f * Time.deltaTime, ForceMode.Impulse);
			} 
		}  
		
		if (Physics.Raycast (rayDown, out rayHitDown, raycastRange)) {
			if (rayHitDown.transform.tag == "Wall") {
				Debug.Log("detected something below");
				rb.AddForce(Vector3.up * 10f * Time.deltaTime, ForceMode.Impulse);
			} 
		}

		if (Physics.Raycast (rayLeft, out rayHitLeft, raycastRange)) {
			if (rayHitLeft.transform.tag == "Wall") {
				rb.AddForce(Vector3.left * 10f * Time.deltaTime, ForceMode.Impulse);
			} 
		}

		if (Physics.Raycast (rayRight, out rayHitRight, raycastRange)) {
			if (rayHitRight.transform.tag == "Wall") {
				rb.AddForce(Vector3.up * 10f * Time.deltaTime, ForceMode.Impulse);
			} 
		}

		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
				rb.AddForce(Vector3.right * 10f * Time.deltaTime, ForceMode.Impulse);
				somethingUpAhead = true;	
			} 
		}

		

		if (Physics.Raycast (rayBack, out rayHitBack, raycastRange)) {
			if (rayHitBack.transform.tag == "Wall") {
				rb.AddForce(Vector3.down * 10f * Time.deltaTime, ForceMode.Impulse);
			} 
		}

	}

	void MathijsMove(){
		tempPosition.x += horizontalSpeed * Time.deltaTime;
		tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
		transform.position = tempPosition;	
	}	

	void ConstantMove ()
	{
		if (!somethingUpAhead) {
			forwardForce = initForwardForce;
			rb.AddForce (Vector3.forward * forwardForce * Time.deltaTime);
			rb.AddTorque (transform.right * rotForce * Time.deltaTime, ForceMode.Force);
			Debug.Log ("moving forward!");
		} else if (somethingUpAhead == true) {
			rb.AddForce(Vector3.up * upForce * Time.deltaTime);
			forwardForce = -forwardForce * 2f;
		}
	}

	void AvoidThings ()
	{
		//1. declare your raycast 
		Ray ray = new Ray (transform.position, transform.forward);
		//2. set up our raycast hit info variable too 
		Debug.DrawRay(ray.origin, ray.direction * raycastRange, Color.red);
		RaycastHit rayHit = new RaycastHit ();

		//3. we're ready to shoot our raycast

		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
				Debug.Log (rayHit.collider.tag);
				Debug.Log ("Wall ahead!");
				somethingUpAhead = true;
//				rb.AddForce (Vector3.up * upForce * Time.deltaTime);	
			} else {
				somethingUpAhead = false;
			}
		}		
		
	}

	//1. add rotational movement. perhaps AddTorque and AddForce?
	//2. raycast in the direction of Vector3.forward. Not infinite distance. When it finds something, change direction.
	//3. Give it a function that detects the player. if it detects the player, or transform.distance between the player and the object is less than a certain value,
	//change direction. 
	//Should it react to gravity? Ideally yes. so how will it levitate? constant force?
}
