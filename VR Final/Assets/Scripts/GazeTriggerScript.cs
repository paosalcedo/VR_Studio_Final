using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

public class GazeTriggerScript : MonoBehaviour {


	[SerializeField] float timeLookedAt = 0f; // Time in secs we spent looking at thing
//	[SerializeField] Image progressImage; // assign in the inspector
	[SerializeField] float angleTrigger;

	public GameObject[] gameObjectHolder;

	GameObject selectedGameObject;

	public Camera camera; // Remove before building


//	public UnityEvent OnGazeComplete = new UnityEvent();

	void Update () {

		Vector3 camLookDir = camera.transform.forward;

		for (int i = 0; i < gameObjectHolder.Length; i++) {
			Vector3 vectorFromCameraToTarget = gameObjectHolder [i].transform.position - camera.transform.position;

			float Angle = Vector3.Angle (camLookDir, vectorFromCameraToTarget);

			if (Angle < angleTrigger) {
				selectedGameObject = gameObjectHolder [i];
				Debug.Log (selectedGameObject.name);
			}
		}

//			if (Angle < angleTrigger) {
//				timeLookedAt = Mathf.Clamp01 (timeLookedAt + Time.deltaTime); //After 1sec this variable will be one
//
//				if (timeLookedAt == 1f) {
//					timeLookedAt = 0f;
//					//				OnGazeComplete.Invoke (); //Fire associated events
//				}
//
//			} else {
//				// Decay progress if we are not looking at it
//				timeLookedAt = Mathf.Clamp01 (timeLookedAt - Time.deltaTime);
//				//			rs.speedMultiplier = timeLookedAt;
//			}
		}
			
		//update our UI image
//		progressImage.fillAmount = timeLookedAt;

}
