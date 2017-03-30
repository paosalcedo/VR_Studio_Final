using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScriptV2 : MonoBehaviour {
	
	public float forwardForce = 50f;
	public float redirectForce = 100f;
	Rigidbody rb;
	public float raycastRange = 5f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		MoveForward();
	}

	void MoveForward ()
	{
		rb.AddForce(transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);
		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		RaycastHit rayHit = new RaycastHit ();
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall") {
				rb.AddForce(rayHit.normal + Random.insideUnitSphere * 100f * Time.deltaTime, ForceMode.Impulse);	
			} 
		}
	}
}
