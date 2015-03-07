using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static GameController INSTANCE { get; private set; }

	public static TextManager textManager;
	public static AudioManager audioManager;
	public static SpriteManager spriteManager;

	public static QuestManager questManager;
	public static CutsceneManager cutsceneManager;
	public static DialogueManager dialogueManager;
	public static MaterialManager materialManager;
	public static EnemyManager enemyManager;

	public static PlayerController playerController;
	public static PathfindHelper playerPathfind;

	private static bool isPaused = false;
	private static bool isFocused = false;

	private bool canCancelFocus;
	
	public float fadeSpeed;
	public bool fade = true;
	private bool fadeIn = false; // Fade to black (true); or fade from black (false)

	public GameObject uiPause;
	public GameObject uiCrosshair;

	void Awake() {
		INSTANCE = this;

		textManager = gameObject.GetComponent<TextManager>();
		audioManager = gameObject.GetComponent<AudioManager>();
		spriteManager = gameObject.GetComponent<SpriteManager>();

		questManager = gameObject.GetComponent<QuestManager>();
		cutsceneManager = gameObject.GetComponent<CutsceneManager>();
		dialogueManager = gameObject.GetComponent<DialogueManager>();
		materialManager = gameObject.GetComponent<MaterialManager>();
		enemyManager = gameObject.GetComponent<EnemyManager>();

		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		playerController = player.GetComponent<PlayerController>();
		playerPathfind = player.transform.FindChild("pathfind").GetComponent<PathfindHelper>();

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
		if(GUI.Button(new Rect(20, 100, 100, 25), "Lock cursor"))
			Screen.lockCursor = true;

		if(GUI.Button(new Rect(20, 135, 100, 25), "Spawn enemies"))
			enemyManager.spawn(new Vector3(5, -24, 80));
	}
	
	public void btnClickedMainMenu() {
		fade = true;
		Application.LoadLevel(0);
	}

	public void btnClickedExit() {
		fade = true;
		Application.Quit();
	}

	public static bool getPaused() {
		return isPaused;
	}

	private static void setPaused(bool state) {
		isPaused = state;
		if(isPaused)
			enterPause();
		else
			exitPause();
	}


	public static bool getFocused() {
		return isFocused;
	}

	public static void setFocused(bool state, bool lockCursor = true, bool canCancelFocus = false) {
		isFocused = state;
		//this.canCancelFocus = canCancelFocus;
		if(isFocused)
			enterFocus(lockCursor);
		else
			exitFocus();
	}

	public static bool isPausedOrFocused() {
		return isPaused || isFocused;
	}

	private void handleCancelInput() {
		if(!isFocused && Input.GetKeyDown(InputManager.cancel)) {
			setPaused(!isPaused);
		}

		if(Input.GetKey(KeyCode.E))
			playerPathfind.setDestination(new Vector3(-100, 0, 0));
	}

	private static void enterPause() {
		//Debug.Log("Pausing game");

		isPaused = true;

		INSTANCE.uiPause.SetActive(true);
		INSTANCE.uiCrosshair.SetActive(false);
		Screen.lockCursor = false;

		// Save preferences in the event of a crash
		INSTANCE.OnApplicationQuit();
	}

	private static void exitPause() {
		//Debug.Log("Resuming game");

		isPaused = false;

		INSTANCE.uiPause.SetActive(false);
		INSTANCE.uiCrosshair.SetActive(true);
		Screen.lockCursor = true;
	}

	private static void enterFocus(bool lockCursor = true) {
		//Debug.Log("Entering focus mode");

		INSTANCE.uiCrosshair.SetActive(false);
		Screen.lockCursor = lockCursor;
	}

	private static void exitFocus() {
		//Debug.Log("Exiting focus mode");

		INSTANCE.uiCrosshair.SetActive(!isPaused);
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