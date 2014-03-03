using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]

public class MoveController : MonoBehaviour
{
	public AudioSource footsteps;
	public AudioSource powerup;

	private float speed = 3.5f;
	private float jumpHeight = 2.0f;
	private float maxVelocityChange = 2.5f;
	private bool grounded = false;

	void Start()
	{
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f;
		footsteps.pitch = 1.25f;
	}
	
	void FixedUpdate()
	{
		if(grounded)
		{
			//determine how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity = targetVelocity * speed;

			//apply force that attempts to hit the target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = targetVelocity - velocity;
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			
			//play footstep sounds
			if(!footsteps.isPlaying && projectVectorOntoPlane(new Vector3(0, 1, 0), velocity).magnitude > 0.25f)
			{
				footsteps.Play();
			}
			
			//jump
			if(Input.GetButton("Jump"))
			{
				Vector3 jumpVec = new Vector3(velocity.x, calculateJumpSpeed(), velocity.z);
				rigidbody.velocity = jumpVec;
			}
		}
		
		rigidbody.AddForce(Physics.gravity);
		grounded = false;
		
		if(Input.GetKey(KeyCode.Escape) && Application.loadedLevel != 0)
		{
			Time.timeScale = 1;
			Time.fixedDeltaTime = 0.02f;
			footsteps.pitch = 1.25f;
			Application.LoadLevel(0);
		}
	}
	
	void OnCollisionStay(Collision c)
	{
		//don't stick to walls
		Vector3 planeNormal = new Vector3(0, 1, 0);
		if(Vector3.Dot(c.contacts[0].normal, planeNormal) > 0.75f)
		{
			grounded = true;
		}
	}
	
	void OnTriggerEnter(Collider c)
	{
		//activate powerup!
		if(c.gameObject.name.CompareTo("sand_clock") == 0)
		{
			Destroy(c.gameObject);
			powerup.Play();
			
			Time.timeScale = 0.25f;
			Time.fixedDeltaTime = 0.0001f;
			footsteps.pitch = 0.7f;
		}
	}
	
	private float calculateJumpSpeed()
	{
		return Mathf.Sqrt(3 * jumpHeight * Physics.gravity.magnitude);
	}
	
	//project the given vector onto a plane defined by the given normal
	private Vector3 projectVectorOntoPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - (Vector3.Dot (vector, planeNormal) * planeNormal);
	}
}
