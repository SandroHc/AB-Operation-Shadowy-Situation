using UnityEngine;
using System.Collections;

public class OptionsController : MonoBehaviour {

	public enum PANEL { GRAPHICS, SOUND, INPUT }
	private PANEL panel = PANEL.GRAPHICS;

	public RectTransform graphicsPanel;
	public RectTransform soundPanel;
	public RectTransform inputPanel;

	public void buttonClickedGraphics() {
		changePanel(PANEL.GRAPHICS);
	}

	public void buttonClickedSound() {
		changePanel(PANEL.SOUND);
	}

	public void buttonClickedInput() {
		changePanel(PANEL.INPUT);
	}

	private void changePanel(PANEL newPanel) {
		// Prevent switching to the same panel
		if(newPanel.Equals(panel))
			return;

		// Hide the old panel
		updatePanel(panel, false);
		// And show the new one
		updatePanel(newPanel, true);

		panel = newPanel;
	}

	private void updatePanel(PANEL panel, bool active) {
		switch(panel) {
		case PANEL.GRAPHICS: graphicsPanel.gameObject.SetActive(active); break;
		case PANEL.SOUND: soundPanel.gameObject.SetActive(active); break;
		case PANEL.INPUT: inputPanel.gameObject.SetActive(active); break;
		}
	}
}
