using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;

	public ParticleSystem bubbles;

	public Transform marker;

	public float lightMax, lightMin;

	public float emitMax, emitMin;

	public Color color1;
	public Color color2;

	public float angleTrigger;
	public GameObject[] gameObjectHolder;

	GameObject selectedGameObject;

	float angleDifference;
	float yRotation;

	public Camera camera; // Remove before building

	[SerializeField] GameObject creature;

	void Start(){
		yRotation = transform.rotation.y;

	}

	void Update(){

		ChangeSkyColor (color1, color2);
		ChangeLightIntensity (lightMin, lightMax);
		//		ChangeExposure (lightMin, lightMax);
		EmitBubbles (emitMin, emitMax);

		Vector3 camLookDir = camera.transform.forward;
		
		for (int i = 0; i < gameObjectHolder.Length; i++) {
			Vector3 vectorFromCameraToTarget = gameObjectHolder [i].transform.position - camera.transform.position;
		
			float Angle = Vector3.Angle (camLookDir, vectorFromCameraToTarget);
		
			if (Angle < angleTrigger) {
				selectedGameObject = gameObjectHolder [i];
		
				selectedGameObject.transform.localScale = new Vector3 (selectedGameObject.transform.localScale.x, 
				ScaleGameObject (gameObjectHolder [i].transform, 1, 4), 
				selectedGameObject.transform.localScale.z);
			}
		}



//
//		if (creature.GetComponent<MovementScriptV2> ().grabbed) {
//
//			ChangeSkyColor (color1, color2);
//			ChangeLightIntensity (lightMin, lightMax);
////		ChangeExposure (lightMin, lightMax);
//			EmitBubbles (emitMin, emitMax);
//
//
//			Vector3 camLookDir = Camera.main.transform.forward;
//
//			for (int i = 0; i < gameObjectHolder.Length; i++) {
//				Vector3 vectorFromCameraToTarget = gameObjectHolder [i].transform.position - Camera.main.transform.position;
//
//				float Angle = Vector3.Angle (camLookDir, vectorFromCameraToTarget);
//
//				if (Angle < angleTrigger) {
//					selectedGameObject = gameObjectHolder [i];
//
//					selectedGameObject.transform.localScale = new Vector3 (selectedGameObject.transform.localScale.x, 
//						ScaleGameObject (gameObjectHolder [i].transform, 1, 4), 
//						selectedGameObject.transform.localScale.z);
//				}
//			}
//		}
//

		angleDifference = transform.rotation.y - yRotation;

	}
		

	public void ChangeSkyColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
	}

	public void ChangeLightIntensity(float min, float max){

		float value = UtilScript.remapRange (angleDifference, -1, 1,lightMin,lightMax );
		light.intensity = value;
	}

	public void ChangeExposure(float min, float max){
		float exposure = UtilScript.remapRange (transform.rotation.y, -1, 1,min,max );
		RenderSettings.skybox.SetFloat ("_Exposure", exposure);
	}

	public void EmitBubbles(float min, float max){
		float rate = UtilScript.remapRange (angleDifference, -1, 1, min, max);
		var emission = bubbles.emission;
		emission.rateOverTime = rate;	
	}

	public void CreateStructure(){
		GameObject myGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		myGameObject.transform.position = marker.position;
	}

	public float ScaleGameObject(Transform t, float min, float max){
		Vector3 scale = t.localScale;
		scale.y = UtilScript.remapRange (angleDifference, -1, 1, min, max);
		Debug.Log (scale.y);

		return scale.y;

	}



}
