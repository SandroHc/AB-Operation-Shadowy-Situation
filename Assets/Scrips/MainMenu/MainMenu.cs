using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture logoTexture;
	public int currentGui = 0;

	private int btnWidth = 150;
	private int btnHeight = 35;
	private int screenWidth;
	private int screenHeight;

	void Start() {
		int savedSettings = PlayerPrefs.GetInt("QualitySettings", -1);
		int currentSettings = QualitySettings.GetQualityLevel();
		if(savedSettings == -1)
			PlayerPrefs.SetInt("QualitySettings", currentSettings);
		else if(savedSettings != currentSettings)
			updateQualitySettings(savedSettings, false);
	}

	void OnGUI() {
		if(currentGui == 0) // Main Menu
			drawMainMenu();
		else if(currentGui == 1) // Options
			drawOptions();
		else
			currentGui = 0;
	}

	public void drawMainMenu() {
		screenWidth = Screen.width / 2 - btnWidth / 2;
		screenHeight = Screen.height / 2 - (5 * btnHeight + 40 /* spacers */) / 2;

		// Draw the game logo
		GUI.DrawTexture(new Rect(Screen.width / 2 - 219, screenHeight - 190, 437, 163), logoTexture);

		if(Input.GetKeyDown(KeyCode.Return) || GUI.Button(new Rect(screenWidth, screenHeight, btnWidth, btnHeight), "Play"))
			Application.LoadLevel(1);
		
		GUI.Button(new Rect(screenWidth, screenHeight + 40, btnWidth, btnHeight), new GUIContent("Multiplayer", "The multiplayer is not yet implemented!"));
		GUI.Label(new Rect(screenWidth + btnWidth + 10, screenHeight + 45, btnWidth * 2, btnHeight), GUI.tooltip);
		
		if(GUI.Button(new Rect(screenWidth, screenHeight + 80, btnWidth, btnHeight), "Hiscores"))
			Application.OpenURL("http://sandrohc.co.nf/ab/hiscores.php");
		
		if(GUI.Button(new Rect(screenWidth, screenHeight + 120, btnWidth, btnHeight), "Options"))
			currentGui = 1;
		
		if(GUI.Button(new Rect(screenWidth, screenHeight + 160, btnWidth, btnHeight), "Exit"))
			Application.Quit();
	}

	private bool walking = true;
	private bool running = false;
	private bool jumping = false;

	public void drawOptions() {
		screenWidth = Screen.width / 2 - btnWidth / 2;
		screenHeight = Screen.height / 3 * 2;

		string[] names = QualitySettings.names;
		int current = QualitySettings.GetQualityLevel();

		Rect buttonRect = new Rect(Screen.width / 2 - (names.Length * 125) / 2, 250, 125, 35);
		for(int i = 0; i < names.Length; i++) {
			GUI.enabled = (i != current);
			if(GUI.Button(buttonRect, names[i])) updateQualitySettings(i);
			buttonRect.x += buttonRect.width;
		}
		GUI.enabled = true;


		bool walkToggle = false; //GUI.Toggle(new Rect(10, 40, 120, 20), walking, "WALK");
		bool runToggle = false; //GUI.Toggle(new Rect(10, 60, 120, 20), running, "RUN");
		bool jumpToggle = false; //GUI.Toggle(new Rect(10, 80, 120, 20), jumping, "JUMP");
		
		if (walkToggle != walking)	{
			walkToggle = walking = true;
			runToggle  = running = false;
			jumpToggle = jumping = false;
		}
		
		if (runToggle != running) {
			walkToggle = walking = false;
			runToggle  = running = true;
			jumpToggle = jumping = false;
		}
		
		if (jumpToggle != jumping) {
			walkToggle = walking = false;
			runToggle  = running = false;
			jumpToggle = jumping = true;
		}


		if(GUI.Button(new Rect(screenWidth - btnWidth, screenHeight + 120, btnWidth, btnHeight), "Cancel"))
			resetChanges();

		if(GUI.Button(new Rect(screenWidth + btnWidth, screenHeight + 120, btnWidth, btnHeight), "Apply"))
			applyChanges();
	}

	private void updateQualitySettings(int newSettings, bool applyExpensiveChanges = true) {
		QualitySettings.SetQualityLevel(newSettings, applyExpensiveChanges);
		PlayerPrefs.SetInt("QualitySettings", newSettings);
	}

	private void resetChanges() {
		currentGui = 0;
	}

	private void applyChanges() {
		PlayerPrefs.Save();
		currentGui = 0;
	}
}
