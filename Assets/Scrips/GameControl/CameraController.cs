using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject target;
	
	public float lookSpeed;
	private float rotationY;
	
	private GameController gameController;
	private CharacterController controller;
	
	public float headbobSpeed;
	public float headbobStepCounter;
	public Vector3 parentLastPos;
	public float headbobAmountX;
	public float headbobAmountY;
	public float eyeHeightRacio;
	public float currentAimRacio = 1; // Used when aiming weapons to define "aim level"
	
	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
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
		
		if(!gameController.stopMovement()) {
			rotationY += Input.GetAxis("Mouse Y") * lookSpeed * Time.fixedDeltaTime;
			rotationY = Mathf.Lerp(rotationY, Mathf.Clamp(rotationY, -90f, 90f), lookSpeed * Time.fixedDeltaTime);
			transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);
			
			target.transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed * Time.fixedDeltaTime, 0);
		}
	}
}
