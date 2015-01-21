using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static TextManager textManager;
	public static AudioManager audioManager;
	public static SpriteManager spriteManager;

	public static DialogueManager dialogueManager;
	public static MaterialManager materialManager;

	public static PlayerController playerController;

	private bool isPaused = false;
	private bool isFocused = false;
	
	public float fadeSpeed;
	public bool fade = true;
	private bool fadeIn = false; // Fade to black (true); or fade from black (false)

	public GameObject uiPause;
	public GameObject uiCrosshair;

	void Awake() {
		textManager = gameObject.GetComponent<TextManager>();
		audioManager = gameObject.GetComponent<AudioManager>();
		spriteManager = gameObject.GetComponent<SpriteManager>();

		dialogueManager = gameObject.GetComponent<DialogueManager>();
		materialManager = gameObject.GetComponent<MaterialManager>();

		playerController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();

		guiTexture.color = Color.black;
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

		Screen.lockCursor = true;

		// Reset the fader
		fade = true;
		fadeIn = false;

		loadPlayerPos();
	}

	void Update() {
		if(fade) {
			guiTexture.enabled = true;
			if(fadeIn) { // To black
				guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);

				if(guiTexture.color.a >= .975f) {
					guiTexture.color = Color.black;
					fade = fadeIn = false;
				}
			} else { // To transparent
				guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
				
				if(guiTexture.color.a <= .0125f) {
					guiTexture.color = Color.clear;
					fade = guiTexture.enabled = false;
					fadeIn = true;
				}
			}
		}
	
		handleCancelInput();
	}

	void OnGUI() {
		if(GUI.Button(new Rect(100, 100, 100, 25), "Lock cursor"))
			Screen.lockCursor = true;
	}
	
	public void btnClickedMainMenu() {
		fade = true;
		Application.LoadLevel(0);
	}

	public void btnClickedExit() {
		fade = true;
		Application.Quit();
	}

	public bool getPaused() {
		return isPaused;
	}

	private void setPaused(bool state) {
		isPaused = state;
		if(isPaused)
			enterPause();
		else
			exitPause();
	}


	public bool getFocused() {
		return isFocused;
	}

	public void setFocused(bool state, bool lockCursor = true) {
		isFocused = state;
		if(isFocused)
			enterFocus(lockCursor);
		else
			exitFocus();
	}

	public bool isPausedOrFocused() {
		return isPaused || isFocused;
	}

	private void handleCancelInput() {
		if(!isFocused && Input.GetButtonDown("Cancel")) {
			setPaused(!isPaused);
		}
	}

	private void enterPause() {
		//Debug.Log("Pausing game");

		isPaused = true;

		uiPause.SetActive(true);
		uiCrosshair.SetActive(false);
		Screen.lockCursor = false;

		// Save preferences in the event of a crash
		OnApplicationQuit();
	}

	private void exitPause() {
		//Debug.Log("Resuming game");

		isPaused = false;

		uiPause.SetActive(false);
		uiCrosshair.SetActive(true);
		Screen.lockCursor = true;
	}

	private void enterFocus(bool lockCursor = true) {
		//Debug.Log("Entering focus mode");

		uiCrosshair.SetActive(false);
		Screen.lockCursor = lockCursor;
	}

	private void exitFocus() {
		//Debug.Log("Exiting focus mode");

		uiCrosshair.SetActive(!isPaused);
		Screen.lockCursor = !isPaused;
	}

	void OnApplicationQuit() {
		savePlayerPos();
		PlayerPrefs.Save();
	}

	private void loadPlayerPos() {
		// Position
		playerController.gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("player_pos_x"), PlayerPrefs.GetFloat("player_pos_y", 1.1f), PlayerPrefs.GetFloat("player_pos_z"));
		// Rotation
		playerController.gameObject.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("player_rot_x"), PlayerPrefs.GetFloat("player_rot_y"), PlayerPrefs.GetFloat("player_rot_z"));
	}

	private void savePlayerPos() {
		Vector3 position = playerController.gameObject.transform.position;
		PlayerPrefs.SetFloat("player_pos_x", position.x);
		PlayerPrefs.SetFloat("player_pos_y", position.y);
		PlayerPrefs.SetFloat("player_pos_z", position.z);

		Vector3 rotation = playerController.gameObject.transform.eulerAngles;
		PlayerPrefs.SetFloat("player_rot_x", rotation.x);
		PlayerPrefs.SetFloat("player_rot_y", rotation.y);
		PlayerPrefs.SetFloat("player_rot_z", rotation.z);
	}
}