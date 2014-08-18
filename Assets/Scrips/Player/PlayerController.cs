using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public float jumpSpeed;
	public bool isJumping = false;

	private GameController gameController;
	private AudioManager audioManager;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		audioManager = gameController.GetComponent<AudioManager>();
	}

	void FixedUpdate() {
		if(!gameController.stopMovement()) {
			if(Input.GetKeyUp(KeyCode.R)) { // Reset the player when the key R is released
				rigidbody.position = rigidbody.velocity = rigidbody.angularVelocity =Vector3.zero; // Kill previous momentum
				rigidbody.rotation = Quaternion.Euler(Vector3.zero);
			}

			if((!Network.isClient && !Network.isServer) || gameController.networkView.isMine) { // If we're not on a network OR we're on a network and it's our player
				HandleMovement();
			}
		}
	}

	private void HandleMovement() {
			// Jumping check
			if(!isJumping && Input.GetButtonDown("Jump")) {
				isJumping = true;
				rigidbody.AddForce(Vector3.up * jumpSpeed);
			}

			// Movement calculations
			Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if(Input.GetButton("Sprint")) movement *= 1.5f;

			rigidbody.AddRelativeForce(movement * moveSpeed);
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

	void OnCollisionStay(Collision hit) {
		if(Tags.isGround(hit.gameObject.tag)) {
			isJumping = false;
			//transform.parent = hit.transform;
		} else {
			//transform.parent = null;
		}

			checkFootsteps(hit);
	}

	private void checkFootsteps(Collision hit) {
		if(audio.isPlaying) return; // If there is a footstep sound, let it finish before changing to a new one

		if(Mathf.Round(Mathf.Abs(rigidbody.velocity.x + rigidbody.velocity.y + rigidbody.velocity.z)) > 0) { //Check if the player is walking...
			// ... and then change the footstep sound according to the ground type
			switch(hit.gameObject.tag) {
			case Tags.groundWood:	audio.clip = audioManager.footstepWood; break;
			case Tags.groundGrass:	audio.clip = audioManager.footstepGrass; break;
			}

			// And play it
			audio.Play();
		}
	}
}
