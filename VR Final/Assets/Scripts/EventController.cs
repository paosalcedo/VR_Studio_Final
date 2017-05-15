using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;

	ParticleSystem [] fireflies;

	public AudioMixer mixer;

	public Transform wind;

	Renderer rend;

	public Material treeMat;
	public Material skybox;
	public Material eyeball;

	LinearMapping lm;
	CircularDrive cd;

	public float windHeightMin, windHeightMax;

	public float eyeMin, eyeMax, eyeStepMin, eyeStepMax;

	public float lightMax, lightMin;

	public float atmoMax, atmoMin;

	public float emitMax, emitMin;

	public float fresMin , fresMax;

	public float volumeMin ,volumeMax;

	public Color groundColor1, groundColor2;

	public Color colorSky1, colorSky2;

//	public Color colorTrail1, colorTrail2, colorTrail3, colorTrail4;

	public Color waterColor1, waterColor2;

	public Color treeColor1, treeColor2;

	public Color eyeColor1, eyeColor2;
//	public Animator animator;   


	public Camera camera; // Change to VRCamera in inspector before building 


	float linearValue;

	[SerializeField] GameObject creature;

	void Start(){

		cd = GetComponent<CircularDrive> ();

		lm = cd.GetComponent<LinearMapping> ();

		GameObject psGo = GameObject.Find ("Fireflies");
		fireflies = psGo.GetComponentsInChildren<ParticleSystem> ();



		GameObject water = GameObject.Find ("WaterGround");
		rend = water.GetComponent<Renderer> ();


		print (rend.sharedMaterial.ToString ());
		rend.material.SetFloat ("_Shininess", 100f);

	}

	void Update () {

		float v = lm.value;

		if (v < 0.5) {
			linearValue = v * 2.0f;
		} else {
			linearValue = (1.0f - v) * 2.0f;
		}


		if (SteamVR.active == false) {



			
			ChangeSkyColor (colorSky1, colorSky2);
			ChangeLightIntensity (lightMin, lightMax);
			ChangeAtmosphere (atmoMin, atmoMax);
			EmitFireflies (emitMin, emitMax);
			RaiseWindZone (windHeightMin, windHeightMax);
			ChangeWaterFresnel (fresMin, fresMax);
//			ChangeTrailColor (colorTrail1, colorTrail2, colorTrail3, colorTrail4);
			ChangeWaterColor (waterColor1, waterColor2);
			
			ChangeTreeMaterial (treeColor1, treeColor2);
			ChangeGroundColor (groundColor1, groundColor2);
			ChangeEyeBrightness (eyeMin, eyeMax, eyeStepMin, eyeStepMax);
			ChangeEyeColor (eyeColor1, eyeColor2);
			ChangeAmbientAudio (volumeMin, volumeMax);

		} else {

			if (creature.GetComponent<MovementScriptV2> ().grabbed) {
				ChangeSkyColor (colorSky1, colorSky2);
				ChangeLightIntensity (lightMin, lightMax);
				ChangeAtmosphere (atmoMin, atmoMax);
				EmitFireflies (emitMin, emitMax);
				RaiseWindZone (windHeightMin, windHeightMax);
				ChangeWaterFresnel (fresMin, fresMax);
//				ChangeTrailColor (colorTrail1, colorTrail2, colorTrail3, colorTrail4);
				ChangeWaterColor (waterColor1, waterColor2);

				ChangeTreeMaterial (treeColor1, treeColor2);
				ChangeGroundColor (groundColor1, groundColor2);
				ChangeEyeBrightness (eyeMin, eyeMax, eyeStepMin, eyeStepMax);
				ChangeEyeColor (eyeColor1, eyeColor2);
				ChangeAmbientAudio (volumeMin, volumeMax);


			}
		}


//		print("Linearmapping: " + linearValue);
//

	}



	public void ChangeSkyColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
	}

	public void ChangeLightIntensity(float min, float max){

		float value = UtilScript.remapRange (linearValue, 0, 1,min,max );
		light.intensity = value;
	}

	public void ChangeAtmosphere(float min, float max){
		float exposure = UtilScript.remapRange (linearValue, 0, 1,min,max );
		RenderSettings.skybox.SetFloat ("_AtmosphereThickness", exposure);
	}

	public void EmitFireflies(float min, float max){
		for (int i = 0; i < fireflies.Length; i++) {
			float rate = UtilScript.remapRange (linearValue, 0, 1, min, max);
			var emission = fireflies[i].emission;
			emission.rateOverTime = rate;
		}
	}
		


	public void ChangeWaterFresnel(float min, float max){
		float fresnel = UtilScript.remapRange (linearValue, 0, 1, min, max);
		rend.material.SetFloat ("_FresnelScale", fresnel);

	}

	public void ChangeTrailColor(Color c1, Color c2, Color c3, Color c4){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1,0,1 );
		Color lerpedColor1 = Color.LerpUnclamped (c1, c2, colorChange);
		Color lerpedColor2 = Color.LerpUnclamped (c3, c4, colorChange);

		GameObject myGameObject = GameObject.Find ("Emitter");
		TrailRenderer tr = myGameObject.GetComponent<TrailRenderer> ();
		float alpha = 1.0f;

		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(lerpedColor1, 0.0f), new GradientColorKey(lerpedColor2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
		);
		tr.colorGradient = gradient;

	}

	public void ChangeWaterColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		rend.material.SetColor ("_ReflectionColor", lerpedColor);
	}

	public void PlayAnimation (Animator anim){
		float animationTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
		Debug.Log("animationTime (normalized) is " + animationTime);

//		Animation animClip;
//


//		float clipTime = UtilScript.remapRange (angleDifference, -1, 1, 0, anim.GetNextAnimatorClipInfo.);
//
//		animator.speed = 0.0001f;
//		animator.Play("TestAnimation", -1, animationTime);

	}

	public void ChangeTreeMaterial(Color c1, Color c2){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		treeMat.SetColor("_TintColor", lerpedColor);
//		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
		
	}

	public void ChangeGroundColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		skybox.SetColor (" _GroundColor", lerpedColor); 
	}

	public void RaiseWindZone(float min, float max){
		float height = UtilScript.remapRange (linearValue, 0, 1, min, max);

		wind.position = new Vector3 (wind.position.x, height, wind.position.z);
			
	}

	public void ChangeEyeBrightness(float min, float max, float stepMin, float stepMax){
		float brightness = UtilScript.remapRange (linearValue, 0, 1, min, max);
		float step = UtilScript.remapRange (linearValue, 0, 1, stepMin, stepMax);
		eyeball.SetFloat ("_Brightness", brightness);
		eyeball.SetFloat ("_StepSize", step);

	}

	public void ChangeEyeColor(Color c1, Color c2){
		float colorChange = UtilScript.remapRange (linearValue, 0, 1, 0, 1);
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);
		eyeball.SetColor ("_Color", lerpedColor);

	}

	public void ChangeAmbientAudio(float min, float max){
		float volume1 = UtilScript.remapRange (linearValue, 0, 1, min, max);
		float volume2 = UtilScript.remapRange (linearValue, 0, 1, max, min);

		mixer.SetFloat ("nightVolume", volume1);
		mixer.SetFloat ("dayVolume", volume2);
	}


		

}
