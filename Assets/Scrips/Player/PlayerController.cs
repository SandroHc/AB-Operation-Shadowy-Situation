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
		if((!Network.isClient && !Network.isServer) || gameController.networkView.isMine) { // If we're not on a network OR we're on a network and it's our player
			HandleMovement();
		}
	}

	private void HandleMovement() {
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

	void OnGUI() {
		if(Network.isServer) {
			int i = 0;
			while (i < Network.connections.Length) {
				NetworkPlayer player = Network.connections[i];
				GUILayout.Label(player + ": " + Network.GetAveragePing(player) + " ms");
				i++;
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
