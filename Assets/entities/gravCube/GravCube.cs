using UnityEngine;
using System.Collections;

public class GravCube : MonoBehaviour
{
	public bool antiGrav;		//antigrav cubes float up instead of down
	public bool fragile;		//fragile cubes can be clicked only twice
	public Texture cracks;		//texture2D of the cracks in broken cubes
	
	private int clicks;

	void Start()
	{
		clicks = 0;
	}
	
	public void Update()
	{
		//if there's a rigidbody on this cube, float
		if(rigidbody && !antiGrav)
		{
			rigidbody.velocity = Physics.gravity / 3.0f;
		}
		else if(rigidbody && antiGrav)
		{
			rigidbody.velocity = -Physics.gravity / 3.0f;
		}
	}
	
	public void toggleGrav()
	{
		//fragile cubes can only be toggled twice
		//inactive cubes glow and have no rigidbody for gravity to interact with
		if(clicks < 2 || !fragile)
		{
			if(GetComponent<Rigidbody>())
			{
				Destroy(rigidbody);
				gameObject.GetComponentInChildren<Light>().enabled = true;
			}
			else
			{
				gameObject.AddComponent<Rigidbody>();
				rigidbody.freezeRotation = true;
				rigidbody.useGravity = false;
				
				gameObject.GetComponentInChildren<Light>().enabled = false;
			}
			
			clicks++;
			if(clicks == 2 && fragile)
			{
				gameObject.renderer.material.mainTexture = cracks;
			}
		}
	}
}
