﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractionPickup : MonoBehaviour {


	Vector3 lastPosition, fallbackVelocity;
	Quaternion lastRotation, fallbackTorque;
	GameObject block;

	// do this stuff for fallback mouse 2D support

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
		
 		// this applies to either Vive controller

		if (hand.GetStandardInteractionButton () == true) { // on Vive controller, this is trigger

			hand.AttachObject (gameObject);
			gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.localEulerAngles.x, 
				gameObject.transform.localEulerAngles.y + 90f, 
				gameObject.transform.localEulerAngles.z); 
 			if (gameObject.tag == "Creature") {
				gameObject.GetComponent<MovementScriptV2>().grabbed = true;
			}

			gameObject.SendMessage("PlayerCallOff");
  		}

  	}



	// this happens when this object is attached to a hand, for whatever reason

	void OnAttachedToHand( Hand hand ) {

		GetComponent<Rigidbody>().isKinematic = true; // turn off physics so we can hold it

	}



	// this is like "Update" as long as we're holding something

	void HandAttachedUpdate( Hand hand ) {

		if ( hand.GetStandardInteractionButton() == false ) { // on Vive controller, this is trigger
			hand.DetachObject( gameObject );
			gameObject.SendMessage ("RollUp");
//			GameObject.Find("Creature").SendMessage("ReleaseCreature");
			if (gameObject.tag == "Creature") {
				gameObject.GetComponent<MovementScriptV2>().grabbed = false;
			}
		}

	}



	// this happens when the object is detached from a hand, for whatever reason

	void OnDetachedFromHand( Hand hand ) {

		GetComponent<Rigidbody>().isKinematic = false; // turns on physics



		// apply forces to it, as if we're throwing it

		GetComponent<Rigidbody>().AddForce( SteamVR.active ? hand.GetTrackedObjectVelocity() : fallbackVelocity, ForceMode.Impulse );

		GetComponent<Rigidbody>().AddTorque( SteamVR.active ? hand.GetTrackedObjectAngularVelocity() * 10f : fallbackTorque.eulerAngles, ForceMode.Impulse );
	}



}


//{
//	//when hand is hovering near an object or over it
//	void HandHoverUpdate(Hand hand){ 
//		//this applies to either Vive controller
//		if(hand.GetStandardInteractionButton() ){ //On Vive controller, this is trigger
//			hand.AttachObject(gameObject);
//		}
//	}
//
//	//this is like update as long as we're holding something
//	void HandAttachedUpdate(Hand hand){
//		if(!hand.GetStandardInteractionButton() ){ //On Vive controller, this is trigger
//			hand.DetachObject(gameObject); // detaches gameobject from hand
//		}
//	}
//
//	//this happens whenever an object is attached to hand
//	void OnAttachedToHand(Hand hand){ 
//		GetComponent<Rigidbody> ().isKinematic = true;//turn off physics so we can hold it
//	}
//
//	//this happens when the object is detached from a hand, for whatever reason
//	void OnDetachedFromHand(Hand hand){
//		GetComponent<Rigidbody> ().isKinematic = false; //turns on physics
//		//apply forces to it, as if we're throwing it
//		GetComponent<Rigidbody>().AddForce(hand.GetTrackedObjectVelocity(), ForceMode.Impulse);
//		GetComponent<Rigidbody> ().AddTorque (hand.GetTrackedObjectAngularVelocity (), ForceMode.Impulse);
//	}
//
//
//}
