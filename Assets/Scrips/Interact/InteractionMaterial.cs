using UnityEngine;
using System.Collections;

public class InteractionMaterial : Interaction {
	void Awake() {
		type = Type.PickUp;
		minDistance = 5;
	}

	public override void doAction(GameObject player) {
		GameController.materialManager.pickUp();
	}
}
