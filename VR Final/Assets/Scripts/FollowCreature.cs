using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCreature : MonoBehaviour {

	GameObject creature;
	float creatureRotX;
	float creatureRotY;
	float creatureRotZ;
	// Use this for initialization
	void Start () {
		creature = GameObject.Find("Creature");
		creatureRotX = creature.transform.localEulerAngles.x;
		creatureRotZ = creature.transform.localEulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = creature.transform.position;
		transform.localEulerAngles = new Vector3(creatureRotX, creature.transform.localEulerAngles.y, creatureRotZ);
		creatureRotX = Mathf.Clamp(creatureRotX, 0, 0);
		creatureRotZ = Mathf.Clamp(creatureRotZ, 0, 0);
	}
}
