using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float gravity = -9.81f;

	public float moveSpeed;
	public float jumpSpeed;
	public bool isFlying = false;
	public bool isSprinting = false;
	public bool isCrouching = false;
	public bool isJumping = false;
	private float jumpCurrent;
	private float jumpFinish;

	private Vector3 spawnLocation = new Vector3(0, 1, 0);
	private float charHeight;

	private CharacterController controller;
	private GameController gameController;
	private Animator animator;

	private Vector3 lastPos; // Used by the animator to get the speed

	void Start() {
		controller = GetComponent<CharacterController>();
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
		animator = GetComponent<Animator>();

		charHeight = controller.height;
	}

	void Awake() {
		lastPos = transform.position;
	}

	void Update() {
		isSprinting = Input.GetButton("Sprint");
		isCrouching = Input.GetButton("Crouch");

		// Jumping check
		if(!isJumping) {
			if(Input.GetButtonDown("Jump")) {
				isJumping = true;
				
				jumpFinish = jumpSpeed;
				//movement.y += jumpSpeed;
			}
		} else if(controller.isGrounded)
			isJumping = false; // Reset jumping when the player is touching the ground

		// Update the camera position based on the crouching state
		float lastHeight = controller.height;
		float newHeight = isCrouching ? charHeight * .5f : charHeight;
		controller.height = Mathf.Lerp(controller.height, newHeight, 5 * Time.deltaTime);
		Vector3 newPosition = transform.position; // Fix vertical position; else, the player with fall though the ground
		newPosition.y += (controller.height - lastHeight) / 2;
		transform.position = newPosition;

		playFootstepSound();



		// Set the sneaking parameter to the sneak input.
		animator.SetBool("Sneaking", isCrouching);
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

		animator.SetFloat("Speed", Vector3.Distance(lastPos, transform.position) * 4);
		lastPos = transform.position;
	}
	
	private void HandleMovement() {
		// Movement calculations
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if(controller.isGrounded && isCrouching)
			movement *= .5f;

		if(movement.z > 0) { // If the movement is forward
			if(controller.isGrounded && (isSprinting && !isCrouching)) // Only sprint if not crouching
				movement.z *= 2f; // Double forward movement speed
		}

		movement *= moveSpeed; // Ajust movement

		if(jumpCurrent + .05f < jumpFinish) {
			jumpCurrent = Mathf.Lerp(jumpCurrent, jumpFinish, 20 * Time.fixedDeltaTime);
			movement.y += jumpCurrent;
		} else
			jumpCurrent = jumpFinish = 0; // Reset jump

		movement = transform.TransformDirection(movement); // Transforms local coords intro global ones
		if(!isFlying) movement.y += gravity; // Add the gravity, if not flying

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
			case Tags.groundWood:	audio.clip = GameController.audioManager.footstepWood; break;
			case Tags.groundGrass:	audio.clip = GameController.audioManager.footstepGrass; break;
			}
	}

	private void playFootstepSound() {
		if(audio.isPlaying || gameController.stopMovement()) return; // If there is a footstep sound playing, let it finish before changing to a new one
	
		//Check if the player is moving and touching the ground...
		if(controller.isGrounded && Mathf.Round(Mathf.Abs(controller.velocity.x + controller.velocity.y + controller.velocity.z)) > 2) {
			// And play it
			audio.Play();
		}
	}
}
