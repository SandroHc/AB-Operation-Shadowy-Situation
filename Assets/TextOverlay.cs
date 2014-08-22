using UnityEngine;
using System.Collections;

public class TextOverlay : MonoBehaviour {
	public Font font;
	public Material material;
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.E)) {
			gameObject.AddComponent<MeshRenderer>();
			renderer.material = material;
			
			TextMesh tm = gameObject.AddComponent<TextMesh>();
			
			tm.text = "Some text to display";
			tm.font = font;
			tm.characterSize = 0.25f;
		}
	}
}
