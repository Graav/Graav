using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour 
{
	public bool exitCover;
	
	private bool fadingToBlack;

	// Use this for initialization
	void Start() 
	{
		renderer.material.color = Color.black;
		fadingToBlack = false;
		if(exitCover)
		{
			Color c = renderer.material.color;
			c.a = 0.0f;
			renderer.material.color = c;
		}
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int szV = vertices.Length;
		Vector3[] newVerts = new Vector3[szV*2];
		Vector2[] newUv = new Vector2[szV*2];
		Vector3[] newNorms = new Vector3[szV*2];
		
		int j = 0;
		for (; j< szV; j++){
			// duplicate vertices and uvs:
			newVerts[j] = newVerts[j+szV] = vertices[j];
			newUv[j] = newUv[j+szV] = uv[j];
			// copy the original normals...
			newNorms[j] = normals[j];
			// and revert the new ones
			newNorms[j+szV] = -normals[j];
		}
		
		int[] triangles = mesh.triangles;
		int szT = triangles.Length;
		int[] newTris = new int[szT*2]; // double the triangles
		for (int i=0; i< szT; i+=3){
			// copy the original triangle
			newTris[i] = triangles[i];
			newTris[i+1] = triangles[i+1];
			newTris[i+2] = triangles[i+2];
			// save the new reversed triangle
			j = i+szT; 
			newTris[j] = triangles[i]+szV;
			newTris[j+2] = triangles[i+1]+szV;
			newTris[j+1] = triangles[i+2]+szV;
		}
		mesh.vertices = newVerts;
		mesh.uv = newUv;
		mesh.normals = newNorms;
		mesh.triangles = newTris; // assign triangles last!
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		if(!exitCover && renderer.material.color.a > 0)
		{
			Color c = renderer.material.color;
			c.a = c.a - 0.01f;
			renderer.material.color = c;
			
			if(renderer.material.color.a <= 0)
			{
				collider.isTrigger = true;
			}
		}
		
		if(exitCover && fadingToBlack && renderer.material.color.a < 1)
		{
			Color c = renderer.material.color;
			c.a = c.a + 0.01f;
			renderer.material.color = c;
			
			if(renderer.material.color.a >= 1)
			{
				Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
			}
		}
	}
	
	public void triggerFadeOut()
	{
		fadingToBlack = true;
		collider.isTrigger = false;
	}
}
