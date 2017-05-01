using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTestScriprt : MonoBehaviour {

	public Slider sliderScrubber;
	public Animator animator;   

	public void Update()
	{
		float animationTime = Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		Debug.Log("animationTime (normalized) is " + animationTime);
		sliderScrubber.value = animationTime;

		if (Input.GetKey(KeyCode.Space)){
			animator.Play("TestAnimtion", 0, 0f);
		}
	}

	public void OnValueChanged(float changedValue)
	{
		animator.Play("TestAnimtion", 0, sliderScrubber.normalizedValue);
	}

}
