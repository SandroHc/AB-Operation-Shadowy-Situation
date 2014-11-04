using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private NetworkManager network;

	public GameObject[] panelList;
	public int currentPanel = 0;

	public Image bgImg;
	public Sprite[] bgTextures;

	public Image logoImg;
	public Image logoImgOver;
	private RectTransform logoImgOverTransform;
	public float logoImgIn = 2;
	public float logoImgOut = 5;

	private bool loading = false;

	private AsyncOperation ao;

	void Awake() {
		bgImg.sprite = bgTextures[Random.Range(0, bgTextures.Length)];
	}

	void Start() {
		network = GetComponent<NetworkManager>();

		logoImgOverTransform = logoImgOver.GetComponent<RectTransform>();;

		int savedSettings = PlayerPrefs.GetInt("QualitySettings", -1);
		int currentSettings = QualitySettings.GetQualityLevel();
		if(savedSettings == -1)
			PlayerPrefs.SetInt("QualitySettings", currentSettings);
		else if(savedSettings != currentSettings)
			updateQualitySettings(savedSettings, false);
	}

	private float minScale = 1f;
	private float maxScale = 1.1f;
	private float currentScale = 1f;
	private bool increaseScale = false;

	void Update() {
		if(!loading) {
			if(increaseScale) {
				currentScale += Time.deltaTime / logoImgOut;
				if(currentScale >= maxScale) increaseScale = false;
			} else {
				currentScale -= Time.deltaTime * logoImgIn;
				if(currentScale <= minScale) increaseScale = true;
			}

			logoImgOverTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
		}
	}

	void OnGUI() {
		if(ao != null) {
			if(ao.isDone) {
				ao = null;
				loading = false;
			} else {
				logoImgOver.fillAmount = Mathf.Lerp(logoImgOver.fillAmount, 1 - ao.progress, Time.deltaTime * 1.6f);
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
		if(ao == null) {
			ao = Application.LoadLevelAsync(1);
			ao.priority = 10;

			logoImgOver.fillAmount = 1;
			logoImgOver.color = Color.gray;
			logoImgOverTransform.localScale = new Vector2(1,1);
			loading = true;
		}
	}

	public void btnClickMultiplayer() {
		updatePanel(2);
		populateMultiplayerList();
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

		populateMultiplayerList();
	}

	private void populateMultiplayerList() {
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
