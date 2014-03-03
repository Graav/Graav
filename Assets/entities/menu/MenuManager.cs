using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GUIStyle style;

	void Start()
	{
		Screen.lockCursor = false;
	}
	
	void Update()
	{
		Camera.main.transform.RotateAround(new Vector3(0,0,0), new Vector3(0, 1, 0), -0.3f);
	}
	
	void OnGUI() 
	{		
		GUI.skin.button.fontSize = 50;

		
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width / 4, (Screen.height * 0.2f),Screen.width / 2,Screen.height / 8), "The Basics")) 
		{
			loadGameLevel(1);
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(Screen.width / 4, (Screen.height * 0.4f),Screen.width / 2,Screen.height / 8), "Cube City"))
		{
			loadGameLevel(2);
		}
		
		// Make the third button.
		if(GUI.Button(new Rect(Screen.width / 4, (Screen.height * 0.6f),Screen.width / 2,Screen.height / 8), "Your Friend"))
		{
			loadGameLevel(3);
		}
		
		// Make the fourth button.
		if(GUI.Button(new Rect(Screen.width / 4, (Screen.height * 0.8f),Screen.width / 2,Screen.height / 8), "Time and Space"))
		{
			loadGameLevel(4);
		}
	}
	
	private void loadGameLevel(int level)
	{
		audio.Play();
		Application.LoadLevel(level);
	}
}
