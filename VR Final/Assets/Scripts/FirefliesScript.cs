using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefliesScript : MonoBehaviour {

	ParticleSystem ps;

	float rad;

	Vector3 axis = new Vector3(0,0,1);
	public float rotationsPerSecond = 0.02f;


	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();

		InvokeRepeating ("ChangeRadius", 0, 5.0f);

		rad = ps.shape.radius;

	}
	
	// Update is called once per frame
	void Update () {
		var angle = 360 * rotationsPerSecond * Time.deltaTime;
		transform.Rotate (axis, angle);

		var psShape = ps.shape;

		psShape.radius = rad;

//		Debug.Log (ps.name + ": " + rad);



			
	}

	void ChangeRadius(){


		float temp = Random.Range (1.1f, 2.0f);
		rad = temp;

	}



	
}
