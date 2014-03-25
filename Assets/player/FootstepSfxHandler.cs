using UnityEngine;
using System.Collections;

public class FootstepSfxHandler : MonoBehaviour 
{

	public AudioSource[] footsteps;

	void start()
	{
		
	}

	void Update() 
	{
		if(this.GetComponent<CharacterController>().velocity.magnitude > 0.5f && this.GetComponent<CharacterController>().isGrounded)
		{
			playRandomFootstep();
		}
	}
	
	private void playRandomFootstep()
	{
		bool isPlaying = false;
		for(int x = 0; x < footsteps.Length; x++)
		{
			if(footsteps[x].isPlaying)
			{
				isPlaying = true;
			}
		}
		
		if(!isPlaying)
		{
			footsteps[Random.Range(0, footsteps.Length)].Play();
		}
	}
}