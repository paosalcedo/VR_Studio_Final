using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GestureToCreature : MonoBehaviour {
	 		
	private Vector3 handVelo;
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
	}

	void CheckHandVelo ()
	{
 		Debug.Log("hand velocity is " + handVelo.magnitude);

		if (handVelo.magnitude > gestureMinVelo) {
			GameObject.Find("BugPrefab").SendMessage("PlayerCallOn");
		} 
	}
	
 
}
