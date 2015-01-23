using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float gravity = 1f; // 9.81m/s in real world

	public float moveSpeed;
	public float jumpSpeed;
	public bool isFlying = false;
	public bool isSprinting = false;
	public bool isCrouching = false;
	public bool isJumping = false;

	public bool isMidJump = false;

	private Vector3 spawnLocation = new Vector3(0, 1.1f, 0);
	private float charHeight;

	private CharacterController controller;
	private Animator animator;

	private Vector3 lastPos; // Used by the animator to get the speed

	public int monsterCount; // Total monsters killed
	public Text uiMonsterCount;

	void Awake() {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

		charHeight = controller.height;

		lastPos = transform.position;

		// Load total monster count
		monsterCount = PlayerPrefs.GetInt("monster_count", 0);
	}

	void Update() {
		isSprinting = Input.GetButton("Sprint");
		isCrouching = Input.GetButton("Crouch");
		isJumping = Input.GetButton("Jump"); // Not using GetButtonDown allows bunnyhops... so, intented feature?

		if(!GameController.isPausedOrFocused()) {
			if(Input.GetKeyUp(KeyCode.Home)) { // Reset the player when the key R is released
				transform.position = spawnLocation;
				transform.eulerAngles = Vector3.zero;
			}

			HandleMovement();
		}

		// Update the camera position based on the crouching state
		float lastHeight = controller.height;
		float newHeight = isCrouching ? charHeight * .5f : charHeight;
		controller.height = Mathf.Lerp(controller.height, newHeight, 5 * Time.deltaTime);
		Vector3 newPosition = transform.position; // Fix vertical position; else, the player with fall though the ground
		newPosition.y += (controller.height - lastHeight) / 2;
		transform.position = newPosition;

		playFootstepSound();

		animator.SetFloat("Speed", Vector3.Distance(lastPos, transform.position) * 4);
		lastPos = transform.position;


		// Set the sneaking parameter to the sneak input.
		animator.SetBool("Sneaking", isCrouching);

		// Update the UI
		uiMonsterCount.text = monsterCount.ToString();
	}

	public float movementY;
	
	private void HandleMovement() {
		// Calculate only gravity
		if(GameController.isPausedOrFocused()) {
			if(controller.isGrounded)
				movementY = 0;
			else
				movementY -= gravity; // Add gravity force

			controller.Move((new Vector3(0, movementY, 0)) * Time.deltaTime);
			return;
		}

		// Movement calculations
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if(controller.isGrounded && isCrouching)
			movement *= .5f;

		if(movement.z > 0) { // Check if there is movement forward
			if(controller.isGrounded && (isSprinting && !isCrouching)) // Only sprint if not crouching
				movement.z *= 2f; // Double the movement speed forward
		}

		// Ajust movement speed
		movement *= moveSpeed;

		// Transforms local coords intro global ones
		movement = transform.TransformDirection(movement);

		// Check if we should jump
		if(isJumping && !isMidJump) {
			movementY = jumpSpeed;
			isMidJump = true;
		}

		if(controller.isGrounded) {
			isMidJump = false; // Reset this variable every time we touch the ground
			movementY = 0; // Reset Y, we don't need gravity when touching the ground
		} else
			movementY -= gravity; // Add gravity force

		movement.y = movementY;
		controller.Move(movement * Time.deltaTime);
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
			case Tags.groundMetal:	audio.clip = GameController.audioManager.footstepMetal; break;
			case Tags.groundWater:	audio.clip = GameController.audioManager.footstepWater; break;
		}
	}

	private void playFootstepSound() {
		if(audio.isPlaying || GameController.getPaused()) return; // If there is a footstep sound playing, let it finish before changing to a new one
	
		// Check if the player is moving and touching the ground...
		if(controller.isGrounded && (Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.y) + Mathf.Abs(controller.velocity.z) > 2)) {
			// And play it
			audio.Play();
		}
	}
}
