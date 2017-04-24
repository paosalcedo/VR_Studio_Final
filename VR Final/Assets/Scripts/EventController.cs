using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;

	public ParticleSystem bubbles;

	Renderer rend;

	public Transform marker;

	public float lightMax, lightMin;

	public float emitMax, emitMin;

	public float fresMin , fresMax;

	public Color colorSky1, colorSky2;

	public Color colorTrail1, colorTrail2, colorTrail3, colorTrail4;

	public Color waterColor1, waterColor2;

	public float angleTrigger;

	public GameObject[] gameObjectHolder;

	GameObject selectedGameObject;

	float angleDifference;
	float yRotation;

	public Camera camera; // Remove before building

	[SerializeField] GameObject creature;

	void Start(){
		yRotation = transform.rotation.y;

		GameObject water = GameObject.Find ("WaterGround");
		rend = water.GetComponent<Renderer> ();


		print (rend.sharedMaterial.ToString ());
		rend.material.SetFloat ("_Shininess", 100f);


	}

	void Update(){

		ChangeSkyColor (colorSky1, colorSky2);
		ChangeLightIntensity (lightMin, lightMax);
		//		ChangeExposure (lightMin, lightMax);
//		EmitBubbles (emitMin, emitMax);

		ChangeWaterFresnel (fresMin, fresMax);
		ChangeTrailColor (colorTrail1, colorTrail2, colorTrail3, colorTrail4);
		ChangeWaterColor (waterColor1, waterColor2);


//		Vector3 camLookDir = camera.transform.forward;
//		
//		for (int i = 0; i < gameObjectHolder.Length; i++) {
//			Vector3 vectorFromCameraToTarget = gameObjectHolder [i].transform.position - camera.transform.position;
//		
//			float Angle = Vector3.Angle (camLookDir, vectorFromCameraToTarget);
//		
//			if (Angle < angleTrigger) {
//				selectedGameObject = gameObjectHolder [i];
//		
//				selectedGameObject.transform.localScale = new Vector3 (selectedGameObject.transform.localScale.x, 
//				ScaleGameObject (gameObjectHolder [i].transform, 1, 4), 
//				selectedGameObject.transform.localScale.z);
//			}
//		}



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

		float value = UtilScript.remapRange (angleDifference, -1, 1,lightMax,lightMin );
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
//		Debug.Log (scale.y);

		return scale.y;

	}

	public void ChangeWaterFresnel(float min, float max){
		float fresnel = UtilScript.remapRange (angleDifference, -1, 1, min, max);
		rend.material.SetFloat ("_FresnelScale", fresnel);

	}

	public void ChangeTrailColor(Color c1, Color c2, Color c3, Color c4){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor1 = Color.LerpUnclamped (c1, c2, colorChange);
		Color lerpedColor2 = Color.LerpUnclamped (c3, c4, colorChange);

		TrailRenderer tr = GetComponentInParent<TrailRenderer> ();
		float alpha = 1.0f;

		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(lerpedColor1, 0.0f), new GradientColorKey(lerpedColor2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
		);
		tr.colorGradient = gradient;

	}

	public void ChangeWaterColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		rend.material.SetColor ("_ReflectionColor", lerpedColor);
	}



}
