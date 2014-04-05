using UnityEngine;
using System.Collections;

public class OptionsContainer : MonoBehaviour {

	public static OptionsContainer Instance;

	public float mouseSensitivityValue;				//Mouse sensitivity
	public float volumeValue;						//volume sensitivity

	void Awake() {
		if(Instance) {
			DestroyImmediate(gameObject);
		} else {
			DontDestroyOnLoad(this);
			Instance = this;
		}
	}

	void Update () {
		AudioListener.volume = volumeValue;
	}
}
