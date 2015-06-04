using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputControl : MonoBehaviour {

	public Text label;
	public Text labelKey;

	public InputManager.InputKey key;

	public bool waitingForKey = false;

	void Update() {
		if(waitingForKey) {
			KeyCode pressedKey = InputManager.fetchKey();
			if(pressedKey != KeyCode.None) {
				key.keyCode = pressedKey;
				key.save();

				labelKey.text = pressedKey.ToString();

				// And reset the fetching state
				waitingForKey = false;
			}
		}
	}

	public void load() {
		if(label == null) label = gameObject.transform.FindChild("label").GetComponent<Text>();
		if(labelKey == null) labelKey = gameObject.transform.FindChild("button/text").GetComponent<Text>();

		label.text = key.label;
		labelKey.text = key.keyCode.ToString();
	}

	public void selectNewKey() {
		waitingForKey = true;

		labelKey.text = "Press key...";
	}
}
