using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float gravity = -9.81f;

	public float moveSpeed;
	public float jumpSpeed;
	public bool isJumping = false;
	public bool isSprinting = false;
	public bool isCrouching = false;

	private Vector3 spawnLocation = new Vector3(0, 1, 0);
	private float cameraY = 0f;

	private CharacterController controller;
	private GameController gameController;
	private AudioManager audioManager;

	void Start() {
		controller = GetComponent<CharacterController>();
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		audioManager = gameController.GetComponent<AudioManager>();
	}

	void Update() {
		isSprinting = Input.GetButton("Sprint");
		isCrouching = Input.GetButton("Crouch");

		// Update the camera position based on the crouching state
		//Vector3 cameraPos = Camera.main.transform.position;
		//cameraY = Mathf.Lerp(Camera.main.transform.position.y, isCrouching ? -.5f : 0, moveSpeed * Time.deltaTime);
		//cameraPos.y += cameraY;
		//Camera.main.transform.position = cameraPos; 

		playFootstepSound();
	}

	void FixedUpdate() {
		if(!gameController.stopMovement()) {
			if(Input.GetKeyUp(KeyCode.R)) { // Reset the player when the key R is released
				transform.position = spawnLocation;
				transform.eulerAngles = Vector3.zero;
			}
			
			if((!Network.isClient && !Network.isServer) || gameController.networkView.isMine) { // If we're not on a network OR we're on a network and it's our player
				HandleMovement();
			}
		}
	}
	
	private void HandleMovement() {
		// Jumping check
		if(!isJumping) {
			if(Input.GetButtonDown("Jump")) {
				isJumping = true;
				controller.Move(Vector3.up * jumpSpeed);
			}
		} else if(controller.isGrounded)
			isJumping = false; // Reset jumping when the player is touching the ground

		// Movement calculations
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if(controller.isGrounded && isCrouching)
			movement *= .5f;

		if(movement.z > 0) { // If the movement is forward
			if(controller.isGrounded && (isSprinting && !isCrouching)) // Only sprint if not crouching
				movement.z *= 2f; // Double forward movement speed
		} else if(movement.z < 0) { // If the movement is backward, slow down
			movement.z *= .5f;
		}

		movement *= moveSpeed;
		movement = transform.TransformDirection(movement); // Transforms local coords intro global ones

		movement.y += gravity; // Add the gravity

		controller.Move(movement * Time.fixedDeltaTime);
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

	void OnControllerColliderHit(ControllerColliderHit hit) {
		updateCurrentFootsteps(hit.collider);
	}

	private void updateCurrentFootsteps(Collider hit) {
			// ... and then change the footstep sound according to the ground type
			switch(hit.gameObject.tag) {
			case Tags.groundWood:	audio.clip = audioManager.footstepWood; break;
			case Tags.groundGrass:	audio.clip = audioManager.footstepGrass; break;
			}
	}

	private void playFootstepSound() {
		if(audio.isPlaying) return; // If there is a footstep sound playing, let it finish before changing to a new one
	
		//Check if the player is moving and touching the ground...
		if(controller.isGrounded && Mathf.Round(Mathf.Abs(controller.velocity.x + controller.velocity.y + controller.velocity.z)) > 2) {
			// And play it
			audio.Play();
		}
	}
}
