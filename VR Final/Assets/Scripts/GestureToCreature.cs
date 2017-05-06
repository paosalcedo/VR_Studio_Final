using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GestureToCreature : MonoBehaviour {

	public float waveTime;
	 		
	private Vector3 handVelo;
	[SerializeField]float gestureMinVelo;

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

	bool isWaving;
	float waveCoolDown;
	void CheckHandVelo ()
	{

//		Debug.Log ("hand velocity is " + handVelo.magnitude);

		if (handVelo.magnitude > gestureMinVelo) {
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
	}
	
 
}
