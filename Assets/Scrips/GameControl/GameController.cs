using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static TextManager textManager;
	public static AudioManager audioManager;
	public static SpriteManager spriteManager;

	private bool isPaused = false;
	private bool isFocused = false;
	
	public float fadeSpeed;
	public bool fade = true;
	private bool fadeIn = false; // Fade to black (true); or fade from black (false)

	public GameObject uiPause;
	public GameObject uiCrosshair;

	void Start() {
		textManager = gameObject.GetComponent<TextManager>();
		audioManager = gameObject.GetComponent<AudioManager>();
		spriteManager = gameObject.GetComponent<SpriteManager>();

		guiTexture.color = Color.black;
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}

	void Awake() {
		Screen.lockCursor = true;

		// Reset the fader
		fade = true;
		fadeIn = false;
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

	public void setFocused(bool state) {
		isFocused = state;
		if(isFocused)
			enterFocus();
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
		Debug.Log("Pausing game");

		isPaused = true;

		uiPause.SetActive(true);
		uiCrosshair.SetActive(false);
		Screen.lockCursor = false;
	}

	private void exitPause() {
		Debug.Log("Resuming game");

		isPaused = false;

		uiPause.SetActive(false);
		uiCrosshair.SetActive(true);
		Screen.lockCursor = true;
	}

	private void enterFocus() {
		Debug.Log("Entering in focus mode");

		uiCrosshair.SetActive(false);
		Screen.lockCursor = true;
	}

	private void exitFocus() {
		Debug.Log("Exiting focus mode");

		uiCrosshair.SetActive(!isPaused);
		Screen.lockCursor = !isPaused;
	}
}