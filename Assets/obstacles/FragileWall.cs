using UnityEngine;
using System.Collections;

public class FragileWall : MonoBehaviour
{

	void Start()
	{
	
	}
	
	void Update()
	{
	
	}
	
	void OnCollisionEnter(Collision c)
	{
		//when the player touches the wall, let gravity affect it
		if(c.gameObject.name.CompareTo("Player") == 0 && !rigidbody)
		{
			gameObject.AddComponent<Rigidbody>();
			rigidbody.angularDrag = 25;
			rigidbody.drag = 6;
		}
	}
}
