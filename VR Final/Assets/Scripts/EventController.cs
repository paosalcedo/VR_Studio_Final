using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;


	public ParticleSystem bubbles;


	public float lightMax, lightMin;

	public float emitMax, emitMin;

	public Color color1;
	public Color color2;


	void Start(){


	}

	void Update(){

		ChangeSkyColor (color1, color2);
		ChangeLightIntensity (lightMin, lightMax);
//		ChangeExposure (lightMin, lightMax);
		EmitBubbles (emitMin, emitMax);


	}

	public void ChangeSkyColor (Color c1, Color c2){
		float colorChange = UtilScript.remapRange (transform.rotation.y, -1, 1,0,1 );
		Color lerpedColor = Color.LerpUnclamped(c1, c2, colorChange);

		RenderSettings.skybox.SetColor ("_SkyTint", lerpedColor); 
	}

	public void ChangeLightIntensity(float min, float max){

		float value = UtilScript.remapRange (transform.rotation.y, -1, 1,lightMin,lightMax );
		light.intensity = value;
	}

	public void ChangeExposure(float min, float max){
		float exposure = UtilScript.remapRange (transform.rotation.y, -1, 1,min,max );
		RenderSettings.skybox.SetFloat ("_Exposure", exposure);
//		print (exposure);
	}

	public void EmitBubbles(float min, float max){
		float rate = UtilScript.remapRange (transform.rotation.y, -1, 1, min, max);
		var emission = bubbles.emission;
		emission.rateOverTime = rate;
		
	}

}
