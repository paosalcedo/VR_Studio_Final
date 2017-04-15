using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GestureToCreature : MonoBehaviour {
	 		
	Vector3 handVelo;
	public float gestureMinVelo;
	// Use this for initialization
	void Start () {
//		gestureMinVelo = 5f;
 		GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
		handVelo = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
		handVelo = GetComponent<VelocityEstimator> ().GetVelocityEstimate ();
		CheckHandVelo ();
//		if (Input.GetKeyDown (KeyCode.W)) {
//			Debug.Log("STOP ESTIMATING!");
// 			GetComponent<VelocityEstimator>().FinishEstimatingVelocity();
//		}
//		if (Input.GetKeyDown (KeyCode.S)) {
//			Debug.Log("START ESTIMATING");
// 			GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
//		}

	}

	void CheckHandVelo ()
	{
// 		Debug.Log("hand velocity is " + handVelo.magnitude);

		if (handVelo.magnitude > gestureMinVelo) {
			GameObject.Find("Creature").SendMessage("PlayerCallOn");
		} 
		else if (handVelo.magnitude < gestureMinVelo) {
			GameObject.Find("Creature").SendMessage("PlayerCallOff");
		}
	}
	
}
