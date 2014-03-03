using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

	void Start()
	{
	
	}
	
	void Update()
	{
	
	}
	
	void OnTriggerStay(Collider c)
	{
		//when the player hits the goal, advance the level and reset the timescale (in case of level 4)
		if(c.gameObject.name.CompareTo ("Player") == 0 && (transform.position - c.gameObject.transform.position).magnitude < 1)
		{
			Time.timeScale = 1;
			Time.fixedDeltaTime = 0.02f;
			Application.LoadLevel((Application.loadedLevel + 1) % 5);
		}
	}
}
