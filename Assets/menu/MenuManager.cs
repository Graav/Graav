using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public GUISkin guiStyle;
	public string[] levelNames;							//List of level names

	private string currScreen;							//Current Screen
	private Vector2 scrollPosition = Vector2.zero;		//Used for scrollbar on level select

	//Options menu values
	private float mouseSensitivityValue;				//Mouse sensitivity

	void Start() {
		Screen.lockCursor = false;
		currScreen = "main";
		loadOptions();
	}
	
	void Update() {
		transform.RotateAround(new Vector3(0,0,0), new Vector3(0, 1, 0), -0.3f);
	}
	
	void OnGUI() 
	{		
		GUI.skin = guiStyle;
		switch(currScreen) {
		case "main": //Main Screen
			//Level Select
			if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.3f, Screen.width*0.4f, Screen.height*0.2f), "Level Select")) {
				currScreen = "levelSelect";
			}

			//Options
			if(GUI.Button(new Rect(Screen.width*0.3f, Screen.height*0.6f, Screen.width*0.4f, Screen.height*0.2f), "Options")) {
				currScreen = "options";
			}

			break;
		case "levelSelect": //Level Select Screen
			float buttonSpacing = Screen.height*0.25f;

			//Dynamically created level select
			scrollPosition = GUI.BeginScrollView(new Rect(Screen.width*0.3f, Screen.height*0.05f, Screen.width*0.42f, Screen.height*0.7f), scrollPosition, new Rect(0, 0, Screen.width*0.38f, buttonSpacing*levelNames.Length));

			for(int i = 0; i < levelNames.Length; i++) {
				if(GUI.Button(new Rect(0, i*(buttonSpacing), Screen.width*0.4f, Screen.height*0.2f), levelNames[i])) {
					loadGameLevel (i+1);
				}
			}

			GUI.EndScrollView();

			//Back button
			if(GUI.Button(new Rect(Screen.width*0.4f, Screen.height*0.8f, Screen.width*0.2f, Screen.height*0.15f), "Back")) {
				saveOptions();
				currScreen = "main";
			}

			break;
		case "options": //Options Screen
			//Mouse Sensitivity Slider
			GUI.Label (new Rect (Screen.width * 0.41f, Screen.height * 0.4f, Screen.width * 0.2f, 35), "Mouse Sensitivity");
			mouseSensitivityValue = GUI.HorizontalSlider(new Rect(Screen.width * 0.3f, Screen.height * 0.5f, Screen.width * 0.4f, 30), mouseSensitivityValue, 0.5F, 1.5F);


			//Back button
			if(GUI.Button(new Rect(Screen.width*0.4f, Screen.height*0.8f, Screen.width*0.2f, Screen.height*0.15f), "Back")) {
				saveOptions();
				currScreen = "main";
			}
			break;
		}

	}

	//Load a game level
	private void loadGameLevel(int level)
	{
		Application.LoadLevel(level);
	}

	//Load option values from OptionsContainer
	private void loadOptions() {
		mouseSensitivityValue = GameObject.Find ("OptionsManager").GetComponent<OptionsContainer>().mouseSensitivityValue;
	}
	
	//Save option values from OptionsContainer
	private void saveOptions() {
		GameObject.Find ("OptionsManager").GetComponent<OptionsContainer>().mouseSensitivityValue = mouseSensitivityValue;
	}
}
