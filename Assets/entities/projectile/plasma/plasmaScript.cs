using UnityEngine;
using System.Collections;

public class plasmaScript : MonoBehaviour 
{
	public float moveSpeed = 1.0f;
	public float rotSpeed = 1.0f;
	public Quaternion forwardDir;
	
	public GameObject cannon;
	
	//timers for reseting the player
	private float wallElapsedTime = 0.5f;
	private float wallUpTime = 0.5f;
	
	//timer for knowing how long we have been alive
	public float lifeTime = 5.0f;
	private float elapsedTime = 0;
	
	//timer for fade
	public float fadeTime = 1.0f;
	private float fadeElapsedTime;
	
	
	// Use this for initialization
	void Start () 
	{
		 fadeElapsedTime = fadeTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		elapsedTime += Time.deltaTime;
		//if we have been alive for lifetime, start fade
		if(elapsedTime >= lifeTime && fadeTime == fadeElapsedTime)
		{
			fadeElapsedTime = fadeTime - Time.deltaTime;
		}
		
		if(wallElapsedTime != wallUpTime)
		{
			wallElapsedTime -= Time.deltaTime;
			
			if(wallElapsedTime < 0)
			{				
				GameObject.FindGameObjectWithTag("Start").GetComponent<Cover>().collider.isTrigger = true;				
				wallElapsedTime = wallElapsedTime + 1;
			}
		}
				
		transform.localPosition += cannon.transform.up * moveSpeed * Time.deltaTime;
		transform.Rotate(Vector3.one * rotSpeed * Time.deltaTime);
		
		//check if we are intersecting player
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		if(Vector3.Distance(player.transform.position, transform.position) <= GetComponent<SphereCollider>().radius * 2)
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
		
		//if we need to fade, fade
		if(fadeElapsedTime != fadeTime)
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, fadeElapsedTime/ fadeTime); 
			fadeElapsedTime -= Time.deltaTime;
			if(fadeElapsedTime <= 0)
			{
				DestroyImmediate(gameObject);
			}
		}
	}
}
