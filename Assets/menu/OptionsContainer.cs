using UnityEngine;
using System.Collections;

public class OptionsContainer : MonoBehaviour {

	public static OptionsContainer Instance;

	public float mouseSensitivityValue;				//Mouse sensitivity

	void Awake() {
		if(Instance) {
			DestroyImmediate(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
}
