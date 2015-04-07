using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {
	public GameObject targetRotation;

	public Transform reference;
	public Vector3 offset;
	
	public float lookSpeed;
	private float rotationY;

	private CharacterController controller;

	public Vector3 lastPos;

	bool effectsEnabled;

	private bool isAiming = false;
	private float aimPrecision = .4f; // .4 = 60% slower movement; .1 = 90%; .9 = 10%

	void Start() {
		// Get the CharacterControler in the parent (the Player GO) of this parent GameObject
		controller = transform.parent.GetComponentInParent<CharacterController>();

		effectsEnabled = PlayerPrefs.GetInt("settings_effects_enabled", 1) == 1;
		if(!effectsEnabled) // Default is the effects are enabled. So, if this is false, apply the changes immediately
			enableEffects(effectsEnabled);
	}
	
	void Awake() {
		// Reset those variables, to prevent the camera from spinning when locking the cursor
		Input.ResetInputAxes();
		
		lastPos = transform.parent.position;
	}
	
	void Update() {
		handleMotion();

		// TODO Debug purposes
		if(Input.GetKeyDown(KeyCode.Alpha0))
			enableEffects(!effectsEnabled);

		if(!GameController.isPausedOrFocused())
			handleMouseLook();
	}

	private void handleMotion() {
		transform.parent.position = Vector3.Lerp(transform.parent.position, reference.transform.position + reference.transform.TransformDirection(offset), Time.deltaTime * 5);
	}

	private void handleMouseLook() {
		// Vertical rotation
		float rot = Input.GetAxis("Mouse Y") * lookSpeed;
		
		if(isAiming)
			rot *= aimPrecision;
		
		rot *= Time.deltaTime;
		rotationY += rot;
		
		rotationY = Mathf.Lerp(rotationY, Mathf.Clamp(rotationY, -90f, 90f), lookSpeed * Time.deltaTime);
		transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);
		
		
		// Horizontal rotation
		rot = Input.GetAxis("Mouse X") * lookSpeed;
		
		if(isAiming)
			rot *= aimPrecision;
		
		rot *= Time.deltaTime;
		targetRotation.transform.Rotate(0, rot, 0);
	}

	public void enableEffects(bool enabled) {
		Debug.Log((enabled ? "Enabling" : "Disabling") + " effects");
		PlayerPrefs.SetInt("settings_effects_enabled", enabled ? 1 : 0);

		gameObject.GetComponent<Antialiasing>().enabled = enabled;
		gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = enabled;
		gameObject.GetComponent<BloomOptimized>().enabled = enabled;

		effectsEnabled = enabled;
	}

	public void aimEnter() {
		isAiming = true;
	}

	public void aimExit() {
		isAiming = false;
	}
}
