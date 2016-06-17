using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamerMovement : MonoBehaviour {

	public float smoothing = 2f;
	public bool Focused = false;
	public Transform target;

	float zoom = 8.5f;
	float maxZoom = 2f;
	float defaultZoom;
	float countin,countout = 0f;

	float y_Max = 6.5f;
	float y_Min = 3f;
	float Y,X = 0f;
	float Ysensitivity = 1/5f;
	float Xsensitivity = 4f;

	Vector3 TargetPosChange;
	Vector3 TargetPos;
	Vector3 targetCamPos;
	PlayerMovement playermovement;
	Vector3 range;
	Ray nextmov;
	Ray UnderTerrain;
	RaycastHit pointHit;
	int layerMask;
	public bool iscolliding = false;



	void Start()
	{
		//target = GameObject.Find("SweepyCenter").transform;
		TargetPos = target.position;
		layerMask = (1 << 11);
		layerMask = ~layerMask;
		defaultZoom = zoom;
	}
	
	void FixedUpdate()
	{   //input
		if (!Focused)
		{
			if(Input.GetButton("Zoom"))
			{
				countout = 0; 
				zoom = Mathf.Lerp(zoom,maxZoom,countin);
				countin += Time.deltaTime;
			} 
			else if(zoom != defaultZoom){
				countin = 0f; 
				zoom = Mathf.Lerp(zoom,defaultZoom,countout);
				countout += Time.deltaTime;
			}

			Y = Input.GetAxis("Mouse Y") * Ysensitivity;
			X = Input.GetAxis("Mouse X") * Xsensitivity;

			//movement
			TargetPosChange = target.position - TargetPos;
			TargetPos = target.position;
			targetCamPos = transform.position + TargetPosChange - (Vector3.up * Y/2f);		
			targetCamPos.y = Mathf.Clamp(targetCamPos.y,TargetPos.y + y_Min,TargetPos.y + y_Max);
			Vector3 flatposc = Vector3.ProjectOnPlane(transform.position,Vector3.up);
			Vector3 flatpost = Vector3.ProjectOnPlane(target.position,Vector3.up);
			float dist = Vector3.Distance(flatpost,flatposc);
			if (dist != zoom)
			{
				targetCamPos = targetCamPos + ((flatpost - flatposc).normalized * (dist - zoom));
			}
			transform.position = targetCamPos;


			//Rotation: X - move "around", Y/X - Look at variable Sweepy height
			if(X != 0){ transform.RotateAround(TargetPos,Vector3.up, X );}
		}

	}
}

