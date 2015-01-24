using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	public GameObject[] panelList;
	public int currentPanel = 0;

	public RectTransform optionsPanel;

	public Image bgImg;
	public Sprite[] bgTextures;

	public Image logoImg;
	public Image logoImgOver;
	private RectTransform logoImgOverTransform;
	public float logoImgIn = 2;
	public float logoImgOut = 5;

	private bool loading = false;

	public Button[] btnList;

	private AsyncOperation ao;

	public GUIStyle labelStyle;

	void Awake() {
		bgImg.sprite = bgTextures[Random.Range(0, bgTextures.Length)];
	}

	void Start() {
		logoImgOverTransform = logoImgOver.GetComponent<RectTransform>();

		// Populate resolution settings list
		int size = Screen.resolutions.Length;
		resolutionList = new List<Resolution>(size);
		resolutionComboBox = new GUIContent[size];
		for(int i=0; i < size; i++) {
			Resolution res = Screen.resolutions[i];
			resolutionList.Add(res);
			resolutionComboBox[i] = new GUIContent(res.width + "x" + res.height);
		}

		int index = resolutionList.IndexOf(Screen.currentResolution);
		if(index > 0) // Prevent invalid index to be set
			resolutionControl.SetSelectedItemIndex(index);

		// Populate quality settings list
		size = QualitySettings.names.Length;
		qualityComboBox = new GUIContent[size];
		for(int i=0; i < size; i++) {
			qualityComboBox[i] = new GUIContent(QualitySettings.names[i]);
		}

		// Select the current quality setting on the ComboBox
		qualityControl.SetSelectedItemIndex(QualitySettings.GetQualityLevel());

		// Create a style to be used by the ComboBoxes
		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background = Texture2D.blackTexture;
		listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left = 10;
		listStyle.padding.right = 10;
		listStyle.padding.top = 2;
		listStyle.padding.bottom = 2;
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

		if(fetchingKeyCode) {
			tempKeyCode = InputManager.fetchKey();

			if(tempKeyCode != KeyCode.None) {
				//Debug.Log("Before: " + currentInput);
				// Update the KeyCode, to be updated in the label
				currentInput = tempKeyCode;

				// And save it to the PlayerPrefs to be loaded
				InputManager.saveKey(currentInputName, (int) tempKeyCode);

				// And reset the fetching state
				fetchingKeyCode = false;
			}
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

			foreach(Button btn in btnList)
				btn.gameObject.SetActive(false);
		}
	}

	public void btnClickHiscores() {
		Application.OpenURL("http://sandrohc.lixter.com/ab");
	}

	public void btnClickOptions() {
		updatePanel(1);
	}

	public void btnClickExit() {
		Application.Quit();
	}

	/*		OPTIONS		*/
	List<Resolution> resolutionList;

	private GUIContent[] resolutionComboBox;
	private GUIContent[] qualityComboBox;

	private ComboBox resolutionControl = new ComboBox();
	private ComboBox qualityControl = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();

	public void drawOptions() {
		// Screen settings
		resolutionControl.SetSelectedItemIndex(resolutionControl.List(new Rect(110, 110, 150, 25), resolutionComboBox[resolutionControl.GetSelectedItemIndex()], resolutionComboBox, listStyle));

		Resolution selectedRes = resolutionList[resolutionControl.GetSelectedItemIndex()];
		if(!Screen.currentResolution.Equals(selectedRes))
			setResolution(selectedRes);

		bool fullscreen = GUI.Toggle(new Rect(275, 110, 150, 25), Screen.fullScreen, "Set fullscreen");
		Debug.Log(fullscreen + " vs " + Screen.fullScreen);
		if(fullscreen != Screen.fullScreen)
			Screen.fullScreen = fullscreen;

		// Quality settings
		qualityControl.SetSelectedItemIndex(qualityControl.List(new Rect(110, 150, 150, 25), qualityComboBox[qualityControl.GetSelectedItemIndex()], qualityComboBox, listStyle));

		if(!QualitySettings.GetQualityLevel().Equals(qualityControl.GetSelectedItemIndex()))
			setQuality(qualityControl.GetSelectedItemIndex(), false);

		// Input settings
		drawOptionsInput();
	}

	private KeyCode currentInput;
	private string currentInputName;

	private KeyCode tempKeyCode;
	private bool fetchingKeyCode = false;

	public void drawOptionsInput() {
		drawInputOption("Fire 1", "fire1", ref InputManager.fire1);
		drawInputOption("Fire 2", "fire2", ref InputManager.fire2);
		drawInputOption("Fire 3", "fire3", ref InputManager.fire3);
		drawInputOption("Reload", "reload", ref InputManager.reload);
		drawInputOption("Interact", "interact", ref InputManager.interact);
		drawInputOption("Jump", "jump", ref InputManager.jump);
		drawInputOption("Sprint", "sprint", ref InputManager.sprint);
		drawInputOption("Crouch", "crouch", ref InputManager.crouch);
		drawInputOption("Submit", "submit", ref InputManager.submit);
		drawInputOption("Cancel", "cancel", ref InputManager.cancel);

		// Reset values after each draw call
		inputY = 200;
		inputX = 110;
	}

	int inputY = 200;
	int inputX = 110;
	int inputPadding = 35;

	private void drawInputOption(string label, string keyName, ref KeyCode key) {
		GUI.Label(new Rect(inputX, inputY, 75, 25), label, labelStyle);
		if(GUI.Button(new Rect(inputX + 95, inputY, 125, 25), key.ToString())) {
			currentInput = key;
			currentInputName = keyName;
			fetchingKeyCode = true;
		}

		inputY += inputPadding;
	}

	private void setResolution(Resolution res) {
		setResolution(res.width, res.height);
	}

	private void setResolution(int width, int height) {
		Screen.SetResolution(width, height, Screen.fullScreen);
	}

	private void setQuality(int newSettings, bool applyExpensiveChanges = true) {
		QualitySettings.SetQualityLevel(newSettings, applyExpensiveChanges);
	}

	public void resetChanges() {
		// TODO Rollback cahnges to PlayersPrefs
		updatePanel(0);
	}

	public void applyChanges() {
		PlayerPrefs.Save();
		updatePanel(0);
	}
}