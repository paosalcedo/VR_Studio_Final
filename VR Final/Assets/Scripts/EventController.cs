using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class EventController : MonoBehaviour {

	public Light light;

	public float lightMax, lightMin;

//	LinearMapping linearMapping;

	void Start(){

//		linearMapping = GetComponent<LinearMapping> ();

	}

	void Update(){
		float value;
//		value = Util.RemapNumber (transform.rotation.y, -2, 2, lightMin, lightMax);
		value = UtilScript.remapRange (transform.rotation.y, -2, 2,lightMin,lightMax );
		light.intensity = value;
		Debug.Log (value);

	}

}
