using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {
	public GameObject target;
	
	public float lookSpeed;
	private float rotationY;

	private CharacterController controller;
	
	public float headbobSpeed;
	public float headbobStepCounter;
	public Vector3 parentLastPos;
	public float headbobAmountX;
	public float headbobAmountY;
	public float eyeHeightRacio;
	public float currentAimRacio = 1; // Used when aiming weapons to define "aim level"

	bool effectsEnabled;

	float upSpeed = 9; // controls smoothing speed
	float dnSpeed = 20; // how fast the weapon returns to original position
	
	Vector3 angleInitial; // initial angle private
	Vector3 angleCurrent = Vector3.zero; // smoothed angle

	private float targetX; // unfiltered recoil angle private

	void Start() {
		controller = transform.parent.GetComponent<CharacterController>();

		effectsEnabled = PlayerPrefs.GetInt("settings_effects_enabled", 1) == 1;
		if(!effectsEnabled) // Default is the effects are enabled
			enableEffects(effectsEnabled);
	}
	
	void Awake() {
		// Reset those variables, to prevent the camera from spinning when the cursor is locked
		Input.ResetInputAxes();
		
		parentLastPos = transform.parent.position;

		// Save original angles
		angleInitial = transform.localEulerAngles;
	}
	
	void Update() {
		if(controller.isGrounded)
			headbobStepCounter += Vector3.Distance(parentLastPos, transform.parent.position) * headbobSpeed;

		Vector3 temp = transform.localPosition;
		temp.x = Mathf.Sin(headbobStepCounter) * headbobAmountX * currentAimRacio;
		temp.y = (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1 * currentAimRacio) + (transform.parent.localScale.y * eyeHeightRacio) - (transform.parent.localScale.y / 2);
		transform.localPosition = temp;

		// Update the last position to this frame
		parentLastPos = transform.parent.position;
		
		if(!GameController.isPausedOrFocused()) {
			rotationY += Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
			rotationY = Mathf.Lerp(rotationY, Mathf.Clamp(rotationY, -90f, 90f), lookSpeed * Time.deltaTime);
			transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);
			
			target.transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime, 0);
		}



		angleCurrent.x = Mathf.Lerp(angleCurrent.x, targetX, upSpeed * Time.deltaTime); // smooth movement a little
		transform.localEulerAngles = angleInitial - angleCurrent; // move the camera or weapon
		targetX = Mathf.Lerp(targetX, 0, dnSpeed * Time.deltaTime); // returns to rest


		if(Input.GetKeyDown(KeyCode.Alpha0))
		   enableEffects(!effectsEnabled);
	}

	public void recoil(float recoil) {
		targetX += recoil; // add recoil force
	}

	public void enableEffects(bool enabled) {
		Debug.Log((enabled ? "Enabling" : "Disabling") + " effects");

		gameObject.GetComponent<Antialiasing>().enabled = enabled;
		gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = enabled;
		gameObject.GetComponent<BloomOptimized>().enabled = enabled;

		effectsEnabled = enabled;
	}
}
