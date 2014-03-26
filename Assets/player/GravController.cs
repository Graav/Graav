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

	void Start()
	{

	}

	void Update()
	{	
		//rotate gravity to the left by projecting the forward vector onto the movement
		//plane, and crossing it with the plane normal
		if(Input.GetKeyDown(KeyCode.Q)) 
		{
			Vector3 projForward = projectVectorOntoPlane(planeNormal, transform.forward);
			Vector3 dir = Vector3.Cross(planeNormal, (projForward.normalized));
			rotateGravToDirection(transform.localPosition, dir);
		}

		//rotate gravity to the right
		if(Input.GetKeyDown(KeyCode.E))
		{	
			Vector3 projForward = projectVectorOntoPlane(planeNormal, transform.forward);
			Vector3 dir = Vector3.Cross(planeNormal, (projForward.normalized) * -1);
			rotateGravToDirection(transform.localPosition, dir);
		}

		//kill the player if either they fall out of the world
		if(transform.localPosition.magnitude > 125)
		{
			Application.LoadLevel(Application.loadedLevelName);
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
}
