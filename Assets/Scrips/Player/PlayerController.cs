using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float movementSpeed = 10;
	public float jumpSpeed = 15000;

	private bool isJumping = false;

	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
	}

	void FixedUpdate() {
		if(!gameController.isPaused) {
			float movementHorizontal = Input.GetAxis("Horizontal");
			float movementVertical = Input.GetAxis("Vertical");
			Vector3 movement = new Vector3(movementHorizontal, 0, movementVertical);

			rigidbody.AddRelativeForce(movement * movementSpeed * Time.deltaTime);

			if(!isJumping && Input.GetButton("Jump")) {
				isJumping = true;
				rigidbody.AddForce(Vector3.up * jumpSpeed * Time.deltaTime);
			}
		}
	}

	public bool walking = true;
	public bool running = false;
	public bool jumping = false;

	void OnGUI() {
		bool walkToggle = GUI.Toggle(new Rect(10, 40, 120, 20), walking, "WALK");
		bool runToggle = GUI.Toggle(new Rect(10, 60, 120, 20), running, "RUN");
		bool jumpToggle = GUI.Toggle(new Rect(10, 80, 120, 20), jumping, "JUMP");

		if (walkToggle != walking)	{
			walkToggle = walking = true;
			runToggle  = running = false;
			jumpToggle = jumping = false;
		}

		if (runToggle != running) {
			walkToggle = walking = false;
			runToggle  = running = true;
			jumpToggle = jumping = false;
		}

		if (jumpToggle != jumping) {
			walkToggle = walking = false;
			runToggle  = running = false;
			jumpToggle = jumping = true;
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == Tags.ground)
			isJumping = false;
	}
}
