using UnityEngine;
using System.Collections;

public class InteractionMaterial : Interaction {
	private GameController gameController;

	void Awake() {
		type = Type.PickUp;
		minDistance = 3f;

		gameController = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<GameController>();
	}

	public override void doAction(GameObject player) {
		gameController.setFocused(true);

		finishAction();
	}

	private void finishAction() {
		// Corountine for about ~3s

		gameController.setFocused(false);

		GameController.materialManager.increase(1);
	}
}
