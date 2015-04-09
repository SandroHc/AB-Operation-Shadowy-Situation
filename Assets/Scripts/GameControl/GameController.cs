using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameController : MonoBehaviour {
	public static GameController INSTANCE { get; private set; }
	private new GUITexture guiTexture;

	public static TextManager textManager;
	public static AudioManager audioManager;
	public static SpriteManager spriteManager;
	public static PrefabManager prefabManager;

	public static QuestManager questManager;
	public static CutsceneManager cutsceneManager;
	public static DialogueManager dialogueManager;
	public static MaterialManager materialManager;
	public static CraftingManager craftingManager;
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

	private static VignetteAndChromaticAberration vignette;
	private static bool vignetteEnabled;
	private static float vignetteIntensity;
	private static float vignetteTargetIntensity = 5f;
	private static float vignetteAberration;
	private static float vignetteTargetAberration = 7f;

	void Awake() {
		INSTANCE = this;
		guiTexture = GetComponent<GUITexture>();

		textManager = gameObject.GetComponent<TextManager>();
		audioManager = gameObject.GetComponent<AudioManager>();
		spriteManager = gameObject.GetComponent<SpriteManager>();
		prefabManager = gameObject.GetComponent<PrefabManager>();

		questManager = gameObject.GetComponent<QuestManager>();
		cutsceneManager = gameObject.GetComponent<CutsceneManager>();
		dialogueManager = gameObject.GetComponent<DialogueManager>();
		materialManager = gameObject.GetComponent<MaterialManager>();
		craftingManager = gameObject.GetComponent<CraftingManager>();
		enemyManager = gameObject.GetComponent<EnemyManager>();

		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		playerController = player.GetComponent<PlayerController>();
		playerPathfind = player.transform.FindChild("pathfind").GetComponent<PathfindHelper>();

		guiTexture.color = Color.black;
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

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


		// Try to obtain an instance of the camera vignette
		if(vignette == null) {
			vignette = Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration>();
		} else {
			if(isPaused) {
				vignette.intensity = Mathf.Lerp(vignette.intensity, vignetteTargetIntensity, Time.deltaTime * 10);
				vignette.chromaticAberration = Mathf.Lerp(vignette.chromaticAberration, vignetteTargetAberration, Time.deltaTime * 10);
			} else {
				vignette.intensity = Mathf.Lerp(vignette.intensity, vignetteIntensity, Time.deltaTime * 15);
				vignette.chromaticAberration = Mathf.Lerp(vignette.chromaticAberration, vignetteAberration, Time.deltaTime * 15);
			}
		}
	
		checkCancelInput();


		if(Input.GetKey(KeyCode.E))
			playerPathfind.updateLine();//setDestination(new Vector3(-100, 0, 0));

		if(Input.GetKeyDown(KeyCode.M))
			MaterialManager.increase(1000);

		if(Input.GetKeyDown(KeyCode.L)) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void OnGUI() {
		if(GUI.Button(new Rect(20, 100, 100, 25), "Spawn enemies"))
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

	private void checkCancelInput() {
		if(Input.GetKeyDown(InputManager.cancel)) {
			if(!isFocused) {
				setPaused(!isPaused);
			}

			cutsceneManager.cancelBtnClicked();
		}
	}

	private static void enterPause() {
		//Debug.Log("Pausing game");

		isPaused = true;

		INSTANCE.uiPause.SetActive(true);
		INSTANCE.uiCrosshair.SetActive(false);

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		// Save preferences in the event of a crash
		INSTANCE.OnApplicationQuit();

		
		if(vignette != null) {
			vignetteEnabled = vignette.enabled;
			if(!vignette.enabled) vignette.enabled = true;
			
			vignetteIntensity = vignette.intensity;
			vignetteAberration = vignette.chromaticAberration;
		}
	}

	private static void exitPause() {
		//Debug.Log("Resuming game");

		isPaused = false;

		INSTANCE.uiPause.SetActive(false);
		INSTANCE.uiCrosshair.SetActive(true);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private static void enterFocus(bool lockCursor = true) {
		//Debug.Log("Entering focus mode");

		INSTANCE.uiCrosshair.SetActive(false);

		Cursor.visible = !lockCursor;
		Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
	}

	private static void exitFocus() {
		//Debug.Log("Exiting focus mode");

		INSTANCE.uiCrosshair.SetActive(!isPaused);

		Cursor.visible = isPaused;
		Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
	}

	void OnApplicationQuit() {
		savePlayerPos();
		PlayerPrefs.Save();
	}

	private void loadPlayerPos() {
		// Position
		playerController.transform.position = new Vector3(PlayerPrefs.GetFloat("player_pos_x"), PlayerPrefs.GetFloat("player_pos_y"), PlayerPrefs.GetFloat("player_pos_z"));
		// Rotation
		playerController.transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("player_rot_x"), PlayerPrefs.GetFloat("player_rot_y"), PlayerPrefs.GetFloat("player_rot_z"));
	}

	private void savePlayerPos() {
		Vector3 position = playerController.transform.position;
		PlayerPrefs.SetFloat("player_pos_x", position.x);
		PlayerPrefs.SetFloat("player_pos_y", position.y);
		PlayerPrefs.SetFloat("player_pos_z", position.z);

		Vector3 rotation = playerController.transform.eulerAngles;
		PlayerPrefs.SetFloat("player_rot_x", rotation.x);
		PlayerPrefs.SetFloat("player_rot_y", rotation.y);
		PlayerPrefs.SetFloat("player_rot_z", rotation.z);
	}
}