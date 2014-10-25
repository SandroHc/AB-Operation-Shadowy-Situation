using UnityEngine;
using System.Collections;

public class TextOverlay : MonoBehaviour {
	void Update() {
		if(Input.GetKeyDown(KeyCode.T)) {
			gameObject.AddComponent<MeshRenderer>();
			renderer.material = GameController.textManager.material;
			
			TextMesh tm = gameObject.AddComponent<TextMesh>();
			
			tm.text = "Some text to display";
			tm.font = GameController.textManager.font;
			tm.characterSize = 0.25f;
		}
	}
}
