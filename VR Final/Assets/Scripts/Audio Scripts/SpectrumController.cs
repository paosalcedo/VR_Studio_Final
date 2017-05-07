using UnityEngine;
using System.Collections;

public class SpectrumController : MonoBehaviour {
	public float maxHeight;
	public int spectrumIndex;
	public float responseSpeed = 32;
	public static float desiredScale;

	Vector3 scale;

	void Start() {
		scale = transform.localScale;

	}

	void Update () {
   		desiredScale = 1+AudioManager.spectrum[spectrumIndex];
		//var desiredScale = Mathf.Log(AudioManager.spectrum[spectrumIndex], 2)*maxHeight * 100;
		scale.z = Mathf.Lerp(transform.localScale.z,desiredScale, Time.deltaTime * responseSpeed);
		transform.localScale = scale;
	}
}