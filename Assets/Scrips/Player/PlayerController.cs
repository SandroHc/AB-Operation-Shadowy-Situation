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

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == Tags.ground)
			isJumping = false;
	}
}
