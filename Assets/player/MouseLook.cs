using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float mouseOption;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	
	public Texture2D crosshair;
	public Texture2D blueArrow;
	public Texture2D orangeArrow;
	public AudioSource fireSfx;

	void Start ()
	{
		//Set Mouse Sensitivity
		mouseOption = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<OptionsContainer>().mouseSensitivityValue;
		sensitivityX = mouseOption * 15F;
		sensitivityY = mouseOption *15F;

		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
			
		//hide and anchor cursor to the center of the screen
		Screen.lockCursor = true;
	}

	void Update ()
	{
		if(!GameObject.FindWithTag("MainCamera").GetComponent<PauseMenu>().isPaused) {
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
			
			//fire a ray when you click and check to see if it hits a GravCube
			if(Input.GetMouseButtonDown(0))
			{
				fireSfx.Play();
			
				Transform camera = Camera.allCameras[0].transform;
				Ray ray = new Ray(camera.position, camera.forward);
				RaycastHit hit = new RaycastHit();
				
				if(Physics.Raycast(ray, out hit, 50))
				{
					if(hit.collider.gameObject.GetComponent<GravCube>())
					{
						hit.collider.gameObject.GetComponent<GravCube>().toggleGrav();
					}
				}
			}
		}
	}
	
	void OnGUI()
	{
		float xMin = (Screen.width / 2) - (crosshair.width / 2);
		float yMin = (Screen.height / 2) - (crosshair.height / 2);
		GUI.DrawTexture(new Rect(xMin, yMin, crosshair.width, crosshair.height), crosshair);
		
		float xMinArrow = (Screen.width / 2) - (blueArrow.width / 20);
		float yMinArrow = (Screen.height / 2) - (blueArrow.height / 20);
		float offset = (crosshair.width / 4);
		GUI.DrawTexture(new Rect(xMinArrow + offset, yMinArrow, blueArrow.width / 10, blueArrow.height / 10), blueArrow);
		GUI.DrawTexture(new Rect(xMinArrow - offset, yMinArrow, orangeArrow.width / 10, orangeArrow.height / 10), orangeArrow);
	}
}