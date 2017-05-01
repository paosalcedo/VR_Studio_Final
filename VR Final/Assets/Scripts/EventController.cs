using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;

	public ParticleSystem bubbles;

	Renderer rend;

	public Material treeMat;
	public Material skybox;

//	public Transform marker;

	public float lightMax, lightMin;

	public float atmoMax, atmoMin;

	public float emitMax, emitMin;

	public float fresMin , fresMax;

	public Color groundColor1, groundColor2;

	public Color colorSky1, colorSky2;

	public Color colorTrail1, colorTrail2, colorTrail3, colorTrail4;

	public Color waterColor1, waterColor2;

	public Color treeColor1, treeColor2;

//	public float angleTrigger;


	public Animator animator;   

//	float angleDifference;
	float yRotation;

	public Camera camera; // Remove before building

	private float angleDifference;

	public float AngleDifference {
		get{ 
			return angleDifference;
		}

		set{
			angleDifference = value;

			if (angleDifference > 1) {
				angleDifference = 1;
			}
			if (angleDifference < -1) {
				angleDifference = -1;
			}
		}
	}

	[SerializeField] GameObject creature;

	void Start(){
		yRotation = transform.rotation.y;

		GameObject water = GameObject.Find ("WaterGround");
		rend = water.GetComponent<Renderer> ();


		print (rend.sharedMaterial.ToString ());
		rend.material.SetFloat ("_Shininess", 100f);

		AngleDifference = 0;
	}

	void Update(){
//
//		ChangeSkyColor (colorSky1, colorSky2);
//		ChangeLightIntensity (lightMin, lightMax);
//		ChangeAtmosphere (atmoMin, atmoMax);
////		EmitBubbles (emitMin, emitMax);
//
//		ChangeWaterFresnel (fresMin, fresMax);
//		ChangeTrailColor (colorTrail1, colorTrail2, colorTrail3, colorTrail4);
//		ChangeWaterColor (waterColor1, waterColor2);
//
//		ChangeTreeMaterial (treeColor1, treeColor2);
//		ChangeGroundColor (groundColor1, groundColor2);


		// Use the Below Code Only when Building for VR

		if (creature.GetComponent<MovementScriptV2> ().grabbed) {
			ChangeSkyColor (colorSky1, colorSky2);
			ChangeLightIntensity (lightMin, lightMax);
			ChangeAtmosphere (atmoMin, atmoMax);
			//		EmitBubbles (emitMin, emitMax);

			ChangeWaterFresnel (fresMin, fresMax);
			ChangeTrailColor (colorTrail1, colorTrail2, colorTrail3, colorTrail4);
			ChangeWaterColor (waterColor1, waterColor2);

			ChangeTreeMaterial (treeColor1, treeColor2);
			ChangeGroundColor (groundColor1, groundColor2);

		}


		AngleDifference = transform.rotation.y - yRotation;

		print (AngleDifference);


	}
		

	public void ChangeSkyColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
	}

	public void ChangeLightIntensity(float min, float max){

		float value = UtilScript.remapRange (angleDifference, -1, 1,min,max );
		light.intensity = value;
	}

	public void ChangeAtmosphere(float min, float max){
		float exposure = UtilScript.remapRange (transform.rotation.y, -1, 1,min,max );
		RenderSettings.skybox.SetFloat ("_AtmosphereThickness", exposure);
	}

	public void EmitBubbles(float min, float max){
		float rate = UtilScript.remapRange (angleDifference, -1, 1, min, max);
		var emission = bubbles.emission;
		emission.rateOverTime = rate;	
	}
		

//	public float ScaleGameObject(Transform t, float min, float max){
//		Vector3 scale = t.localScale;
//		scale.y = UtilScript.remapRange (angleDifference, -1, 1, min, max);
////		Debug.Log (scale.y);
//
//		return scale.y;
//
//	}

	public void ChangeWaterFresnel(float min, float max){
		float fresnel = UtilScript.remapRange (angleDifference, -1, 1, min, max);
		rend.material.SetFloat ("_FresnelScale", fresnel);

	}

	public void ChangeTrailColor(Color c1, Color c2, Color c3, Color c4){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
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
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
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
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		treeMat.SetColor("_TintColor", lerpedColor);
//		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
		
	}

	public void ChangeGroundColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (angleDifference, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		skybox.SetColor (" _GroundColor", lerpedColor); 
	}
		

}
