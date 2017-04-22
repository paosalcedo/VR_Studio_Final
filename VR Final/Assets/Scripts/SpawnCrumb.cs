using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SpawnCrumb : MonoBehaviour {

	Hand _hand;
 	// Use this for initialization

	public static SpawnCrumb instance;

	void Start () {
		_hand = GetComponent<Hand> ();
 		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftShift) && _hand.GetStandardInteractionButtonDown() == true) {
			GameObject crumb = Instantiate (Resources.Load ("Prefabs/Crumb") as GameObject);
			_hand.AttachObject (crumb);
			Debug.Log ("attaching object!");
			MoveToCrumb.instance.crumbs.Add(crumb);
//			MoveToCrumb.instance.crumbsInScene = true;

//			MoveToCrumb.instance.CrumbsAreInScene ();
		}
	}
}
