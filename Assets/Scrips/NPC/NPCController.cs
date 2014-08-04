using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {
	public bool hover = false;
	public bool toggle = false;
	public float hSliderValue = 0;

	void OnGUI() {
		if(hover) {
			if(GUI.Button(new Rect(20, 20, 80, 20), "Click me!"))
				Application.LoadLevel(0);

			toggle = GUI.Toggle(new Rect(20, 40, 100, 20), toggle, "A Toggle text");
			hSliderValue = GUI.HorizontalSlider(new Rect(20, 60, 100, 20), hSliderValue, 0, 10);
		}
	}

	void OnTriggerEnter(Collider other) {
		hover = true;
	}

	void OnTriggerExit(Collider other) {
		hover = false;
	}
}