using UnityEngine;
using System.Collections;

public class plasmaScript : MonoBehaviour 
{
	public float moveSpeed = 1.0f;
	public float rotSpeed = 1.0f;
	public Quaternion forwardDir;
	
	private Vector3 worldRot;
	
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
				
		Vector3 force;
		if(worldRot != transform.rotation.eulerAngles)
		{
			Quaternion diff = transform.rotation * Quaternion.Inverse(Quaternion.Euler(worldRot));
			forwardDir = diff * forwardDir;
			force = (forwardDir * Vector3.up).normalized;
		}
		else
		{
			force = (forwardDir * Vector3.up).normalized;
		}
		transform.localPosition += force * moveSpeed * Time.deltaTime;
		transform.Rotate(Vector3.one * rotSpeed * Time.deltaTime);
		worldRot = transform.rotation.eulerAngles; 
		
		//check if we are intersecting player
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		if(Vector3.Distance(player.transform.position, transform.position) <= GetComponent<SphereCollider>().radius * 2)
		{
			GameObject.FindGameObjectWithTag("Start").GetComponent<Cover>().triggerFadeOut();
			player.transform.position = new Vector3(0, 1, 0);
			GameObject.FindGameObjectWithTag("Start").GetComponent<Cover>().collider.isTrigger = false;
			wallElapsedTime = wallUpTime - Time.deltaTime;
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
