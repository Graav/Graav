using UnityEngine;
using System.Collections;

//gets the size of the object it is attached to and tiles its texture appropriately
public class TextureTile : MonoBehaviour
{
	public float scaleFactorX = 1;
	public float scaleFactorZ = 1;

	void Start()
	{
		//tile the given texture an appropriate number of times
		Vector3 scale = gameObject.transform.localScale;
		gameObject.renderer.material.mainTextureScale = new Vector2(scale.x * scaleFactorX, scale.z * scaleFactorZ);
	}
}
