using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MoveToCrumb : MonoBehaviour {

	public static List<GameObject> crumbs = new List<GameObject> ();
	Quaternion _lookRotation;
	float rotSpeed = 1f;
	float forwardSpeed = 1f;

	public static bool crumbsInScene;

	// Use this for initialization
	Transform player;

	public static MoveToCrumb instance;

	void Start () {
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		ghostcrumbs = new GameObject[crumbs.Count];
//		Debug.Log ("Ghost crumbs: " + ghostcrumbs.Length);
		if (GetComponent<MovementScriptV2>().grabbed == false && crumbsInScene == true) {
			FindCrumb ();
		}
		
		Debug.Log (crumbs.Count);
		
		if (crumbs.Count > 0) {
			crumbsInScene = true;
		} else {
			crumbsInScene = false;
		}

//		if (crumbs.Count == 0) {
//			crumbsInScene = false;		
//		}
 	}

	void FindCrumb ()
	{
		Debug.Log ("CRUMB SEEKING ACTIVE");
		Vector3 crumbDir;
		if (crumbs.Count > 0) {
			crumbDir = crumbs [0].transform.position - transform.position;
		
 
			//make the creature look at the crumb

			_lookRotation = Quaternion.LookRotation (crumbDir);

			//rotate creature over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
//			if ((transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) < 10f) {
			transform.position += crumbDir * forwardSpeed * Time.deltaTime;	
//			}
		}
	}

}
