using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelInput : MonoBehaviour {

	public GameObject inputPrefab;

	public RectTransform content;

	void Start() {
		float y = -10;

		foreach(KeyValuePair<string, InputManager.InputKey> key in InputManager.keyList) {
			GameObject obj = Instantiate(inputPrefab);
			InputControl input = obj.GetComponent<InputControl>();
			if(input == null) return;

			input.key = key.Value;
			input.load();

			RectTransform rt = input.transform as RectTransform;
			rt.SetParent(content, true);
			rt.SetLeftTopPosition(new Vector2(-280, y));

			y -= rt.GetHeight() + 15;
		}
	}
}
