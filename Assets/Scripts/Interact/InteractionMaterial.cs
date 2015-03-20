using UnityEngine;
using System.Collections;

public class InteractionMaterial : Interaction {
	public float uses = 3;
	private float currentStage = 3;

	void Awake() {
		type = Type.PickUp;
		minDistance = 5;
	}

	void Update() {
		if(uses < 3)
			uses += .4f * Time.deltaTime;

		if(currentStage != uses) {
			Mesh newMesh;
			if(uses >= 3)
				newMesh = GameController.materialManager.stage3;
			else if(uses >= 2)
				newMesh = GameController.materialManager.stage2;
			else if(uses >= 1)
				newMesh = GameController.materialManager.stage1;
			else
				newMesh = GameController.materialManager.stageDepleted;

			gameObject.GetComponent<MeshFilter>().mesh = newMesh;
			currentStage = uses;
		}
	}

	public override void doAction(GameObject player) {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(destName).setPosition(gameObject.transform.position));

		if(uses >= 1) {
			uses--;
			GameController.materialManager.pickUp();
		}
	}
}
