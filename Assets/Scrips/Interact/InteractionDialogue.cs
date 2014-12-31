using UnityEngine;
using System.Collections;

public class InteractionDialogue : Interaction {
	private GameController gameController;

	void Start() {
		type = Type.Dialogue;
		minDistance = 3f;

		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
	}

	public override void doAction(GameObject player) {
		gameController.toggleInteracting();

		show = true;
	}

	private bool show = false;
	void OnGUI() {
		if(show)
			GUI.Box(new Rect(Screen.width / 6, Screen.height - 100, Screen.width / 1.5f, 50), "Dialogue... WIP");
	}
}
