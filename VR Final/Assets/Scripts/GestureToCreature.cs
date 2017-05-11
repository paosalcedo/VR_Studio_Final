using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GestureToCreature : MonoBehaviour {

	Hand _hand; 
	GameObject bug;

	public float waveTime;
 	private Vector3 handVelo;
	[SerializeField]float dizzyMinVelo;
	[SerializeField]float gestureMinVelo;

	// Use this for initialization
	void Start () {
 		GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
 		handVelo = Vector3.zero;
		_hand = GetComponent<Hand>();
		bug = GameObject.Find("BugPrefab");		
	}
	
	// Update is called once per frame
	void Update ()
	{
		handVelo = GetComponent<VelocityEstimator> ().GetVelocityEstimate ();
		CheckHandVelo ();
	}

	bool isWaving;
	float waveCoolDown;
	void CheckHandVelo ()
	{

		if (handVelo.magnitude > gestureMinVelo && !bug.GetComponent<MovementScriptV2>().grabbed) {
			waveTime += Time.deltaTime;
			if (waveTime > 1f) {
				GameObject.Find ("BugPrefab").SendMessage ("PlayerCallOn");
			}
		} 
//		else if(MovementScriptV2.isNearPlayer){
//			GameObject.Find ("BugPrefab").SendMessage ("PlayerCallOff");
//		}
			else {
			waveTime = 0;
		} 

//check if hand velocity was enough to cause dizziness.
//if yes, tell MovementScript to 

		if (handVelo.magnitude > dizzyMinVelo && _hand.GetStandardInteractionButtonUp() == true && !bug.GetComponent<MovementScriptV2>().grabbed) {
			Debug.Log("GETTING DIZZY!");
			Debug.Log ("hand velocity is " + handVelo.magnitude);
			GameObject.Find("BugPrefab").SendMessage("StartDizziness");	
		}

	}
	
 
}
