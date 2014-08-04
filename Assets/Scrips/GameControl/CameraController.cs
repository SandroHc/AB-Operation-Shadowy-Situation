using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject target;
	public float rotateSpeed;
	public float rotateVerticalSpeed;

	//private Vector3 offset;
	private float verticalAngle;
	private int minVerticalAngle = 13;
	private int maxVerticalAngle = 35;

	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();

		//offset = target.transform.position - transform.position;
	}

	void FixedUpdate() {
		if(!gameController.isPaused) {
			//if(offset.y >= -10 && offset.y <= -1) {
			//	float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
			//	if(scrollWheel != 0) {
			//		offset.y += scrollWheel;
			//		offset.y = Mathf.Clamp(offset.y, -10, -1);
			//	}
			//}

			// Player Y-axis rotation
			float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
			target.transform.Rotate(0, horizontal, 0);

			// Camera X-axis rotation
			verticalAngle -= Input.GetAxis("Mouse Y") * rotateVerticalSpeed * Time.deltaTime;
			verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
			Camera.main.transform.localEulerAngles = new Vector3(verticalAngle, 0, 0);
		}
	}
}
