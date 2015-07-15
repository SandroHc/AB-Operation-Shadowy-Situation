using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCController : MonoBehaviour {
	public new string name;

	//public GameObject uiCanvas;
	public Text uiName;

	void Awake() {
		uiName.text = name;
	}

	//void Update() {
	//	if(Camera.main != null && Camera.main.transform.hasChanged) { // When in cutscenes, the main camera is disabled
	//		gameObject.transform.forward = Camera.main.transform.forward; // Keeps the text facing the player

	//		Quaternion rotation = Quaternion.Euler(0, Camera.main.transform.localRotation.y, 0);
	//		gameObject.transform.localRotation = rotation;
	//	}
	//}
}