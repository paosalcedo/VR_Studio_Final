using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventController : MonoBehaviour {


	[SerializeField] float timeLookedAt = 0f;
	[SerializeField] Image progressImage;
	[SerializeField] float angleTrigger;


//	public UnityEvent LightTriggers = new UnityEvent();
//	public UnityEvent AudioTriggers = new UnityEvent();
	public UnityEvent OnGazeStart = new UnityEvent();
	public UnityEvent OnGazeEnd = new UnityEvent ();

	void Update () {
		// Is the camera looking / pointing at something?

		//Get direction user is looking
		Vector3 camLookDir = Camera.main.transform.forward;
		// Direction from player to the target object (A to B = B-A)
		Vector3 vectorFromCameraToTarget = transform.position - Camera.main.transform.position;

		// get angle between look direction and object's direction
		float Angle = Vector3.Angle(camLookDir, vectorFromCameraToTarget);

		//do stuff based on that angle
		if (Angle < angleTrigger) {
			timeLookedAt = Mathf.Clamp01 (timeLookedAt + Time.deltaTime); //After 1sec this variable will be one

			if (timeLookedAt == 1f) {
				timeLookedAt = 0f;
//				OnGazeComplete.Invoke (); 
				//				rs.speedMultiplier = timeLookedAt;
			}


		} else {
			// Decay progress if we are not looking at it
			timeLookedAt = Mathf.Clamp01 (timeLookedAt - Time.deltaTime);
			//			rs.speedMultiplier = timeLookedAt;
		}

		//update our UI image
		progressImage.fillAmount = timeLookedAt;
	}
}
