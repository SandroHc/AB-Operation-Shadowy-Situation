using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture logoTexture;
	public int currentGui = 0;

	private int btnWidth = 150;
	private int btnHeight = 35;
	private int screenWidth;
	private int screenHeight;

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

	public void drawOptions() {
		screenWidth = Screen.width / 2 - btnWidth / 2;
		screenHeight = Screen.height / 3 * 2;

		if(GUI.Button(new Rect(screenWidth - btnWidth, screenHeight + 120, btnWidth, btnHeight), "Cancel"))
			resetChanges();

		if(GUI.Button(new Rect(screenWidth + btnWidth, screenHeight + 120, btnWidth, btnHeight), "Apply"))
			applyChanges();
	}

	private void resetChanges() {
		currentGui = 0;
	}

	private void applyChanges() {
		currentGui = 0;
	}
}
