using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BugInteraction : MonoBehaviour {

	GameObject wing;
	TrailRenderer tr;
	LineRenderer lr;
	Vector2 touch;

	public Color colorTrail1, colorTrail2, colorTrail3, colorTrail4;

	public float widthMin, widthMax;

	EventController sceneControl;

	Interactable interactable;

	Vector3 lastPosition, fallbackVelocity;
	Quaternion lastRotation, fallbackTorque;
	GameObject block;
	public Animator wingAnim;



	// Use this for initialization
	void Start () {
		wing = GameObject.Find ("BugController");
		sceneControl = wing.GetComponent<EventController> ();
		interactable = GetComponent<Interactable> ();

		GameObject emitter = GameObject.Find ("Emitter");
		tr = emitter.GetComponent<TrailRenderer> ();

		tr.enabled = false;

		lr = gameObject.AddComponent<LineRenderer> ();
		lr.material = new Material (Shader.Find("Sprites/Default"));
		lr.widthMultiplier = 0.005f;
		lr.positionCount = 2;
		lr.enabled = false;

		float a = 0.4f;
		float t = 0f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.black, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(a, 0.0f), new GradientAlphaKey(t, 1.0f) }
		);
		lr.colorGradient = gradient;
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

	void HandHoverUpdate (Hand hand){


		if (hand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {

			hand.AttachObject (gameObject);
			gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.localEulerAngles.x, 
				gameObject.transform.localEulerAngles.y, 
				gameObject.transform.localEulerAngles.z); 

			if (gameObject.tag == "Creature") {
				gameObject.GetComponent<MovementScriptV2> ().grabbed = true;
				wingAnim.enabled = false;

				gameObject.SendMessage ("PlayerCallOff");
			}
		}
	}



	// this happens when this object is attached to a hand, for whatever reason

	void OnAttachedToHand( Hand hand ) {

		GetComponent<Rigidbody>().isKinematic = true; // turn off physics so we can hold it



	}



	// this is like "Update" as long as we're holding something

	void HandAttachedUpdate( Hand hand ) {

		touch = hand.controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);


		if (hand.otherHand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			hand.HoverLock (interactable);
//			hand.otherHand.controller.TriggerHapticPulse(500, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );

			lr.enabled = true;;
		} else {
			hand.HoverUnlock (interactable);
			lr.enabled = false;
		}

		if (hand.controller.GetTouch (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
			tr.enabled = true;
//
//			hand.controller.TriggerHapticPulse();

			ChangeTrailColor (touch.x, colorTrail1, colorTrail2, colorTrail3, colorTrail4);
			ChangeTrailColor (touch.y, colorTrail3, colorTrail4, colorTrail1, colorTrail2);

		} else {
			tr.enabled = false;
		}

		lr.SetPosition (0, hand.otherHand.transform.position);
		lr.SetPosition (1, wing.transform.position);

//		print (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

		if (hand.controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) { // on Vive controller, this is trigger
			hand.DetachObject( gameObject );

			if (gameObject.tag == "Creature") {
				wingAnim.enabled = true;


			}

		}

	}





	// this happens when the object is detached from a hand, for whatever reason

	void OnDetachedFromHand( Hand hand ) {

//		GetComponent<Rigidbody>().isKinematic = false; // turns on physics
		gameObject.GetComponent<MovementScriptV2>().grabbed = false;
		gameObject.SendMessage ("RegainControl");

		tr.enabled = false;
		tr.Clear();

		// apply forces to it, as if we're throwing it

		GetComponent<Rigidbody>().AddForce( SteamVR.active ? hand.GetTrackedObjectVelocity() : fallbackVelocity, ForceMode.Impulse );

		GetComponent<Rigidbody>().AddTorque( SteamVR.active ? hand.GetTrackedObjectAngularVelocity() * 10f : fallbackTorque.eulerAngles, ForceMode.Impulse );

	}

	void ActivateEventController(){

		sceneControl.enabled = true;

	}

	void ChangeTrailColor(float axis, Color c1, Color c2, Color c3, Color c4){
		float colorChange = UtilScript.remapRange (axis, 0, 1, 0, 1);
		Color lerpedColor1 = Color.LerpUnclamped (c1, c2, colorChange);
		Color lerpedColor2 = Color.LerpUnclamped (c3, c4, colorChange);

		GameObject myGameObject = GameObject.Find ("Emitter");
		TrailRenderer tr = myGameObject.GetComponent<TrailRenderer> ();
		float alpha = 1.0f;

		Gradient gradient = new Gradient ();
		gradient.SetKeys (
			new GradientColorKey[] { new GradientColorKey (lerpedColor1, 0.0f), new GradientColorKey (lerpedColor2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey (alpha, 0.0f), new GradientAlphaKey (alpha, 1.0f) }
		);
		tr.colorGradient = gradient; 
	}

	void ChangeTrailWidth (float axis, float min, float max){
		float width = UtilScript.remapRange (axis, 0, 1,min,max );
		tr.widthMultiplier = width;
	}


}
