using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool isPaused;

	// Use this for initialization
	void Start () {
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if(isPaused) {
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
			if(GUI.Button(new Rect(Screen.width * 0.10f, Screen.height * 0.10f, Screen.width * 0.35f, Screen.height * 0.55f), "Resume")) {
				togglePause();
			}
			
			if(GUI.Button(new Rect(Screen.width * 0.55f, Screen.height * 0.10f, Screen.width * 0.35f, Screen.height * 0.55f), "Main Menu")) {
				togglePause();
				Application.LoadLevel(0);	
			}
			if(GUI.Button(new Rect(Screen.width * 0.10f, Screen.height * 0.70f, Screen.width * 0.80f, Screen.height * 0.20f), "Restart Level")) {
				togglePause();
				Application.LoadLevel(Application.loadedLevel);
			}

		}
	}

	public void togglePause() {
		if(isPaused) {
			Screen.lockCursor = true;
			Time.timeScale = 1f;
			isPaused = false;
		} else {
			Screen.lockCursor = false;
			Time.timeScale = 0f;
			isPaused = true;    
		}
	}
}
