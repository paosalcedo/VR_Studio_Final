using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MovementScriptV2 : MonoBehaviour {
	
//	GameObject player;
//	public float avoidDist;
	Animator anim; 

	Transform player;
	Vector3 newDir;
	Vector3 newVec;
 //	public float avoidForce = 2f;
	float callTime;
	public TrailRenderer tr;
	public float kinematicDelay; //the larger this value is, the longer it takes for the bug to regain control after being thrown.
	public float minCallTime;
	public float minCallVolume;
	public float forwardForce = 50f;
//	public float stableForce = 50f;
	public float rotSpeed = 10f;
	public float dizzyRotSpeed = 50f;
//	public float rotCooldownValue;
//	public float hoverVertSpeed;
//	public float hoverHoriSpeed;
//	public float amplitude;
	public float moveToPlayerSpeed = 1f;
//	public float lengthOfDizziness;
	float dist;
	
	private Vector3 tempPos;

	Rigidbody rb;
	[SerializeField]float raycastRange = 5f;

	private Quaternion _lookRotation;
    private Vector3 _direction;

	public bool enableMic;
	public bool playerIsCalling;
	public bool grabbed;
	public bool bugWasThrownFast;
	public bool bugIsDizzy;

	void Start () {
		Debug.Log(minCallVolume);
 		rb = GetComponent<Rigidbody>();
 		player = GameObject.Find("Player").GetComponent<Player>().hmdTransform;
		tempPos = transform.position;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// !grabbed stops creature from moving when you grab it--because it uses Transform to move and not Rigidbody, 
		// hand.AttachObject() does not keep it from moving.
		// the crumbsInScene bool checks if there are any crumbs in the scene.

		//THIS IS FOR STANDARD MOVEMENT
		if (!grabbed && !MoveToCrumb.crumbsInScene) {//insert bool for crumbs here.)
			MoveForward ();
			GoToPlayer ();
		}

		//FOR MIC INPUT
//		Debug.Log("mic volume is: " + SpectrumController.desiredScale);

		if (SpectrumController.desiredScale > minCallVolume && enableMic) {
			callTime += Time.deltaTime;
			if (callTime > minCallTime) {
 				GameObject.Find ("MainBugPrefab").SendMessage ("PlayerCallOn");
			}
		} 
		else {
			callTime = 0f;
		}
//		Check for rigidbody velocity

//		Debug.Log("velocity is: " + GetComponent<Rigidbody>().velocity.magnitude);
//		Debug.Log("angularVelocity is: " + GetComponent<Rigidbody>().angularVelocity.magnitude);
	}

	void PlayerCallOn(){
		playerIsCalling = true;
 	}

	void PlayerCallOff ()
	{
		playerIsCalling = false;
 	}

	
	void MoveForward ()
	{
		if (!playerIsCalling && !bugIsDizzy) {
			transform.position += transform.forward * forwardForce * Time.deltaTime;
 
			//create the rotation we need to be in to look at the target
			_lookRotation = Quaternion.LookRotation (newVec);
 
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
		}

		if (bugIsDizzy) {
            Debug.Log("Bug is in dizzy state!");
 			anim.SetBool ("isDizzy", true);
			tr.enabled = true;
            transform.position += Vector3.zero;
        }
        else {
			tr.enabled = false; 
			anim.SetBool ("isDizzy", false);
		}

 		Ray ray = new Ray (transform.position, transform.forward);
		Debug.DrawRay (ray.origin, ray.direction * raycastRange, Color.red);
		
		RaycastHit rayHit = new RaycastHit ();		 
		
		if (Physics.Raycast (ray, out rayHit, raycastRange)) {
			if (rayHit.transform.tag == "Wall" || rayHit.transform.tag == "Ground") {
				newVec = rayHit.normal + Random.insideUnitSphere;				
			} 
		}     
	}

	void GoToPlayer ()
	{
//		Vector3 playerDir = player.position - transform.position;
		
//		Debug.Log(transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y);
 
		if (playerIsCalling) {
//			anim.SetBool("isTwisting", true); 
			dist = Vector3.Distance (player.position, transform.position);
			Vector3 playerDir = ((player.forward * 0.75f) + (player.up * 1.5f)) - transform.position;
			//ROTATE NECK animation state here.
			//make the creature look at the player
			_lookRotation = Quaternion.LookRotation (playerDir);
//			newVec = playerDir;
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
//			Debug.Log("rotation diff: " + (transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y));
			if ((transform.rotation.eulerAngles.y - _lookRotation.eulerAngles.y) >= -5f) {
//				anim.SetBool("isTwisting", false);
				transform.position += playerDir * moveToPlayerSpeed * Time.deltaTime;	
				if (dist > 2f) {
					anim.SetBool ("isDashing", true);
				} else {
					anim.SetBool ("isDashing", false);				
				}
			} 
		}
	}

	void RegainControl (){
        //bug regains control after kinematicDelay
        Invoke ("MakeKinematic", kinematicDelay);
	}

	void MakeKinematic ()
	{
		GetComponent<Rigidbody> ().isKinematic = true;
		anim.enabled = true;
       
    }

	public void StartDizziness (){
		bugWasThrownFast = true;
        
        Debug.Log("StartDizziness was called!");
		Invoke("StopDizziness", 5f);
 	}

	public void StopDizziness ()
	{
        Debug.Log("StopDizziness was called!");
		bugWasThrownFast = false;
		bugIsDizzy = false;
		anim.SetBool("isDizzy", false);
		tr.enabled = false;
	}


    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Wall" && bugWasThrownFast == true)
        {
            bugIsDizzy = true;
        }
    }

   

 
}
