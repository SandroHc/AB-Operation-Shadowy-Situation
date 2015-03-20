using UnityEngine;
using System.Collections;

public class InteractionDialogue : Interaction {
	//public Dialogue dialogue;

	void Awake() {
		type = Type.Dialogue;
		minDistance = 5;
	}

	public override void doAction(GameObject player) {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(destName).setPosition(gameObject.transform.position));

		GameController.dialogueManager.showDialogue(new DialogueTest());
	}
}
