using UnityEngine;
using System.Collections;

public class InteractionDialogue : Interaction {
	//public Dialogue dialogue;

	void Awake() {
		type = Type.Dialogue;
	}

	public override void doAction() {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(name).setPosition(gameObject.transform.position));

		GameController.dialogueManager.showDialogue(new DialogueTest());
	}
}
