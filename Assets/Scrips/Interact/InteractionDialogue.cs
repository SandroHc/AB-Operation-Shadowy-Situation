using UnityEngine;
using System.Collections;

public class InteractionDialogue : Interaction {
	//public Dialogue dialogue;

	void Awake() {
		type = Type.Dialogue;
		minDistance = 5;
	}

	public override void doAction(GameObject player) {
		GameController.dialogueManager.showDialogue(new DialogueTest());
	}
}
