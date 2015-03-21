using UnityEngine;
using System.Collections;

public class InteractPlayer : MonoBehaviour {
	public float range = 10f;

	// Update is called once per frame
	void Update() {
		if(GameController.isPausedOrFocused()) return; // Avoid showing any HUD during focus events (and useless calculations)

		RaycastHit hit;

		// Check if we're looking at an interactable entity
		if(Physics.Raycast(transform.position, transform.forward, out hit, range)) {
			Interaction other = (Interaction) hit.transform.gameObject.GetComponent(typeof(Interaction));

			if(other != null) {
				if(hit.distance <= other.minDistance)
					text = other.type + " available. Press E to interact with " + other.name;
				else
					text = other.type + " available, but not in range";

				if(Input.GetKeyDown(InputManager.interact))
					other.doAction(this.gameObject);
			} else
				text = "";
		}
	}

	private string text = "";

	void OnGUI() {
		if(!GameController.isPausedOrFocused() && !text.Equals(""))
			GUI.Box(new Rect(Screen.width / 6, Screen.height - 50, Screen.width / 1.5f, 50), text);
	}
}
