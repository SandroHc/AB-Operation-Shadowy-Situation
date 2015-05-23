using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private AudioSource audioSource;

	public float gravity = 1f; // 9.81m/s in real world

	public float moveSpeed;
	public float jumpSpeed;
	public bool isFlying; // Not used
	public bool isSprinting;
	public bool isCrawling;
	public bool isJumping;

	public bool isMidJump;

	private float charHeight;

	private CharacterController controller;
	private Animator animator;

	private Vector3 lastPos; // Used by the animator to get the speed
	public float speed;

	public int monsterCount; // Total monsters killed
	public Text uiMonsterCount;

	void Awake() {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
		charHeight = controller.height;

		lastPos = transform.position;

		// Load total monster count
		monsterCount = PlayerPrefs.GetInt("monster_count", 0);

		// Start the monster counter UI
		uiMonsterCount.text = monsterCount.ToString();
	}

	void Update() {
		if(!GameController.isPausedOrFocused()) {
			isSprinting = Input.GetKey(InputManager.getKey("sprint"));
			isCrawling = Input.GetKey(InputManager.getKey("crawl"));
			isJumping = Input.GetKey(InputManager.getKey("jump")); // Not using GetKeyDown allows bunnyhops... so, intented feature?

			if(Input.GetKeyUp(KeyCode.Home)) { // Reset the player when the key R is released
				transform.position = Vector3.zero;
				transform.eulerAngles = Vector3.zero;
			}

			handleMovement();
		}

		// Calculate the speed by calculating the distance travelled from last frame, and multiply it for four
		speed = Vector3.Distance(lastPos, transform.position) * 4;
		animator.SetFloat("Speed", speed);

		// Set the sneaking parameter to the sneak input.
		animator.SetBool("Sneaking", isCrawling);

		playFootstepSound();

		lastPos = transform.position;
	}

	public float momentumY;
	
	private void handleMovement() {
		// Check if we should jump
		if(isJumping && !isMidJump) {
			momentumY = jumpSpeed;
			isMidJump = true;
		}

		if(controller.isGrounded) {
			isMidJump = false; // Reset this variable every time we touch the ground
			momentumY = 0; // Reset Y, we don't need gravity when touching the ground
		} else		
			momentumY -= gravity * Time.deltaTime; // Add gravity force


		// Movement calculations
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		if(isCrawling && controller.isGrounded)
			movement *= .5f;

		if(movement.z > 0) { // Check if there is movement forward
			if((isSprinting && !isCrawling) && controller.isGrounded) // Only sprint if not crouching
				movement.z *= 2f; // Double the movement speed forward
		}

		// Ajust movement speed
		movement *= moveSpeed;

		// Transforms local coords intro global ones
		movement = transform.TransformDirection(movement);

		// Setup the momentum stored from previous frames
		movement.y = momentumY;

		controller.Move(movement * Time.deltaTime);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		updateCurrentFootsteps(hit.collider);
	}

	private void updateCurrentFootsteps(Collider hit) {
		// Change the footstep sound according to the ground type
		switch(hit.gameObject.tag) {
			case Tags.groundWood:	audioSource.clip = GameController.audioManager.footstepWood; break;
			case Tags.groundGrass:	audioSource.clip = GameController.audioManager.footstepGrass; break;
			case Tags.groundMetal:	audioSource.clip = GameController.audioManager.footstepMetal; break;
			case Tags.groundWater:	audioSource.clip = GameController.audioManager.footstepWater; break;
		}
	}

	private void playFootstepSound() {
		if(audioSource.isPlaying || GameController.getPaused()) return; // If there is a footstep sound playing, let it finish before changing to a new one
	
		// Check if the player is moving and touching the ground...
		if(controller.isGrounded && (Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.y) + Mathf.Abs(controller.velocity.z) > 2)) {
			// And play it
			audioSource.Play();
		}
	}
}
