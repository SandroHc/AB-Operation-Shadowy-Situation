using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject target;

	public float lookSpeed;
	private float rotationY;

	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
	}

	void Awake() {
		// Reset those variables, to prevent the camera from spinning when the cursor is locked
		Input.GetAxis("Mouse X");
		Input.GetAxis("Mouse Y");
	}

	void FixedUpdate() {
		if(!gameController.isPaused) {
			rotationY += Input.GetAxis("Mouse Y") * lookSpeed * Time.fixedDeltaTime;
			rotationY = Mathf.Lerp(rotationY, Mathf.Clamp(rotationY, -90f, 90f), lookSpeed * Time.fixedDeltaTime);
			transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);

			target.transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed * Time.fixedDeltaTime, 0);
		}
	}
}
