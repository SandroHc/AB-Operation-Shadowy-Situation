using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public NetworkManager network;

	public GameObject[] panelList;
	public int currentPanel = 0;

	public Slider loadingBar;
	public Text loadingBarText;

	private AsyncOperation ao;

	void Start() {
		network = GetComponent<NetworkManager>();

		int savedSettings = PlayerPrefs.GetInt("QualitySettings", -1);
		int currentSettings = QualitySettings.GetQualityLevel();
		if(savedSettings == -1)
			PlayerPrefs.SetInt("QualitySettings", currentSettings);
		else if(savedSettings != currentSettings)
			updateQualitySettings(savedSettings, false);
	}

	void OnGUI() {
		if(ao != null) {
			if(ao.isDone) {
				loadingBar.gameObject.SetActive(false);
				ao = null;
			} else {
				loadingBar.gameObject.SetActive(true);
				loadingBar.value = ao.progress;
				loadingBarText.text = (ao.progress * 100).ToString("0") + "%";
			}
		}

		if(currentPanel == 1)
			drawOptions();
	}

	private void updatePanel(int newPanel) {
		if(newPanel < 0 || newPanel > panelList.Length - 1)
			return;

		for(int i=0; i < panelList.Length; i++)
			panelList[i].SetActive(i == newPanel);

		currentPanel = newPanel;
	}

	public void goToMainScreen() {
		updatePanel(0);
	}

	public void btnClickPlay() {
		ao = Application.LoadLevelAsync(1);
		ao.priority = 10;
	}

	public void btnClickMultiplayer() {
		updatePanel(2);
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

	/*		OPTIONS		*/
	public void drawOptions() {
		string[] names = QualitySettings.names;
		int current = QualitySettings.GetQualityLevel();

		Rect buttonRect = new Rect(Screen.width / 2 - (names.Length * 125) / 2, 250, 125, 35);
		for(int i = 0; i < names.Length; i++) {
			GUI.enabled = (i != current);
			if(GUI.Button(buttonRect, names[i])) updateQualitySettings(i);
			buttonRect.x += buttonRect.width;
		}
		GUI.enabled = true;
	}

	private void updateQualitySettings(int newSettings, bool applyExpensiveChanges = true) {
		QualitySettings.SetQualityLevel(newSettings, applyExpensiveChanges);
		PlayerPrefs.SetInt("QualitySettings", newSettings);
	}

	public void resetChanges() {
		updatePanel(0);
	}

	public void applyChanges() {
		PlayerPrefs.Save();
		updatePanel(0);
	}

	/*		MULTIPLAYER		*/
	public Text multiplayerList;

	public void btnClickStartServer() {
		network.StartServer("Test");
	}

	public void btnClickJoinServer() {
		network.JoinServer(network.hostList[0]);
	}

	public void btnClickRefreshList() {
		network.RefreshHostList();

		repopulateMultiplayerList();
	}

	private void repopulateMultiplayerList() {
		if(network.hostList == null || network.hostList.Length < 0) {
			multiplayerList.text = "There are no available servers.";
			return;
		}

		multiplayerList.text = "";

		for(int i=0; i < network.hostList.Length; i++) {
			multiplayerList.text += network.hostList[i].gameName + " - " + network.hostList[i].comment + " (" + network.hostList[i].connectedPlayers + "/" + network.hostList[i].playerLimit + " players)\n";
		}
	}

}
