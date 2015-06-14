using UnityEngine;
using UnityEngine.UI;
using Kender.uGUI;

public class PanelGraphics : MonoBehaviour {
	public ComboBox comboBoxResolution;
	public ComboBox comboBoxQuality;

	public Toggle fullscreen;
	public Toggle postProcessEffects;

	public Slider mouseSensitivity;
	public Text mouseSensitivityText;

	public void Start() {
		// Populate resolution settings list
		int size = Screen.resolutions.Length;
		ComboBoxItem[] list = new ComboBoxItem[size];
		int resIndex = 0;

		ComboBoxItem item;

		for(int i = 0; i < size; i++) {
			Resolution res = Screen.resolutions[i];
			if(res.Equals(Screen.currentResolution))
				resIndex = i;

			item = new ComboBoxItem(res.width + "x" + res.height);
			item.OnSelect += () => {
				setResolution(res);
			};

			list[i] = item;
		}

		comboBoxResolution.AddItems(list);
		comboBoxResolution.SelectedIndex = resIndex;

		// Populate quality settings list
		size = QualitySettings.names.Length;
		list = new ComboBoxItem[size];
		resIndex = 0;


		for(int i = 0; i < size; i++) {
			string name = QualitySettings.names[i];
			if(i == QualitySettings.GetQualityLevel())
				resIndex = i;

			item = new ComboBoxItem(name);
			item.OnSelect += () => {
				setQuality(i);
			};

			list[i] = item;
		}

		comboBoxQuality.AddItems(list);
		comboBoxQuality.SelectedIndex = resIndex;

		// Load the fullscreen toggle value
		fullscreen.isOn = Screen.fullScreen;

		// Load the post-process effects value
		postProcessEffects.isOn = PlayerPrefs.GetInt("settings_effects_enabled", 1) == 1;

		mouseSensitivity.value = PlayerPrefs.GetFloat("mouse_sensitivity", 1);
		mouseSensitivityText.text = 'x' + mouseSensitivity.value.ToString();
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

	public void setFullscreen() {
		Screen.fullScreen = fullscreen.isOn;
	}

	public void setPostProcessEffects() {
		PlayerPrefs.SetInt("settings_effects_enabled", postProcessEffects.isOn ? 1 : 0);
	}

	public void mouseSensitivityChanged() {
		PlayerPrefs.SetFloat("mouse_sensitivity", mouseSensitivity.value);

		mouseSensitivityText.text = 'x' + mouseSensitivity.value.ToString();
	}
}
