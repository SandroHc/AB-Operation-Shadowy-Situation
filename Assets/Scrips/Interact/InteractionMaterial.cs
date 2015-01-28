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
			if(uses >= 3)
				gameObject.GetComponent<MeshFilter>().mesh = GameController.materialManager.stage3;
			else if(uses >= 2)
				gameObject.GetComponent<MeshFilter>().mesh = GameController.materialManager.stage2;
			else if(uses >= 1)
				gameObject.GetComponent<MeshFilter>().mesh = GameController.materialManager.stage1;
			else if(uses < 1)
				gameObject.GetComponent<MeshFilter>().mesh = GameController.materialManager.stageDepleted;
			
			currentStage = uses;
		}
	}

	public override void doAction(GameObject player) {
		if(uses >= 1) {
			uses--;
			GameController.materialManager.pickUp();
		}
	}
}
