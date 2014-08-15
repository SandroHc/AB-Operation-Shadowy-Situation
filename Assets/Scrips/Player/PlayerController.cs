using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public float jumpSpeed;
	private bool isJumping = false;

	private GameController gameController;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
	}

	void FixedUpdate() {
		if(Input.GetKeyUp(KeyCode.R)) { // Reset the player when the key R is released
			rigidbody.position = rigidbody.velocity = rigidbody.angularVelocity =Vector3.zero; // Kill previous momentum
			rigidbody.rotation = Quaternion.Euler(Vector3.zero);
		}

		if((!Network.isClient && !Network.isServer) || gameController.networkView.isMine) { // If we're not on a network OR we're on a network and it's our player
			HandleMovement();
		}
	}

	private void HandleMovement() {
		if(!gameController.isPaused) {
			Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

			rigidbody.AddRelativeForce(movement * moveSpeed);
			
			if(!isJumping && Input.GetButton("Jump")) {
				isJumping = true;
				rigidbody.AddForce(Vector3.up * jumpSpeed);
			}
		}
	}

	void OnGUI() {
		if(Network.isServer) {
			for(int i = 0; i < Network.connections.Length; i++) {
				NetworkPlayer player = Network.connections[i];
				GUILayout.Label(player + ": " + Network.GetAveragePing(player) + " ms");
			}
		} else if(Network.isClient) {
			GUI.Label(new Rect(10, 10, 50, 20), Network.GetAveragePing(Network.player) + " ms");
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == Tags.ground)
			isJumping = false;
	}
}
