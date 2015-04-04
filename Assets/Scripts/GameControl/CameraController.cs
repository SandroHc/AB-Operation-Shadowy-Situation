using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {
	public GameObject targetRotation;
	private Vector3 originalPosition;
	private Vector3 crawlingPosition;
	
	public float lookSpeed;
	private float rotationY;

	private CharacterController controller;

	public Vector3 lastPos;
	
	public float headbobSpeed = .5f;
	public float headbobStepCounter;
	public float headbobAmountX = .2f;
	public float headbobAmountY = .1f;

	public float eyeHeightRacio;

	bool effectsEnabled;

	private bool isAiming = false;
	private float aimPrecision = .4f; // .4 = 60% slower movement; .1 = 90%; .9 = 10%

	void Start() {
		// Get the CharacterControler in the parent (the Player GO) of this parent GameObject
		controller = transform.parent.GetComponentInParent<CharacterController>();

		effectsEnabled = PlayerPrefs.GetInt("settings_effects_enabled", 1) == 1;
		if(!effectsEnabled) // Default is the effects are enabled. So, if this is false, apply the changes immediately
			enableEffects(effectsEnabled);

		originalPosition = transform.parent.localPosition;
		crawlingPosition = new Vector3(0, .56f, .42f);
	}
	
	void Awake() {
		// Reset those variables, to prevent the camera from spinning when locking the cursor
		Input.ResetInputAxes();
		
		lastPos = transform.parent.position;
	}
	
	void Update() {
		//handleHeadbob();
		handleCrawling();

		// TODO Debug purposes
		if(Input.GetKeyDown(KeyCode.Alpha0))
			enableEffects(!effectsEnabled);

		if(!GameController.isPausedOrFocused())
			handleMouseLook();
	}

	private void handleHeadbob() {
		if(controller.isGrounded)
			headbobStepCounter += Vector3.Distance(lastPos, transform.parent.position) * headbobSpeed;
		
		Vector3 temp = transform.localPosition;
		temp.x = Mathf.Sin(headbobStepCounter) * headbobAmountX;
		temp.y = (Mathf.Cos(headbobStepCounter * 2) * headbobAmountY * -1) + (transform.parent.localScale.y * eyeHeightRacio) - (transform.parent.localScale.y / 2);
		transform.localPosition = temp;
		
		// Update the last position to this frame
		lastPos = transform.parent.position;
	}

	private void handleCrawling() {
		// If the player is walking/sprinting, take the camera a little forward
		originalPosition.z = Mathf.Lerp(originalPosition.z, GameController.playerController.speed > .2f ? .5f : .25f, Time.deltaTime * 10);

		transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, GameController.playerController.isCrawling ? crawlingPosition : originalPosition, Time.deltaTime * 5);
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
