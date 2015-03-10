using UnityEngine;
using System.Collections;

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

	public float maxRecoilX = -20;
	public float maxRecoilY = 20;
	public float recoilSpeed = 10;
	public float recoil = 0;
	
	void Start() {
		controller = transform.parent.GetComponent<CharacterController>();
	}
	
	void Awake() {
		// Reset those variables, to prevent the camera from spinning when the cursor is locked
		Input.ResetInputAxes();
		
		parentLastPos = transform.parent.position;
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
	}

	public void addRecoil(float value) {
		recoil += value;
	}

	public void doRecoil() {
		if (recoil > 0) {
			var maxRecoil = Quaternion.Euler(maxRecoilX + Random.Range(-10, 10), maxRecoilY + Random.Range(-10, 10), 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, recoilSpeed * Time.deltaTime); // This controls the actual recoil
		//	transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z); // This one also
			//equippedGun.model.transform.position = Vector3.Slerp(equippedGun.model.transform.position, new Vector3(equippedGun.model.transform.position.x + equippedGun.maxRecoilX, equippedGun.model.transform.position.y,equippedGun.model.transform.position.z), Time.deltaTime * equippedGun.recoilSpeed);
			// Don't you dare uncomment the above line!
			recoil -= Time.deltaTime;
		} else {
			recoil = 0;
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, recoilSpeed / 2 * Time.deltaTime);
		//	transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
		} 
	}
}
