using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public static AudioManager audioManager;
	public static TextManager textManager;

	public bool isPaused = false;
	private bool isInteracting = false;
	
	public float fadeSpeed;
	public bool fade = true;
	private bool fadeIn = false; // Fade to black (true); or fade from black (false)

	void Awake() {
		Screen.lockCursor = true;

		// Reset the fader
		fade = true;
		fadeIn = false;
		guiTexture.color = Color.black;
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

		audioManager = gameObject.GetComponent<AudioManager>();
		textManager = gameObject.GetComponent<TextManager>();
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

		handleEscapeKey();
	}
	
	void OnGUI() {
		if(isPaused) drawPauseGui();
	}

	private void drawPauseGui() {
		int btnWidth = 150;
		int btnHeight = 30;
		int screenWidth = Screen.width / 2 - btnWidth / 2;
		int screenHeight = Screen.height / 2 - (2 * btnHeight + 10 /* spacers */) / 2;
		
		if(GUI.Button(new Rect(screenWidth, screenHeight, btnWidth, btnHeight), "Main Menu")) {
			fade = true;
			Application.LoadLevel(0);
		}
		
		if(GUI.Button(new Rect(screenWidth, screenHeight + 40, btnWidth, btnHeight), "Exit")) {
			fade = true;
			Application.Quit();
		}
	}

	public bool stopMovement() {
		return isPaused || isInteracting;
	}

	private void handleEscapeKey() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(isInteracting) {
				isInteracting = false;
			} else {
				isPaused = !isPaused;
			}
		}
		
		Screen.lockCursor = !stopMovement();
	}

	public void setInteracting(bool state) {
		isInteracting = state;
	}

	public bool getInteracting() {
		return isInteracting;
	}

	public void toggleInteracting() {
		isInteracting = !isInteracting;
	}
}