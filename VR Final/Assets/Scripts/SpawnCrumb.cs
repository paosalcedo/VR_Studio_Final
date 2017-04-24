using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SpawnCrumb : MonoBehaviour {

	Hand _hand;
	Transform player;
	[SerializeField]float angleMin;
	[SerializeField]float angleMax;

	 	// Use this for initialization

//	public bool isHovering;
	public static SpawnCrumb instance;

	void Start () {
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
		_hand = GetComponent<Hand> ();
// 		if (instance == null) {
//			instance = this;
//			DontDestroyOnLoad (this);
//		} else {
//			Destroy (gameObject);
//		}
	}
	
	// Update is called once per frame
	void Update () {
	
 		//Get where player is facing and create vector between hand and player.
		Vector3 playerLookDir = player.forward;

		Vector3 vecFromHandToPlayer = _hand.transform.position - player.position;

		//Get angle between hand and player's look direction.
		float angle = Vector3.Angle(playerLookDir, vecFromHandToPlayer);
		
		if (_hand.GetStandardInteractionButtonDown() == true && angle >= angleMin && angle <= angleMax) {
		
			GameObject crumb = Instantiate (Resources.Load ("Prefabs/Crumb") as GameObject);
			_hand.AttachObject (crumb);
			Debug.Log ("attaching object!");
			MoveToCrumb.instance.crumbs.Add(crumb);
//			MoveToCrumb.instance.crumbsInScene = true;

//			MoveToCrumb.instance.CrumbsAreInScene ();
		}
	}
}
