using UnityEngine;
using System.Collections;

//class that listens for input to rotate gravity, and makes sure that
//the player remains upright
public class GravController : MonoBehaviour
{
	private static Vector3 gravDown    = new Vector3(0, -1, 0);
	private static Vector3 gravUp      = new Vector3(0, 1, 0);
	private static Vector3 gravLeft    = new Vector3(-1, 0, 0);
	private static Vector3 gravRight   = new Vector3(1, 0, 0);
	private static Vector3 gravBack    = new Vector3(0, 0, -1);
	private static Vector3 gravForward = new Vector3(0, 0, 1);
	private static Vector3[] directions = {gravDown, gravUp, gravLeft, gravRight, gravBack, gravForward};

	//normal vector defining the player's movement plane
	//used for vector projection
	private static Vector3 planeNormal = new Vector3(0, 1, 0);
	
	private Vector3 prevVelocity;
	
	public AudioSource rotatorSfx;
	
	public Texture fullBarTex;
	public Texture rechargeBarTex;
	public Texture emptyBarTex;
	
	public float maxSlow = 0.5f;
	public float slowLength = 5.0f;
	public float rechargeTime = 5.0f;
	public float rechargeElapsed = 0f;
	private float timeSlowed = 0f;
	private float timeSpeed = 1.0f;
	private bool speedUp = false;

	private bool canRotate;

	void Start()
	{	
		canRotate = false;
		Time.timeScale = 1.0f;
	}

	void Update()
	{	
		if(!GameObject.FindWithTag("MainCamera").GetComponent<PauseMenu>().isPaused) {
			if(this.GetComponent<CharacterController>().isGrounded)
			{
				canRotate = true;
			}

			//rotate gravity to the left by projecting the forward vector onto the movement
			//plane, and crossing it with the plane normal
			if(Input.GetKeyDown(KeyCode.Q) && canRotate) 
			{
				canRotate = false;
				Vector3 projForward = projectVectorOntoPlane(planeNormal, transform.forward);
				Vector3 dir = Vector3.Cross(planeNormal, (projForward.normalized));
				rotateGravToDirection(transform.localPosition, dir);
			}

			//rotate gravity to the right
			if(Input.GetKeyDown(KeyCode.E) && canRotate)
			{	
				canRotate = false;
				Vector3 projForward = projectVectorOntoPlane(planeNormal, transform.forward);
				Vector3 dir = Vector3.Cross(planeNormal, (projForward.normalized) * -1);
				rotateGravToDirection(transform.localPosition, dir);
			}
		
			//pause functionality
			if(Input.GetKeyDown(KeyCode.Escape) && Application.loadedLevel != 0)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<PauseMenu>().togglePause();
			}
	
			//kill the player if either they fall out of the world
			
			Transform bounds = GameObject.FindGameObjectWithTag("MapBounds").transform;
			if(Vector3.Distance(transform.position, bounds.position) > bounds.localScale.x/2)
			{
				Application.LoadLevel(Application.loadedLevelName);
			}
			
			if(Input.GetKeyDown(KeyCode.LeftShift))
			{	
				attemptTimeSlow();
			}
			else if(timeSpeed != 1.0f)
			{
				
				if(!speedUp)
				{
					if(timeSpeed < maxSlow)
					{
						Time.timeScale = timeSpeed;
						timeSpeed -= getCancelSlow() *Time.deltaTime;
					}
					else
					{
						Time.timeScale = maxSlow;
						timeSlowed += getCancelSlow() *Time.deltaTime;
						if(timeSlowed >= slowLength)
						{
							timeSpeedUp();
						}
					}
				}
				else
				{
					if(timeSpeed < 1.0f)
					{
						Time.timeScale = timeSpeed;
						timeSpeed += getCancelSlow() * Time.deltaTime;
					}
					else
					{
						timeSpeed = 1.0f;
						Time.timeScale = timeSpeed;
						rechargeElapsed = rechargeTime;
					}
				}
			}
			else if(rechargeElapsed > 0)
			{
				rechargeElapsed -= getCancelSlow() * Time.deltaTime;
			}
			else
			{
				rechargeElapsed = 0;
			}
		}
	}
	
	void OnGUI()
	{
		//display time meter
		GUI.DrawTexture(new Rect(10f, Screen.height - 25f, Screen.width/5f, 10f), emptyBarTex);
		
		if(rechargeElapsed > 0)
		{
			GUI.DrawTexture(new Rect(10f, Screen.height - 25f, Screen.width/5f * (1.0f - (rechargeElapsed / rechargeTime)), 10f), rechargeBarTex);
		}
		else if(timeSlowed > 0)
		{
			GUI.DrawTexture(new Rect(10f, Screen.height - 25f, (Screen.width/5f) * (1.0f - (timeSlowed/slowLength)), 10f), fullBarTex);
		}
		else if (rechargeElapsed == 0 && Time.timeScale == 1.0f)
		{
			GUI.DrawTexture(new Rect(10f, Screen.height - 25f, Screen.width/5f, 10f), fullBarTex);
		}
	}
	
	//origin: the position of the player and the center of world rotation
	//newGrav: a vector defining the new "down" relative to the current down
	private void rotateGravToDirection(Vector3 origin, Vector3 newGrav)
	{
		if(!rotatorSfx.isPlaying)
		{
			rotatorSfx.Play();
		}
	
		//iterate through the axis unit vectors and find the closest one to newGrav
		int closestIndex = 0;
		float highestDot = -1;
		for(int x = 0; x < directions.Length; x++)
		{
			float currentDot = Vector3.Dot(directions[x], newGrav);
			if(currentDot > highestDot)
			{
				highestDot = currentDot;
				closestIndex = x;
			}
		}
	
		//tell the WorldManager to rotate everything to the new down
		GameObject worldManager = GameObject.Find("WorldManager");
		worldManager.GetComponent<WorldManager>().rotateAllEntities(origin, directions[closestIndex]);
	}
	
	//project the given vector onto a plane defined by the given normal
	private Vector3 projectVectorOntoPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - (Vector3.Dot (vector, planeNormal) * planeNormal);
	}
	
	private void attemptTimeSlow()
	{
		if(rechargeElapsed == 0 && Time.timeScale == 1.0f)
		{
			timeSpeed = 1.0f - Time.deltaTime;
			speedUp = false;
			Time.fixedDeltaTime = Time.fixedDeltaTime *Time.timeScale;
		}
		else if(Time.timeScale == maxSlow)
		{
			timeSpeedUp();
		}
	}
	
	private void timeSpeedUp()
	{
		timeSpeed = maxSlow;
		speedUp = true;
		timeSlowed = 0;
		Time.fixedDeltaTime = Time.fixedDeltaTime *Time.timeScale;
	}
		
	private float getCancelSlow()
	{
		return 1/Time.timeScale;
	}
}
