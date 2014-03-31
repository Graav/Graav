using UnityEngine;
using System.Collections;

public class turretScript : MonoBehaviour {
	public Transform plasmaPrefab;
	private Transform woldManager;
	
	
	//counter for next turret fire
	private float timeUntillFire = float.MaxValue;
	private int delayIndex = 0;
	public float[] delayTimes; // default, fires every 1 second
	
	
	// Use this for initialization
	void Start () 
	{
		if(delayTimes.Length == 0)
		{
			delayTimes = new float[1];
			delayTimes[0] = 1.0f;
		}
		
		firePlasma();
		
	}
	
	void Awake()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(woldManager == null)
		{
			woldManager = GameObject.Find("WorldManager").transform;
		}
		
		if(timeUntillFire  != float.MaxValue && timeUntillFire > 0)
		{
			timeUntillFire -= Time.deltaTime;
		}
		else
		{
			firePlasma();	
		}
	}
	
	private void firePlasma()
	{
		//set the next delay time
		timeUntillFire = delayTimes[delayIndex];
		delayIndex = (delayIndex + 1) % delayTimes.Length;
		
		//create projectile
		Transform proj = (Transform)Instantiate(plasmaPrefab);		
		proj.parent = woldManager;
		proj.position = transform.position;
		proj.GetComponent<plasmaScript>().cannon = gameObject;
	}
}
