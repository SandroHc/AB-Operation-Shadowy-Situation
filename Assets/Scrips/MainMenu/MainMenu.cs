using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture logoTexture;

	public GameObject[] panelList;
	public int currentPanel = 0;

	private int btnWidth = 150;
	private int btnHeight = 35;
	private int screenWidth;
	private int screenHeight;

	private AsyncOperation ao;

	void Start() {
		int savedSettings = PlayerPrefs.GetInt("QualitySettings", -1);
		int currentSettings = QualitySettings.GetQualityLevel();
		if(savedSettings == -1)
			PlayerPrefs.SetInt("QualitySettings", currentSettings);
		else if(savedSettings != currentSettings)
			updateQualitySettings(savedSettings, false);
	}

	void OnGUI() {
		if(ao != null) {
			if(ao.isDone)
				ao = null;
			else
				GUI.Box(new Rect(0, 40, ao.progress * Screen.width, 40), "Loading");
		}
	}

	private void updatePanel(int newPanel) {
		if(newPanel < 0 || newPanel > panelList.Length - 1)
			return;

		for(int i=0; i < panelList.Length; i++)
			panelList[i].SetActive(i == newPanel);

		currentPanel = newPanel;
	}

	public void btnClickPlay() {
		ao = Application.LoadLevelAsync(1);ao = Application.LoadLevelAsync(1);
	}

	public void btnClickMultiplayer() {

	}

	public void btnClickHiscores() {
		Application.OpenURL("http://sandrohc.co.nf/ab/hiscores.php");
	}

	public void btnClickOptions() {
		updatePanel(1);
	}

	public void btnClickExit() {
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
		updatePanel(0);
	}

	private void applyChanges() {
		PlayerPrefs.Save();
		updatePanel(0);
	}
}
