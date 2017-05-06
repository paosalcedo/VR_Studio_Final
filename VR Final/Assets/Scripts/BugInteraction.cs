using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BugInteraction : MonoBehaviour {

	GameObject wing;
	TrailRenderer tr;

	EventController sceneControl;

	IgnoreHovering ignore;

	Vector3 lastPosition, fallbackVelocity;
	Quaternion lastRotation, fallbackTorque;
	GameObject block;
	public Animator wingAnim;

	// Use this for initialization
	void Start () {
		wing = GameObject.Find ("BugController");
		sceneControl = wing.GetComponent<EventController> ();
		ignore = GetComponent<IgnoreHovering> ();

		GameObject emitter = GameObject.Find ("Emitter");
		tr = emitter.GetComponent<TrailRenderer> ();

		tr.enabled = false;
	}

	void FixedUpdate () {

		if ( SteamVR.active == false ) {

			// manually record velocity

			fallbackVelocity = (transform.position - lastPosition) * 20f;

			lastPosition = transform.position;

			// manually record angular velocity

			fallbackTorque = Quaternion.FromToRotation( lastRotation.eulerAngles, transform.eulerAngles );

			lastRotation = transform.rotation;

		}

	}



	// this happens whenever a hand is near this object

	void HandHoverUpdate (Hand hand)
	{
		if (hand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			
			hand.AttachObject (gameObject);
			gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.localEulerAngles.x, 
				gameObject.transform.localEulerAngles.y , 
				gameObject.transform.localEulerAngles.z); 

			if (gameObject.tag == "Creature") {
				gameObject.GetComponent<MovementScriptV2>().grabbed = true;
				wingAnim.enabled = false;

				gameObject.SendMessage("PlayerCallOff");
			}
		}
	}



	// this happens when this object is attached to a hand, for whatever reason

	void OnAttachedToHand( Hand hand ) {

		GetComponent<Rigidbody>().isKinematic = true; // turn off physics so we can hold it



	}



	// this is like "Update" as long as we're holding something

	void HandAttachedUpdate( Hand hand ) {

		hand.otherHand = ignore.onlyIgnoreHand;

		if (hand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_Grip)) {
			tr.enabled = true;
		} else {
			tr.enabled = false;
		}

		if (hand.controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) { // on Vive controller, this is trigger
			hand.DetachObject( gameObject );

			if (gameObject.tag == "Creature") {
				gameObject.GetComponent<MovementScriptV2>().grabbed = false;
				wingAnim.enabled = true;


			}

		}

	}



	// this happens when the object is detached from a hand, for whatever reason

	void OnDetachedFromHand( Hand hand ) {

		GetComponent<Rigidbody>().isKinematic = false; // turns on physics

		tr.enabled = false;
		tr.Clear();

		// apply forces to it, as if we're throwing it

		GetComponent<Rigidbody>().AddForce( SteamVR.active ? hand.GetTrackedObjectVelocity() : fallbackVelocity, ForceMode.Impulse );

		GetComponent<Rigidbody>().AddTorque( SteamVR.active ? hand.GetTrackedObjectAngularVelocity() * 10f : fallbackTorque.eulerAngles, ForceMode.Impulse );

	}

	void ActivateEventController(){

		sceneControl.enabled = true;

	}


}
