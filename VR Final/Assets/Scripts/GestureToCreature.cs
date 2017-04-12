using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GestureToCreature : MonoBehaviour {
	 		
	Vector3 handVelo;
	[SerializeField]float gestureMinVelo;
	// Use this for initialization
	void Start () {
		 
		handVelo = GetComponent<VelocityEstimator>().GetVelocityEstimate();
		Debug.Log("hand velocity is " + handVelo);
	}
	
	// Update is called once per frame
	void Update () {
		CheckHandVelo();
	}

	void CheckHandVelo ()
	{
		if (handVelo.magnitude > gestureMinVelo) {
			GameObject.Find ("Creature").SendMessage ("PlayerCallOn");
		} else if (handVelo.magnitude < gestureMinVelo) {
			GameObject.Find("Creature").SendMessage("PlayerCallOff");
		}
	}
	
}
