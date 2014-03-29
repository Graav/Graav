using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour
{
	private Vector3 oldGrav;
	private Vector3 newGrav;
	private Vector3 origin;
	private bool rotating;
	private int rotateTicks;

	void Start()
	{
		oldGrav = new Vector3(0, -1, 0);
		rotating = false;
	}
	
	void Update()
	{
		if(rotating && rotateTicks < 30)
		{
			rotateTicks++;
			foreach(Transform child in transform)
			{
				child.RotateAround(origin, Vector3.Cross(oldGrav, newGrav), Vector3.Angle (oldGrav, newGrav) / 30);
			}
		}
		else if(rotating)
		{
			rotating = false;
			rotateTicks = 0;
		}
	}
	
	public void rotateAllEntities(Vector3 point, Vector3 grav)
	{
		if(!rotating)
		{
			origin = point;
			newGrav = grav;
			rotating = true;
		}
	}
}
