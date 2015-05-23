using UnityEngine;
using UnityEngine.UI;
using Kender.uGUI;

public class PanelGraphics : MonoBehaviour {
	public ComboBox comboBoxResolution;
	public ComboBox comboBoxQuality;

	public Toggle fullscreen;

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

		//comboBoxResolution.UpdateGraphics();

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

		//comboBoxQuality.UpdateGraphics();
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
}
