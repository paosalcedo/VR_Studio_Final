using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MoveToCrumb : MonoBehaviour {

	public List<GameObject> crumbs = new List<GameObject> ();
//	public GameObject[] crumbs;
	Quaternion _lookRotation;
	float rotSpeed = 1f;

	public bool crumbsInScene;

	// Use this for initialization
	Transform player;

	public static MoveToCrumb instance;

	void Start () {
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		}
		else {
			Destroy (gameObject);
		}
 
	}
	
	// Update is called once per frame
	void Update () {
		if (!MovementScriptV2.instance.grabbed) {
			FindCrumb ();
		}
		Debug.Log (crumbs.Count);
		if (crumbs.Count > 1) {
			crumbsInScene = true;
		} else if (crumbs.Count <= 1) {
			crumbsInScene = false;		
		}
 	}

	void FindCrumb ()
	{
 		Vector3 crumbDir;

		crumbDir = crumbs [0].transform.position - transform.position;
 
		//make the creature look at the crumb

		_lookRotation = Quaternion.LookRotation (crumbDir);

		//rotate creature over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
		if ((transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) < 10f){
			transform.position += crumbDir * 1f * Time.deltaTime;	
		}
	
	}

}
