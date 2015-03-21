using UnityEngine;
using System.Collections;

public class InteractionMaterial : Interaction {
	public float uses = 3;
	private float currentStage = 3;

	public MeshFilter mesh;

	void Awake() {
		type = Type.PickUp;
	}

	void Update() {
		if(uses < 3)
			uses += .4f * Time.deltaTime;

		if(currentStage != uses) {
			if(uses >= 3)
				mesh.mesh = GameController.materialManager.stage3;
			else if(uses >= 2)
				mesh.mesh = GameController.materialManager.stage2;
			else if(uses >= 1)
				mesh.mesh = GameController.materialManager.stage1;
			else
				mesh.mesh = GameController.materialManager.stageDepleted;

			currentStage = uses;
		}
	}

	public override void doAction() {
		GameController.questManager.fireProgressEvent(new QuestProgress(QuestProgress.ProgressType.INTERACTION).setStr(name).setPosition(gameObject.transform.position));

		if(uses >= 1) {
			uses--;
			GameController.materialManager.pickUp();
		}
	}
}
