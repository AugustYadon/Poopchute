using UnityEngine;
using System.Collections;

public class FollowCamBox : MonoBehaviour {


	public Transform target;
	GameObject CamBox;
	CamerMovement camboxmove;
	Ray ToCamBox;
	RaycastHit pointHit;
	Vector3 norm;
    int layerMask;

	void Awake () 
	{
		//target = GameObject.Find("SweepyCenter").transform;
		CamBox = GameObject.FindWithTag("CamBox");
		camboxmove = CamBox.GetComponent<CamerMovement>();
		layerMask = (1 << 11);
		layerMask = ~layerMask;
	}

	void LateUpdate () 
	{
			ToCamBox.origin = target.position;
			norm = CamBox.transform.position - target.position;
			ToCamBox.direction = (norm).normalized;
			if(Physics.Raycast (ToCamBox, out pointHit, norm.magnitude, layerMask))//change this to avoid hitting the cambox too!!!
			{
				transform.position = pointHit.point;
			}
			else{transform.position = CamBox.transform.position;}
		
			if(!camboxmove.Focused)
			{
				Quaternion newrot = Quaternion.LookRotation(target.position - transform.position + new Vector3(0f,(target.position.y - transform.position.y)*0.5f,0f));
				transform.rotation = newrot;
			}
	}
}