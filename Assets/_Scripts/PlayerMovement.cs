using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 5f;
	public AudioSource Action;
	public AudioClip[] Sweeps;
	public float TimeBetweenSweeps = 0.5f;
	public float FallTimer;
	public float JumpForce;
	public float MaxSpeed;
	//public GameController gamecontroller;
	public bool Reading = true;
	public AudioClip[] fartSounds;
	public List<Collider> SweepObj = new List<Collider>();
	public Slider healthGUI;
	public SkinnedMeshRenderer rend;
	public Color red;
	public float fartforce;
	public AudioClip damaged;
	public AudioClip footsteps;

	bool IsFalling = false;
	Vector3 movement;
	Animator anim;
	public float accel = 10;
	Rigidbody playerRigidbody;
	float Waittime = 1.0f;
	float LastFrameFallVelocity = 0f;
	float SprintTime = 0f;
	float multiplier = 1f;
	ParticleSystem fart;
	Material[] Mats;
	CamerMovement CamMove;
	float damp = 5;
	//Poopchute Specifics
	public Transform PipeTrans;
	Transform playa;

	void Awake()
	{	
		playerRigidbody = GetComponent<Rigidbody>();
		playa = GetComponent<Transform>();
		CamMove = GameObject.FindWithTag("CamBox").GetComponent<CamerMovement>();
	}

	void FixedUpdate() 
	{
				Waittime += Time.deltaTime;
				SprintTime += Time.deltaTime;
				float h = Input.GetAxisRaw ("Horizontal");
				float v = Input.GetAxisRaw ("Vertical");
				bool shift = Input.GetButton("Fire3");
				bool JumpAct = Input.GetButtonDown("Jump");
				bool Sweep = Input.GetButtonDown("Fire1");
				Move (h,v,shift);
	}
	
	void OnTriggerEnter(Collider obj)
	{   
		if (obj.tag == "Pipe"){PipeTrans = obj.transform;}
	}
	
	void Move (float h, float v, bool shift)
	{
		if(!shift){SprintTime = 0f; multiplier = 1f;}
		else 
		{
			multiplier = 1.5f;
			if(SprintTime > 10f) {multiplier = 2f;}
		}

		//h will give turning
		if (h != 0)
		{   
			float turn = h * Time.deltaTime;
			//if angle between pipes and turd < 90
			if (Vector3.Angle(transform.forward, PipeTrans.forward) <= 90)
			{
				//rotate around Y
				transform.RotateAround(transform.position, transform.up, turn * 100f);
			}
			else {transform.rotation = Quaternion.Lerp(transform.rotation,PipeTrans.rotation, 5 *Time.deltaTime);}
			Debug.Log("turning: " + turn);
		}
		//v will give forward speed
		if (v != 0)
		{
			//increase speed forward
			speed += (v * accel * multiplier * Time.deltaTime);
		}
		else {speed -= damp * Time.deltaTime;}//decrease speed toward zero

		//clamp
		speed = Mathf.Clamp(speed,0f,MaxSpeed);
		Debug.Log ("v: " + v + "moving: " + speed);
		playerRigidbody.velocity = Vector3.ProjectOnPlane(playerRigidbody.velocity, transform.forward) + (transform.forward * speed);
	}
}
	/*
	void CheckForJump(bool JumpAct){
		//When you Jump: Not on Ground but not falling until y vel is less than 0
		if (!IsFalling){
			if (playerRigidbody.velocity.y <= 0.02f){
				IsFalling = true;
				FallTimer = 0.0f;
			}
		}
		FallTimer += Time.deltaTime;
		//When you are falling and you're not floating, check for y vel = 0
		if (IsFalling && FallTimer > 0.1f){
			if (playerRigidbody.velocity.y >= -3f){	
				//print("Done Falling!    :playerRigidbody.velocity.y " + playerRigidbody.velocity.y);
				IsFalling = false;
				anim.SetBool("IsOnGround",true);
			}
			if (playerRigidbody.velocity.y - LastFrameFallVelocity >= 25f)
			{	
				TakeDamage((playerRigidbody.velocity.y - LastFrameFallVelocity)/4f);
			}
		}
		LastFrameFallVelocity = playerRigidbody.velocity.y;

	}

	void Jump (){
		playerRigidbody.AddExplosionForce(JumpForce, playerRigidbody.transform.position, 2.0f);
		anim.SetTrigger("Jump");
		anim.SetBool("IsOnGround",false);
		IsFalling = false;
	}
	IEnumerator PauseMove(float sec)
	{
		Reading = true; 
		yield return new WaitForSeconds(sec); 
		Reading = false;
	}*/


